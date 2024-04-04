using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Loot")]
public class LootInfo : ScriptableObject
{
    [field: SerializeField] public GameObject Instance { get; private set; }
    [field: SerializeField] public string Message { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public AudioClip Clip { get; private set; }
    [field: SerializeField] public float LifeTime { get; private set; }
    [field: SerializeField] public EffectInfo Effect { get; private set; }
}
