using UnityEngine;

public class PoisoningEffect : Effect
{
    protected override void OnTickExtended()
    {
        if (_periodCounter <= 0)
        {
            _character.Health.GetDamage((int)_value, DamageType.Poison, null);
            _periodCounter = _period;
        }

        _periodCounter -= Time.fixedDeltaTime;
    }
}

