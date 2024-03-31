using UnityEngine;

public abstract class WeaponInfo : ItemInfo
{
    [field: SerializeField] public Sprite Icon { get; private set; }
}
