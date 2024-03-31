using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;
    [SerializeField] private int _minDamage;
    [SerializeField] private int _maxDamage;

    [field: SerializeField] protected GameObject ExplosionSample { get; private set; }
    [field: SerializeField] protected bool IsPenetrating { get; private set; }

    protected Character Sender { get; private set; }

    protected bool IsActive;
    protected bool ShouldBeDestroyed;

    public void SetSender(Character sender)
    {
        Sender = sender;
    }

    protected virtual void Awake()
    {
        IsActive = true;
        ShouldBeDestroyed = false;

        Destroy(gameObject, _lifeTime);
    }

    private void FixedUpdate()
    {
        if (IsActive == true)
        {
            TryHandleHits();

            if (ShouldBeDestroyed == false)
            {
                transform.Translate((transform.forward * _speed + Vector3.down * _gravity) * Time.fixedDeltaTime, Space.World);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void TryHandleHits()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, _speed * Time.fixedDeltaTime);

        foreach (RaycastHit hit in hits)
        {
            if ((Sender != null && hit.collider.gameObject != Sender.gameObject) || Sender == null)
            {
                HandleHit(hit);
            }
        }
    }

    protected abstract void HandleHit(RaycastHit hit);

    protected int GetRandomDamage()
    {
        float damageMultiplier = 1;

        if (Sender != null)
        {
            damageMultiplier = Sender.DamageMultiplier;
        }

        int minDamage = (int)(_minDamage * damageMultiplier);
        int maxDamage = (int)(_maxDamage * damageMultiplier) + 1;

        return Random.Range(minDamage, maxDamage);
    }
}
