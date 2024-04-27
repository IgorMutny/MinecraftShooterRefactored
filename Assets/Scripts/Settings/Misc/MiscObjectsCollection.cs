using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Objects Collection")]
public class MiscObjectsCollection : ScriptableObject
{
    [field: SerializeField] public GameObject MetaUI { get; private set; }
    [field: SerializeField] public GameObject CoreUI { get; private set; }
    [field: SerializeField] public GameObject UIAudioSource { get; private set; }
    [field: SerializeField] public GameObject Timer { get; private set; }
    [field: SerializeField] public GameObject PCInput { get; private set; }
    [field: SerializeField] public GameObject MobileInput { get; private set; }
    [field: SerializeField] public GameObject CoreCameras { get; private set; }
    [field: SerializeField] public AudioClip WaveBegins { get; private set; }
    [field: SerializeField] public AudioClip WaveEnds { get; private set; }
}
