public class HasteEffect : Effect
{
    protected override void OnTickExtended()
    {
        _appliedEffects.MultiplySpeedMultiplier(_value);
    }
}
