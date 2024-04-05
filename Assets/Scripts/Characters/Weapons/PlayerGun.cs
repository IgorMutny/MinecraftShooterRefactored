using UnityEngine;

public class PlayerGun : Weapon
{
    private GameObject _handModel;
    private PlayerGunView _view;

    private GameObject _projectileSample;
    private int _projectilesAmount;
    private float _spreadAngle;
    private float _cooldownTime;
    private float _reloadTime;

    private bool _isReady;
    private bool _isReloaded;

    private PlayerGunInfo _info;

    public PlayerGun(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (PlayerGunInfo)weaponInfo;

        if (_info.HandModel != null)
        {
            _handModel = GameObject.Instantiate(_info.HandModel, character.Head);
            _handModel.transform.Translate(_info.Position, Space.Self);
            _view = _handModel.GetComponent<PlayerGunView>();

            bool reloadRpgStyle = _info.ReloadRPGStyle;
            if (reloadRpgStyle == true && Inventory.HasRounds() == false)
            {
                _view.SetEmpty();
            }
        }

        _projectileSample = _info.Projectile;
        _projectilesAmount = _info.ProjectilesAmount;
        _spreadAngle = _info.SpreadAngle;

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
                _cooldownTime = _info.CooldownTime;

                if (Inventory.HasRounds() == true)
                {
                    _cooldownTime = _info.CooldownTime;
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

        if (_view != null)
        {
            _view.Shoot();
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
            _reloadTime = _info.ReloadTime;
        }
    }

    private void OnReload()
    {
        if (_view != null)
        {
            _view.Reload();
        }
    }

    private void Reload()
    {
        Inventory.ReloadRounds();
    }
    #endregion ReloadMethods

    public override void Remove(float time)
    {
        if (_view != null)
        {
            _view.Remove(time);
            _view.Removed += OnRemoved;
        }
    }

    private void OnRemoved()
    {
        GameObject.Destroy(_handModel);
    }

    public override void Raise(float time)
    {
        if (_view != null)
        {
            _view.Raise(time);
            _view.ChangeDamage(Character.AppliedEffects.DamageMultiplier);
        }
    }

    public override void ChangeSpeed(float multiplier)
    {
        if (_view != null)
        {
            _view.ChangeSpeed(multiplier);
        }
    }

    public override void ChangeDamage(float multiplier)
    {
        if (_view != null)
        {
            _view.ChangeDamage(multiplier);
        }
    }
}
