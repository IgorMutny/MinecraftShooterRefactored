using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _lifetime;

    private Character _sender;

    private void Awake()
    {
        Destroy(gameObject, _lifetime);
    }

    public void SetSender(Character sender)
    {
        _sender = sender;
    }
}
