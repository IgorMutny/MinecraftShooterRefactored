using UnityEngine;

public abstract class Weapon
{
    protected Character Character { get; private set; }
    protected Inventory Inventory { get; private set; }
    protected WeaponInfo WeaponInfo { get; private set; }

    protected bool IsLocked { get; private set; }

    protected float SpeedMultiplier => Mathf.Max(1f, Character.SpeedMultiplier);

    public Weapon(Character character, Inventory inventory, WeaponInfo weaponInfo)
    {
        Character = character;
        Inventory = inventory;
        WeaponInfo = weaponInfo;
        IsLocked = false;
    }

    public abstract void OnTick();

    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }

    public abstract void TryAttack();

    public virtual void TryReload() { }

    public virtual void Remove(float time) { }

    public virtual void Raise(float time) { }


}
