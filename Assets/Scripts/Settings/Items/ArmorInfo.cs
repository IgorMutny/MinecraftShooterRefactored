using UnityEngine;

[CreateAssetMenu(menuName ="Settings/Armor")]
public class ArmorInfo : ItemInfo
{
    [field: SerializeField] public float PhysicalDefence { get; private set; }
    [field: SerializeField] public float MagicalDefence { get; private set; }
}
