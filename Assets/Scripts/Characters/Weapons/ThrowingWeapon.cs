using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeapon : Weapon
{
    private ThrowingWeaponInfo _info;
    private GameObject _projectileSample;
    private float _spreadAngle;
    private float _cooldownCounter;
    private bool _isReady;

    public ThrowingWeapon(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (ThrowingWeaponInfo)weaponInfo;

        _projectileSample = _info.Projectile;
        _spreadAngle = _info.SpreadAngle;
        _cooldownCounter = _info.CooldownTime;

        _isReady = true;

    }

    public override void ChangeDamage(float multiplier)
    {

    }

    public override void ChangeSpeed(float multiplier)
    {

    }

    public override void OnTick()
    {
        if (IsLocked == false)
        {
            DecreaseCoolDownTime();
        }
    }

    private void DecreaseCoolDownTime()
    {
        if (_cooldownCounter > 0)
        {
            _cooldownCounter -= Time.fixedDeltaTime * SpeedMultiplier;

            if (_cooldownCounter <= 0)
            {
                _cooldownCounter = 0;
                _isReady = true;
            }
        }
    }

    public override void TryAttack()
    {
        if (_isReady == true && IsLocked == false)
        {
            Attack();
            _isReady = false;
            _cooldownCounter = _info.CooldownTime;
        }
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
