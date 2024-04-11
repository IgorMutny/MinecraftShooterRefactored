using DG.Tweening;
using UnityEngine;

public class NormalExplosion : Explosion
{
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _radius;
    [SerializeField] private AudioClip _sound;

    private AudioSourceWrapper _audioSource;
    private float _flashTime = 0.1f;
    private float _audioVolume = 2;

    public override void Activate()
    {
        Destroy(gameObject, _lifetime);

        if (TryGetComponent(out ParticleSystem particleSystem) == true)
        {
            particleSystem.Play();
        }

        if (TryGetComponent(out Light light) == true)
        {
            light.DOIntensity(0, _flashTime);
        }

        if (_sound != null)
        {
            _audioSource = new AudioSourceWrapper(transform, true);
            _audioSource.SetClip(_sound);
            _audioSource.SetVolume(_audioVolume);
            _audioSource.Play();
        }

        DoDamage();
    }

    private void DoDamage()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radius, Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Character character) == true)
            {
                float distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                float percentOfDamage = (_radius - distance) / _radius;

                float damageMultiplier = 1;
                if (Sender != null)
                {
                    damageMultiplier = Sender.AppliedEffects.DamageMultiplier;
                }

                int damage = (int)(_maxDamage * percentOfDamage * damageMultiplier);
                character.Health.GetDamage(damage, DamageType.Physical, Sender);
            }
        }
    }

    private void OnDestroy()
    {
        _audioSource = null;

        DOTween.Kill(this);
    }
}
