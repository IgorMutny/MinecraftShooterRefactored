using DG.Tweening;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _lifetime;
    [SerializeField] private AudioClip _sound;

    private float _flashTime = 0.1f;
    private Character _sender;
    private AudioSourceWrapper _audioSource;

    public void SetSender(Character sender)
    {
        _sender = sender;
    }

    public void Activate()
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
            _audioSource.Play();
        }

        if (_maxDamage > 0)
        {
            DoDamage();
        }
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
                if (_sender != null)
                {
                    damageMultiplier = _sender.AppliedEffects.DamageMultiplier;
                }

                int damage = (int)(_maxDamage * percentOfDamage * damageMultiplier);
                character.Health.GetDamage(damage, DamageType.Physical, _sender);
            }
        }
    }

    private void OnDestroy()
    {
        _audioSource = null;

        DOTween.Kill(this);
    }
}

