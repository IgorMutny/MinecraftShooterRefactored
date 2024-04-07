using UnityEngine;

public class SuicideBombing : Weapon
{
    private SuicideBombingInfo _info;
    private GameObject _explosionSample;
    private float _delayCounter;
    private bool _activated;

    public SuicideBombing(Character character, Inventory inventory, WeaponInfo weaponInfo)
        : base(character, inventory, weaponInfo)
    {
        _info = (SuicideBombingInfo)weaponInfo;
        _explosionSample = _info.Explosion;
        _delayCounter = 0;
        _activated = false;
    }

    public override void ChangeDamage(float multiplier)
    {

    }

    public override void ChangeSpeed(float multiplier)
    {

    }

    public override void OnTick()
    {
        if (_delayCounter > 0)
        {
            _delayCounter -= Time.fixedDeltaTime;

            if (_delayCounter <= 0)
            {
                Explode();
            }
        }
    }

    public override void TryAttack()
    {
        if (_activated == false)
        {
            {
                _delayCounter = _info.DelayBeforeExplosion;
                _activated = true;
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
}
