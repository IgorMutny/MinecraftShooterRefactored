using UnityEngine;

public class PlayerGun : Weapon
{
    private GameObject _handModel;
    private PlayerGunView _view;
    private TimerWrapper _timer;
    private float _speedMultiplier = 1f;

    private GameObject _projectileSample;
    private int _projectilesAmount;
    private float _spreadAngle;
    private float _cooldownTime;
    private float _reloadTime;

    private bool _isReady;
    private bool _isReloaded;
    private TimerSignal _cooldownSignal;
    private TimerSignal _reloadSignal;

    private PlayerGunInfo _info;

    public PlayerGun(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _timer = ServiceLocator.Get<TimerWrapper>();
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
        _cooldownTime = _info.CooldownTime;
        _reloadTime = _info.ReloadTime;

        ChangeDamage(Character.AppliedEffects.DamageMultiplier);
        ChangeSpeed(Character.AppliedEffects.SpeedMultiplier);

        _isReady = true;
        _isReloaded = true;
    }

    #region AttackMethods
    public override void TryAttack()
    {
        if (_isReady == true && _isReloaded == true && IsLocked == false) 
        {
            if (Inventory.HasRounds() == true)
            {
                Attack();
                _isReady = false;
               _cooldownSignal = _timer.AddSignal(_cooldownTime, SetReady, _speedMultiplier);
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

    private void SetReady()
    {
        _isReady = true;
        _cooldownSignal = null;
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

            if (_view != null)
            {
                _view.Reload();
            }

            _isReloaded = false;
            _reloadSignal = _timer.AddSignal(_reloadTime, Reload, _speedMultiplier);
        }
    }

    private void Reload()
    {
        Inventory.ReloadRounds();
        _isReloaded = true;
        _reloadSignal = null;
    }
    #endregion ReloadMethods

    public override void Remove(float time)
    {
        if (_view != null)
        {
            _view.Remove(time);
            _view.Removed += OnViewRemoved;
        }

        _timer.RemoveSignal(_cooldownSignal);
        _timer.RemoveSignal(_reloadSignal);
    }

    private void OnViewRemoved()
    {
        if (_view != null)
        {
            _view.Destroy();
        }
    }

    public override void Raise(float time)
    {
        if (_view != null)
        {
            _view.Raise(time);
        }
    }

    public override void ChangeSpeed(float multiplier)
    {
        if (_view != null)
        {
            _view.ChangeSpeed(multiplier);
        }

        if (_cooldownSignal != null)
        {
            _cooldownSignal.ChangeMultiplier(multiplier);
        }

        if (_reloadSignal != null)
        {
            _reloadSignal.ChangeMultiplier(multiplier);
        }

        _speedMultiplier = multiplier;
    }

    public override void ChangeDamage(float multiplier)
    {
        if (_view != null)
        {
            _view.ChangeDamage(multiplier);
        }
    }
}
