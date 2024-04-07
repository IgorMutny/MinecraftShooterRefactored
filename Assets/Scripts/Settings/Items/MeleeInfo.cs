using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Melee")]
public class MeleeInfo : WeaponInfo
{
    [field: SerializeField] public float MinDamage { get; private set; }
    [field: SerializeField] public float MaxDamage { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [field: SerializeField] public float DelayBeforeDamaging { get; private set; }
}
