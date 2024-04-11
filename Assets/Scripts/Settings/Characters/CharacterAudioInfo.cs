using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Character audio")]
public class CharacterAudioInfo : ScriptableObject
{
    [field: SerializeField] public AudioClip[] IdleClips;
    [field: SerializeField] public AudioClip[] MovingClips;
    [field: SerializeField] public AudioClip[] AttackingClips;
    [field: SerializeField] public AudioClip[] DamagedClips;
    [field: SerializeField] public AudioClip[] DiedClips;

    [field: SerializeField] public float MinIdleClipRepeatingDelay;
    [field: SerializeField] public float MaxIdleClipRepeatingDelay;
    [field: SerializeField] public float MinMovingClipRepeatingDelay;
    [field: SerializeField] public float MaxMovingClipRepeatingDelay;

}
