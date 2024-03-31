using System;

public interface IReadOnlyInventory
{
    public event Action<int> MaxRoundsChanged;
    public event Action<int> RoundsChanged;
    public event Action<int> WeaponChanged;

    public WeaponInfo[] GetAllWeapons();
}
