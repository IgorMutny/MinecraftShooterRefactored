using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Firearm")]
public class FirearmInfo : WeaponInfo
{
    [field: SerializeField] public GameObject HandModel { get; private set; }
    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [field: SerializeField] public float ReloadTime { get; private set; }
    [field: SerializeField] public GameObject Projectile { get; private set; }
    [field: SerializeField] public int ProjectilesAmount { get; private set; }
    [field: SerializeField] public float SpreadAngle { get; private set; }
    [field: SerializeField] public int Rounds { get; private set; }
}
