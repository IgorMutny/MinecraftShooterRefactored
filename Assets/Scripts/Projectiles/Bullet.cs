using UnityEngine;

public class Bullet : Projectile
{
    protected override void HandleHit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Character character) == true)
        { 
            character.Health.GetDamage(GetRandomDamage(), DamageType.Physical, Sender);

            if (IsPenetrating == false)
            {
                Explode(hit.point);
            }
        }
        
        if (hit.collider.TryGetComponent(out SolidBlock solidBlock) == true)
        {
            Explode(hit.point);
        }
    }

    private void Explode(Vector3 position)
    {
        if (IsActive == true && ExplosionSample != null)
        {
            Explosion explosion =
                Instantiate(ExplosionSample, position, Quaternion.identity).GetComponent<Explosion>();
            explosion.SetSender(Sender);
            explosion.Activate();
        }

        ShouldBeDestroyed = true;
        IsActive = false;
    }
}
