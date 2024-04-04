using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Loot Chance")]
public class DropInfo : ScriptableObject
{
    [field: SerializeField] public int GoldAmount { get; private set; }
    [field: SerializeField] public float DiamondChance { get; private set; }
    [field: SerializeField] public float MedKitChance { get; private set; }
    [field: SerializeField] public float PowerupChance { get; private set; }
}
