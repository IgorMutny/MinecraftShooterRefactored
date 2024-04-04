using System;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    [SerializeField] private LootBody _body;

    private float _rotationTime = 1;
    private float _triggerRadius;

    private Vector3 _gravity = new Vector3(0, -9.8f, 0);
    private float _lifeTime;
    private bool _isAlive;

    public LootInfo Info { get; private set; }

    public event Action<Loot> Picked;
    public event Action<Loot> Died;

    public void Initialize(LootInfo info, GameObject beam)
    {
        Info = info;

        _body.SetAnimation(_rotationTime);

        Instantiate(beam, transform);

        _triggerRadius = GetComponent<SphereCollider>().radius;
        _lifeTime = info.LifeTime;
        _isAlive = true;
    }

    private void FixedUpdate()
    {
        SimulateGravity();
        DecreaseLifeTime();
    }

    private void SimulateGravity()
    {
        RaycastHit[] hits =
            Physics.RaycastAll(transform.position, Vector3.down, _triggerRadius);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out SolidBlock block) == true)
            {
                return;
            }
        }

        transform.Translate(_gravity * Time.fixedDeltaTime);
    }

    private void DecreaseLifeTime()
    {
        if (_isAlive == true)
        {
            _lifeTime -= Time.fixedDeltaTime;

            if (_lifeTime <= 0)
            {
                _isAlive = false;
                Died?.Invoke(this);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Character character) == true)
        {
            if (character.IsPlayer == true)
            {
                Apply(character);
                Picked?.Invoke(this);
            }
        }
    }

    protected abstract void Apply(Character character);
}
