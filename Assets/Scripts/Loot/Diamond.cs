public class Diamond : Loot
{
    protected override void Apply(Character character)
    {
        ServiceLocator.Get<LootCollection>().AddDiamond();
    }
}
