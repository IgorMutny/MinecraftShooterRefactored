using UnityEngine;

public class Firearm : Weapon
{
    private GameObject _handModel;
    private FirearmView _firearmView;

    private GameObject _projectileSample;
    private int _projectilesAmount;
    private float _spreadAngle;
    private float _cooldownTime;
    private float _reloadTime;

    private bool _isReady;
    private bool _isReloaded;

    private FirearmInfo _firearmInfo;

    public Firearm(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _firearmInfo = (FirearmInfo)weaponInfo;

        if (_firearmInfo.HandModel != null)
        {
            _handModel = GameObject.Instantiate(_firearmInfo.HandModel, character.Head);
            _handModel.transform.Translate(_firearmInfo.Position, Space.Self);
            _firearmView = _handModel.GetComponent<FirearmView>();

            bool reloadRpgStyle = _firearmInfo.ReloadRPGStyle;
            if (reloadRpgStyle == true && Inventory.HasRounds() == false)
            {
                _firearmView.SetEmpty();
            }
        }

        _projectileSample = _firearmInfo.Projectile;
        _projectilesAmount = _firearmInfo.ProjectilesAmount;
        _spreadAngle = _firearmInfo.SpreadAngle;

        _isReady = true;
        _isReloaded = true;
    }

    #region OnTickMethods
    public override void OnTick()
    {
        if (IsLocked == false)
        {
            DecreaseCoolDownTime();
            DecreaseReloadTime();
        }
    }

    private void DecreaseCoolDownTime()
    {
        if (_cooldownTime > 0)
        {
            _cooldownTime -= Time.fixedDeltaTime * SpeedMultiplier;
        }

        if (_cooldownTime <= 0)
        {
            _cooldownTime = 0;
            _isReady = true;
        }
    }

    private void DecreaseReloadTime()
    {
        if (_reloadTime > 0)
        {
            _reloadTime -= Time.fixedDeltaTime * SpeedMultiplier;
        }

        if (_reloadTime <= 0 && _isReloaded == false)
        {
            Reload();
            _reloadTime = 0;
            _isReloaded = true;
        }
    }
    #endregion OnTickMethods

    #region AttackMethods
    public override void TryAttack()
    {
        if (_isReady == true && _isReloaded == true 
            && IsLocked == false) 
        {
            if (Inventory.HasRounds() == true)
            {
                Attack();
                _isReady = false;
                _cooldownTime = _firearmInfo.CooldownTime;

                if (Inventory.HasRounds() == true)
                {
                    _cooldownTime = _firearmInfo.CooldownTime;
                }
            }
            else
            {
                TryReload();
            }
        }
    }

    private void Attack()
    {
        for (int i = 0; i < _projectilesAmount; i++)
        {
            CreateProjectile();
        }

        if (_firearmView != null)
        {
            _firearmView.Shoot();
        }

        Inventory.DecreaseRounds();
    }

    private void CreateProjectile()
    {
        Transform head = Character.Head;
        Vector3 angles = head.rotation.eulerAngles;
        angles.x += Random.Range(-_spreadAngle, _spreadAngle);
        angles.y += Random.Range(-_spreadAngle, _spreadAngle);
        Quaternion rotation = Quaternion.Euler(angles);

        Projectile projectile = 
            GameObject.Instantiate(_projectileSample, head.position, rotation)
            .GetComponent<Projectile>();
        projectile.SetSender(Character);
    }
    #endregion AttackMethods

    #region ReloadMethods
    public override void TryReload()
    {
        if (_isReloaded == true && IsLocked == false)
        {
            if (Inventory.IsFull() == true)
            {
                return;
            }

            OnReload();
            _isReloaded = false;
            _reloadTime = _firearmInfo.ReloadTime;
        }
    }

    private void OnReload()
    {
        if (_firearmView != null)
        {
            _firearmView.Reload();
        }
    }

    private void Reload()
    {
        Inventory.ReloadRounds();
    }
    #endregion ReloadMethods

    public override void Remove(float time)
    {
        if (_firearmView != null)
        {
            _firearmView.Remove(time);
            _firearmView.Removed += OnRemoved;
        }
    }

    private void OnRemoved()
    {
        GameObject.Destroy(_handModel);
    }

    public override void Raise(float time)
    {
        if (_firearmView != null)
        {
            _firearmView.Raise(time);
            _firearmView.ChangeDamage(Character.AppliedEffects.DamageMultiplier);
        }
    }

    public override void ChangeSpeed(float multiplier)
    {
        if (_firearmView != null)
        {
            _firearmView.ChangeSpeed(multiplier);
        }
    }

    public override void ChangeDamage(float multiplier)
    {
        if (_firearmView != null)
        {
            _firearmView.ChangeDamage(multiplier);
        }
    }
}
