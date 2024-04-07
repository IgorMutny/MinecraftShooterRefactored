using UnityEngine;

public class EmptyExplosion : Explosion
{
    public override void Activate()
    {
        Destroy(gameObject, _lifetime);

        if (TryGetComponent(out ParticleSystem particleSystem) == true)
        {
            particleSystem.Play();
        }
    }
}