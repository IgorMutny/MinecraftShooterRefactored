public class QuadDamageEffect : Effect
{
    protected override void OnTickExtended()
    {
        if (_isActive == true)
        {
            _appliedEffects.MultiplyDamageMultiplier(_value);
        }
    }
}
