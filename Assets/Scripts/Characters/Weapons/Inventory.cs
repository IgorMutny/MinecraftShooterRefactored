using System;

public class Inventory
{
    private Character _character;

    private InventoryRecord[] _records;
    private Weapon _weapon;

    private int _currentRecord;
    private bool _isLocked;

    private float _switchWeaponTime = 0.5f;
    private TimerWrapper _timer;
    private int _nextIndex;

    public event Action<int> MaxRoundsChanged;
    public event Action<int> RoundsChanged;
    public event Action<int> WeaponChanged;

    public Inventory(Character character, CharacterInfo characterInfo)
    {
        _character = character;
        _isLocked = false;
        _timer = ServiceLocator.Get<TimerWrapper>();

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

        _timer = ServiceLocator.Get<TimerWrapper>();

        FillInventory(gameDataService, itemInfoCollection);
        
        _timer.AddSignal(0.1f, () => TryTakeWeapon(GetBestWeaponIndex()));
    }

    private void FillInventory(IReadOnlyGameDataService gameDataService, ItemInfoCollection itemInfoCollection)
    {
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
        _isLocked = true;

        if (_weapon != null)
        {
            _weapon.Lock();
            _weapon.Remove(_switchWeaponTime / 2);
        }

        _nextIndex = index;
        _timer.AddSignal(_switchWeaponTime / 2, OnWeaponTaken);
    }

    private void OnWeaponTaken()
    {
        _currentRecord = _nextIndex;

        if (_records[_currentRecord].Weapon is PlayerGunInfo gun)
        {
            _weapon = new PlayerGun(_character, this, _records[_currentRecord].Weapon);
            WeaponChanged?.Invoke(_currentRecord);
            MaxRoundsChanged?.Invoke(gun.Rounds);
            RoundsChanged?.Invoke(_records[_currentRecord].Rounds);
        }

        if (_records[_currentRecord].Weapon is MeleeInfo)
        {
            _weapon = new Melee(_character, this, _records[_currentRecord].Weapon);
        }

        if (_records[_currentRecord].Weapon is ThrowingWeaponInfo)
        {
            _weapon = new ThrowingWeapon(_character, this, _records[_currentRecord].Weapon);
        }

        if (_records[_currentRecord].Weapon is SuicideBombingInfo)
        {
            _weapon = new SuicideBombing(_character, this, _records[_currentRecord].Weapon);
        }

        _weapon.Lock();
        _weapon.Raise(_switchWeaponTime / 2);
        _timer.AddSignal(_switchWeaponTime / 2, OnWeaponReady);
    }

    private void OnWeaponReady()
    {
        _weapon.Unlock();
        _isLocked = false;
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
