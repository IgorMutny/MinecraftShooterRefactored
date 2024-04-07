using UnityEngine;

public class PotionExplosion : Explosion
{
    [SerializeField] private ExplosionType _type;
    [SerializeField] private EffectInfo _effectInfo;
    [SerializeField] private float _radius;
    [SerializeField] private AudioClip _sound;

    private AudioSourceWrapper _audioSource;

    public override void Activate()
    {
        Destroy(gameObject, _lifetime);

        if (TryGetComponent(out ParticleSystem particleSystem) == true)
        {
            particleSystem.Play();
        }


        if (_sound != null)
        {
            _audioSource = new AudioSourceWrapper(transform, true);
            _audioSource.SetClip(_sound);
            _audioSource.Play();
        }

        AddEffects();
    }

    private void AddEffects()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radius, Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Character character) == true)
            {
                if (character != Sender)
                {
                    AddEffect(character);
                }
            }
        }
    }

    private void AddEffect(Character character)
    {
        if (_type == ExplosionType.Harming)
        {
            character.Health.GetDamage((int)_effectInfo.Value, DamageType.Magical, null);
        }

        if (_type == ExplosionType.Poisoning)
        {
            character.AppliedEffects.TryAddEffect<PoisoningEffect>(_effectInfo);
        }

        if (_type == ExplosionType.Slowness)
        {
            character.AppliedEffects.TryAddEffect<SlownessEffect>(_effectInfo);
        }
    }

    private enum ExplosionType
    {
        Harming,
        Poisoning,
        Slowness
    }
}
