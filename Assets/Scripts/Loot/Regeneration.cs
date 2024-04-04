public class Regeneration : Loot
{
    protected override void Apply(Character character)
    {
        character.AppliedEffects.TryAddEffect<RegenerationEffect>(
            Info.Effect.Duration, Info.Effect.Period, Info.Effect.Value);
    }
}
