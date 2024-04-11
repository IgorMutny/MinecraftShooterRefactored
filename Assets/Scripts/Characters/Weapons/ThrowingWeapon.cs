using UnityEngine;

public class ThrowingWeapon : Weapon
{
    private ThrowingWeaponInfo _info;
    private GameObject _projectileSample;
    private float _spreadAngle;
    private float _cooldownTime;
    private bool _isReady;
    private TimerWrapper _timer;

    public ThrowingWeapon(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (ThrowingWeaponInfo)weaponInfo;

        _projectileSample = _info.Projectile;
        _spreadAngle = _info.SpreadAngle;
        _cooldownTime = _info.CooldownTime;

        _isReady = true;

        _timer = ServiceLocator.Get<TimerWrapper>();
    }

    public override void ChangeDamage(float multiplier) { }

    public override void ChangeSpeed(float multiplier)
    {
        _cooldownTime = _info.CooldownTime * multiplier;
    }

    public override void TryAttack()
    {
        if (_isReady == true && IsLocked == false)
        {
            Attack();
            Character.View.Attack();
            _isReady = false;
            _timer.AddSignal(_cooldownTime, AllowAttack);
        }
    }

    private void AllowAttack()
    {
        _isReady = true;
    }

    private void Attack()
    {
        Transform attackPoint = Character.AttackPoint;
        Vector3 angles = attackPoint.rotation.eulerAngles;
        angles.x += Random.Range(-_spreadAngle, _spreadAngle);
        angles.y += Random.Range(-_spreadAngle, _spreadAngle);
        Quaternion rotation = Quaternion.Euler(angles);

        Projectile projectile =
            GameObject.Instantiate(_projectileSample, attackPoint.position, rotation)
            .GetComponent<Projectile>();
        projectile.SetSender(Character);
    }
}
