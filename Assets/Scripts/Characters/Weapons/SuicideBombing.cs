using UnityEngine;

public class SuicideBombing : Weapon
{
    private SuicideBombingInfo _info;
    private GameObject _explosionSample;
    private float _delayBeforeExplosion;
    private bool _activated;
    private TimerWrapper _timer;
    private TimerSignal _timerSignal;

    public SuicideBombing(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (SuicideBombingInfo)weaponInfo;
        _explosionSample = _info.Explosion;
        _delayBeforeExplosion = _info.DelayBeforeExplosion;
        _activated = false;
        _timer = ServiceLocator.Get<TimerWrapper>();
    }

    public override void ChangeDamage(float multiplier) { }

    public override void ChangeSpeed(float multiplier) { }

    public override void TryAttack()
    {
        if (_activated == false)
        {
            {
                _timerSignal = _timer.AddSignal(_delayBeforeExplosion, Explode);
                _activated = true;
                Character.View.Explode(_delayBeforeExplosion);
            }
        }
    }

    private void Explode()
    {
        Explosion explosion =
            GameObject.Instantiate(_explosionSample, Character.transform.position, Quaternion.identity)
            .GetComponent<Explosion>();
        explosion.SetSender(Character);
        explosion.Activate();
    }

    public override void Remove(float time)
    {
        _timer.RemoveSignal(_timerSignal);
    }
}
