using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private AudioClip _hitClip;

    private Character _parent;

    protected override void HandleHit(RaycastHit hit)
    {
        {
            if (hit.collider.TryGetComponent(out Character character) == true)
            {
                character.Health.GetDamage(GetRandomDamage(), DamageType.Physical, Sender);
                Deactivate(hit, character);
                if (character.IsPlayer == true)
                {
                    ShouldBeDestroyed = true;
                }
            }

            if (hit.collider.TryGetComponent(out SolidBlock solidBlock) == true)
            {
                Deactivate(hit, null);
            }
        }
    }

    private void Deactivate(RaycastHit hit, Character character)
    {
        transform.position = hit.point;
        transform.parent = hit.collider.transform;
        IsActive = false;

        if (character != null)
        {
            _parent = character;
            _parent.Health.Died += OnParentDied;
        }

        AudioSourceWrapper audioSource = new AudioSourceWrapper(transform, true);
        audioSource.SetClip(_hitClip);
        audioSource.Play();
    }

    private void OnParentDied(Character character)
    {
        ShouldBeDestroyed = true;
    }

    private void OnDestroy()
    {
        if (_parent != null)
        {
            _parent.Health.Died -= OnParentDied;
        }
    }
}

