public class SlownessEffect : Effect
{
    protected override void OnTickExtended()
    {
        if (_isActive == true)
        {
            _appliedEffects.MultiplySpeedMultiplier(_value);

        }
    }
}
