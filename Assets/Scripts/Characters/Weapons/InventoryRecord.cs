public struct InventoryRecord
{
    public readonly WeaponInfo Weapon;
    public readonly int Rounds;

    public InventoryRecord(WeaponInfo weapon, int rounds)
    {
        Weapon = weapon;
        Rounds = rounds;
    }
}
