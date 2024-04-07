using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Effect")]
public class EffectInfo : ScriptableObject
{
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public float Period { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
    [field: SerializeField] public GameObject Smoke { get; private set; }
}
