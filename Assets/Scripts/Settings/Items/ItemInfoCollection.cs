using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Items Collection")]
public class ItemInfoCollection : ScriptableObject
{
    [field: SerializeField] public ItemInfo[] Weapons { get; private set; }
    [field: SerializeField] public ItemInfo[] Armors { get; private set; }
    [field: SerializeField] public ItemInfo[] Totems { get; private set; }
}
