using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Characters Collection")]
public class CharacterInfoCollection : ScriptableObject
{
    [field: SerializeField] public CharacterInfo[] Characters { get; private set; }
}
