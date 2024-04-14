using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Character")]
public class CharacterInfo : ScriptableObject
{
    [field: SerializeField] public GameObject Instance { get; private set; }
    [field: SerializeField] public MovementType MovementType { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public Vector2 HeadMaxAngles { get; private set; }
    [field: SerializeField] public WeaponInfo Weapon { get; private set; }
    [field: SerializeField] public ArmorInfo Armor { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public DropInfo LootChance { get; private set; }
    [field: SerializeField] public AIInfo AI { get; private set; }
    [field: SerializeField] public CharacterAudioInfo Audio { get; private set; }
}
