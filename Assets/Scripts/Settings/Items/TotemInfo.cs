using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Totem")]
public class TotemInfo : ItemInfo
{
    [field: SerializeField] public TotemType TotemType { get; private set; }
}
