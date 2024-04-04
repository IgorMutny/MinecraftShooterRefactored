public class QuadDamageEffect : Effect
{
    protected override void OnTickExtended()
    {
        _appliedEffects.MultiplyDamageMultiplier(_value);
    }
}
