using UnityEngine;

public class Melee : Weapon
{
    private MeleeInfo _info;
    private bool _isReady;
    private float _minDamage;
    private float _maxDamage;
    private float _attackRange;
    private float _cooldownCounter;
    private float _delayBeforeDamagingCounter;


    public Melee(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (MeleeInfo)weaponInfo;

        _maxDamage = _info.MaxDamage;
        _minDamage = _info.MinDamage;
        _attackRange = _info.AttackRange;
        _cooldownCounter = _info.CooldownTime;
        _delayBeforeDamagingCounter = 0;

        _isReady = true;
    }

    public override void ChangeDamage(float multiplier) { }

    public override void ChangeSpeed(float multiplier) { }

    public override void OnTick()
    {
        if (IsLocked == false)
        {
            DecreaseCoolDownTime();
            DecreaseDelayBeforeDamagingTime();
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

    private void DecreaseDelayBeforeDamagingTime()
    {
        if (_delayBeforeDamagingCounter > 0)
        {
            _delayBeforeDamagingCounter -= Time.fixedDeltaTime * SpeedMultiplier;

            if (_delayBeforeDamagingCounter <= 0)
            {
                _delayBeforeDamagingCounter = 0;
                DoDamage();
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
        _delayBeforeDamagingCounter = _info.DelayBeforeDamaging;
    }

    private void DoDamage()
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(Character.AttackPoint.position, Character.AttackPoint.forward, _attackRange);

        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.gameObject.TryGetComponent(out Character victim) == true)
            {
                victim.Health.GetDamage(GetRandomDamage(), DamageType.Physical, Character);
            }
        }
    }

    private int GetRandomDamage()
    {
        float damageMultiplier = Character.AppliedEffects.DamageMultiplier;

        int minDamage = (int)(_minDamage * damageMultiplier);
        int maxDamage = (int)(_maxDamage * damageMultiplier) + 1;

        return Random.Range(minDamage, maxDamage);
    }
}

