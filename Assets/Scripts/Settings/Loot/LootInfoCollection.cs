using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Loot Collection")]
public class LootInfoCollection : ScriptableObject
{
    [field: SerializeField] public LootInfo Diamond { get; private set; }
    [field: SerializeField] public LootInfo MedKit { get; private set; }
    [field: SerializeField] public LootInfo[] Powerups { get; private set; }
    [field: SerializeField] public GameObject Beam { get; private set; }
}
