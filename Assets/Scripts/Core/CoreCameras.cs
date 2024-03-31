using UnityEngine;

public class CoreCameras : MonoBehaviour
{
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public Camera WeaponCamera { get; private set; }
    [field: SerializeField] public Camera UICamera { get; private set; }
}
