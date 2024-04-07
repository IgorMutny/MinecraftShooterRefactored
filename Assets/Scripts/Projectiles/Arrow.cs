using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private AudioClip _hitClip;

    protected override void HandleHit(RaycastHit hit)
    {
        {
            if (hit.collider.TryGetComponent(out Character character) == true)
            {
                character.Health.GetDamage(GetRandomDamage(), DamageType.Physical, Sender);
                Deactivate(hit);
                if (character.IsPlayer == true)
                {
                    ShouldBeDestroyed = true;
                }
            }

            if (hit.collider.TryGetComponent(out SolidBlock solidBlock) == true)
            {
                Deactivate(hit);
            }
        }
    }

    private void Deactivate(RaycastHit hit)
    {
        transform.position = hit.point;
        transform.parent = hit.collider.transform;
        IsActive = false;

        AudioSourceWrapper audioSource = new AudioSourceWrapper(transform, true);
        audioSource.SetClip(_hitClip);
        audioSource.Play();
    }
}

