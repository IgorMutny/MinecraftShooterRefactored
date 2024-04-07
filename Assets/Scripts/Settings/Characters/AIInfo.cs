using UnityEngine;

[CreateAssetMenu(menuName ="Settings/AI")]
public class AIInfo : ScriptableObject
{
    [field: SerializeField] public bool DashingMovement {  get; private set; }
    [field: SerializeField] public float DashDuration { get; private set; }
    [field: SerializeField] public float DelayBetweenDashes { get; private set; }
    [field: SerializeField] public float StrafeDuration { get; private set; }
    [field: SerializeField] public float DistanceToAttack { get; private set; }
    [field: SerializeField] public float DelayBetweenAttacks { get; private set; }
    [field: SerializeField] public bool CanFlee { get; private set; }
    [field: SerializeField] public float DistanceToFlee { get; private set; }
    [field: SerializeField] public float PreferredFlightHeight { get; private set; }
}
