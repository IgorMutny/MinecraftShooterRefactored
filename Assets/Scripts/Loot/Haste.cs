public class Haste : Loot
{
    protected override void Apply(Character character)
    {
        character.AppliedEffects.TryAddEffect<HasteEffect>(Info.Effect);
    }
}
