using UnityEngine;

public class Melee : Weapon
{
    private MeleeInfo _info;
    private bool _isReady;
    private float _minDamage;
    private float _maxDamage;
    private float _attackRange;
    private float _cooldownTime;
    private float _delayBeforeDamaging;
    private TimerWrapper _timer;

    public Melee(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (MeleeInfo)weaponInfo;
        _timer = ServiceLocator.Get<TimerWrapper>();

        _maxDamage = _info.MaxDamage;
        _minDamage = _info.MinDamage;
        _attackRange = _info.AttackRange;
        _cooldownTime = _info.CooldownTime;
        _delayBeforeDamaging = _info.DelayBeforeDamaging;

        _isReady = true;
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
            _timer.AddSignal(_delayBeforeDamaging, DoDamage);
            _timer.AddSignal(_cooldownTime, AllowAttack);
            Character.View.Attack();
            _isReady = false;
            _cooldownTime = _info.CooldownTime;
        }
    }

    private void AllowAttack()
    {
        _isReady = true;
    }

    private void DoDamage()
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(Character.AttackPoint.position, Character.AttackPoint.forward, _attackRange);

        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.gameObject.TryGetComponent(out Character victim) == true)
            {
                victim.Health.GetDamage(GetRandomDamage(), DamageType.Physical, Character);

                if (_info.PoisoningEffect != null)
                {
                    victim.AppliedEffects.TryAddEffect<PoisoningEffect>(_info.PoisoningEffect);
                }
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

