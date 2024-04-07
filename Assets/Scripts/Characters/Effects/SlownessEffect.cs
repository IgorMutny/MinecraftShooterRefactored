public class SlownessEffect : Effect
{
    protected override void OnTickExtended()
    {
        _appliedEffects.MultiplySpeedMultiplier(_value);
    }
}
