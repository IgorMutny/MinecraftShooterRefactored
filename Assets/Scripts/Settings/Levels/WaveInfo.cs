using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Wave Info")]
public class WaveInfo : ScriptableObject
{
    [field: SerializeField] public WaveRecord[] Records { get; private set; }
}
