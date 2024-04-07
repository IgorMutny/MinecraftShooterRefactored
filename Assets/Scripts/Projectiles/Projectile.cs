using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        AwakeExtended();

        Destroy(gameObject, _lifeTime);
    }

    protected virtual void AwakeExtended() { }

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

        if (hits.Length == 0)
        {
            return;
        }

        float[] distances = new float[hits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            distances[i] = Vector3.Distance(transform.position, hits[i].point);
        }

        Array.Sort(distances, hits);

        foreach (RaycastHit hit in hits)
        {
            if (Sender != null && hit.collider.gameObject == Sender.gameObject)
            {
                continue;
            }

            HandleHit(hit);

            if (IsActive == false)
            {
                return;
            }
        }
    }

    protected abstract void HandleHit(RaycastHit hit);

    protected int GetRandomDamage()
    {
        float damageMultiplier = 1;

        if (Sender != null)
        {
            damageMultiplier = Sender.AppliedEffects.DamageMultiplier;
        }

        int minDamage = (int)(_minDamage * damageMultiplier);
        int maxDamage = (int)(_maxDamage * damageMultiplier) + 1;

        return Random.Range(minDamage, maxDamage);
    }
}
