using System;
using UnityEngine;

public class Inventory
{
    private Character _character;

    private InventoryRecord[] _records;
    private Weapon _weapon;

    private int _currentRecord;
    private bool _isLocked;

    private float _switchWeaponTime = 0.5f;
    private SwitchWeaponTimer _switchWeaponTimer;

    public event Action<int> MaxRoundsChanged;
    public event Action<int> RoundsChanged;
    public event Action<int> WeaponChanged;

    public Inventory(Character character, CharacterInfo characterInfo)
    {
        _character = character;
        _isLocked = false;

        _records = new InventoryRecord[1];
        AddRecord(0, characterInfo.Weapon);

        TryTakeWeapon(GetBestWeaponIndex());
    }

    public Inventory(Character character, IReadOnlyGameDataService gameDataService)
    {
        _character = character;
        _isLocked = false;

        ItemInfoCollection itemInfoCollection =
            ServiceLocator.Get<SettingsService>().Get<ItemInfoCollection>();

        _records = new InventoryRecord[itemInfoCollection.Weapons.Length];

        int recordIndex = 0;

        for (int i = 0; i < _records.Length; i++)
        {
            int id = itemInfoCollection.Weapons[i].Id;
            if (gameDataService.HasItem(id) == true)
            {
                WeaponInfo weaponInfo = (WeaponInfo)itemInfoCollection.Weapons[i];
                AddRecord(recordIndex, weaponInfo);
                recordIndex += 1;
            }
        }

        TryTakeWeapon(GetBestWeaponIndex());
    }

    private void AddRecord(int index, WeaponInfo weapon)
    {
        InventoryRecord record;
        if (weapon is PlayerGunInfo gun)
        {
            record = new InventoryRecord(weapon, gun.Rounds);
        }
        else
        {
            record = new InventoryRecord(weapon, 1);
        }

        _records[index] = record;
    }

    public void OnTick()
    {
        if (_switchWeaponTimer != null)
        {
            _switchWeaponTimer.OnTick();
        }

        if (_weapon != null)
        {
            _weapon.OnTick();
        }
    }

    public void OnDied()
    {
        if (_weapon != null)
        {
            _weapon.Lock();
            _weapon.Remove(_switchWeaponTime / 2);
        }
    }

    public void ChangeWeaponSpeed(float multiplier)
    {
        if (_weapon != null)
        {
            _weapon.ChangeSpeed(multiplier);
        }
    }

    public void ChangeWeaponDamage(float multiplier)
    {
        if (_weapon != null)
        {
            _weapon.ChangeDamage(multiplier);
        }
    }

    public void SetInput(bool isAttacking, bool isReloading,
        int numericWeaponIndex, int prevNextWeaponIndex)
    {
        if (_weapon != null)
        {
            if (isAttacking == true)
            {
                _weapon.TryAttack();
            }

            if (isReloading == true)
            {
                _weapon.TryReload();
            }
        }

        if (numericWeaponIndex != -1)
        {
            TryTakeWeaponByIndex(numericWeaponIndex);
        }

        if (prevNextWeaponIndex != 0)
        {
            TryTakeWeaponByDirection(prevNextWeaponIndex);
        }
    }

    private void TryTakeWeaponByIndex(int index)
    {
        if (index >= _records.Length)
        {
            return;
        }

        if (_records[index].Weapon != null && _currentRecord != index && _isLocked == false)
        {
            TryTakeWeapon(index);
        }
    }

    private void TryTakeWeaponByDirection(int direction)
    {
        int index = _currentRecord;
        //do
        //{
        //    index += direction;
        //    if (index >= _records.Length)
        //    {
        //        index = 0;
        //    }
        //    if (index < _records.Length)
        //    {
        //        index = _records.Length - 1;
        //    }
        //}
        //while (_records[index].Weapon == null);

        //TryTakeWeapon(index);
    }

    private void TryTakeWeapon(int index)
    {
        if (_weapon != null)
        {
            _weapon.Lock();
            _weapon.Remove(_switchWeaponTime / 2);
        }

        _switchWeaponTimer = new SwitchWeaponTimer(_switchWeaponTime, index);
        _switchWeaponTimer.WeaponTaken += OnWeaponTaken;
        _switchWeaponTimer.WeaponReady += OnWeaponReady;
    }

    private void OnWeaponTaken(int index)
    {
        _currentRecord = index;

        if (_records[index].Weapon is PlayerGunInfo gun)
        {
            _weapon = new PlayerGun(_character, this, _records[index].Weapon);
            WeaponChanged?.Invoke(_currentRecord);
            MaxRoundsChanged?.Invoke(gun.Rounds);
            RoundsChanged?.Invoke(_records[index].Rounds);
        }

        if (_records[index].Weapon is MeleeInfo)
        {
            _weapon = new Melee(_character, this, _records[index].Weapon);
        }

        if (_records[index].Weapon is ThrowingWeaponInfo)
        {
            _weapon = new ThrowingWeapon(_character, this, _records[index].Weapon);
        }

        if (_records[index].Weapon is SuicideBombingInfo)
        {
            _weapon = new SuicideBombing(_character, this, _records[index].Weapon);
        }

        _weapon.Lock();
        _weapon.Raise(_switchWeaponTime);
    }

    private void OnWeaponReady()
    {
        _switchWeaponTimer.WeaponTaken -= OnWeaponTaken;
        _switchWeaponTimer.WeaponReady -= OnWeaponReady;
        _switchWeaponTimer = null;
        _weapon.Unlock();
    }

    public void DecreaseRounds()
    {
        if (_records[_currentRecord].Rounds > 0)
        {
            WeaponInfo weaponInfo = _records[_currentRecord].Weapon;
            int rounds = _records[_currentRecord].Rounds - 1;
            _records[_currentRecord] = new InventoryRecord(weaponInfo, rounds);

            RoundsChanged?.Invoke(rounds);
        }
    }

    public bool HasRounds()
    {
        return _records[_currentRecord].Rounds > 0;
    }

    public bool IsFull()
    {
        if (_records[_currentRecord].Weapon is PlayerGunInfo firearm)
        {
            return _records[_currentRecord].Rounds == firearm.Rounds;
        }
        else
        {
            return true;
        }
    }

    public void ReloadRounds()
    {
        if (_records[_currentRecord].Weapon is PlayerGunInfo firearm)
        {
            int rounds = firearm.Rounds;
            _records[_currentRecord] = new InventoryRecord(firearm, rounds);

            RoundsChanged?.Invoke(rounds);
        }
    }

    private int GetBestWeaponIndex()
    {
        int index = 0;
        for (int i = 0; i < _records.Length; i++)
        {
            if (_records[i].Weapon != null)
            {
                index = i;
            }
        }

        return index;
    }

    public WeaponInfo[] GetAllWeapons()
    {
        WeaponInfo[] result = new WeaponInfo[_records.Length];

        for (int i = 0; i < _records.Length; i++)
        {
            result[i] = _records[i].Weapon;
        }

        return result;
    }
}
