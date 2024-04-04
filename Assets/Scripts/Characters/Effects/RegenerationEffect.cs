using UnityEngine;

public class RegenerationEffect : Effect
{
    protected override void OnTickExtended()
    {
        if (_periodCounter <= 0)
        {
            _character.Health.GetCure((int)_value);
            _periodCounter = _period;
        }

        _periodCounter -= Time.fixedDeltaTime;
    }
}
