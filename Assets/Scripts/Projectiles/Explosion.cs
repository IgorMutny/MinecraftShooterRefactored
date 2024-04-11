using UnityEngine;

public abstract class Explosion : MonoBehaviour
{
    [SerializeField] protected float _lifetime;

    protected Character Sender { get; private set; }

    public void SetSender(Character sender)
    {
        Sender = sender;
    }

    public abstract void Activate();
}

