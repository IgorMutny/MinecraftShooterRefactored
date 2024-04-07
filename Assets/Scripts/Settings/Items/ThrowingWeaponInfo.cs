using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Throwing Weapon")]
public class ThrowingWeaponInfo : WeaponInfo
{
    [field: SerializeField] public GameObject Projectile { get; private set; }
    [field: SerializeField] public float SpreadAngle { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
}
