using UnityEngine;

public class Potion : Projectile
{
    [SerializeField] private PotionInfo[] _infos;
    [SerializeField] private MeshRenderer _overlay;

    private PotionInfo _info;

    protected override void AwakeExtended()
    {
        int rnd = Random.Range(0, _infos.Length);
        _info = _infos[rnd];
        _overlay.materials[0].color = _info.Color;
    }

    protected override void HandleHit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Character character) == true)
        {
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
        if (IsActive == true && _info.Explosion != null)
        {
            Explosion explosion =
                Instantiate(_info.Explosion, position, Quaternion.identity).GetComponent<Explosion>();
            explosion.SetSender(Sender);
            explosion.Activate();
        }

        ShouldBeDestroyed = true;
        IsActive = false;
    }
}

