public class QuadDamage : Loot
{
    protected override void Apply(Character character)
    {
        character.AppliedEffects.TryAddEffect<QuadDamageEffect>(
            Info.Effect.Duration, Info.Effect.Period, Info.Effect.Value);
    }
}
