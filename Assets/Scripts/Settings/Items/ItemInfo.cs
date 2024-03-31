using UnityEngine;

public abstract class ItemInfo : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int PriceInGold { get; private set; }
    [field: SerializeField] public int PriceInDiamonds { get; private set; }
    [field: SerializeField] public GameObject Model { get; private set; }
}
