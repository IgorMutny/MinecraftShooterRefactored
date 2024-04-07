using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Potion")]
public class PotionInfo : ScriptableObject
{
    [field: SerializeField] public GameObject Explosion { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}
