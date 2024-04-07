public class Regeneration : Loot
{
    protected override void Apply(Character character)
    {
        character.AppliedEffects.TryAddEffect<RegenerationEffect>(Info.Effect);
    }
}
