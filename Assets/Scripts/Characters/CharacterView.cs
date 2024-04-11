using System;
using UnityEngine;
using CharacterViewElements;

public class CharacterView
{
    private Character _character;
    private AnimatorController _animator;
    private AudioController _audio;
    private BodyPartsController _bodyParts;
    private EffectsController _effects;

    public event Action<Character> Dissolved;

    public CharacterView(Character character, CharacterAudioInfo audioInfo)
    {
        _character = character;

        _animator = new AnimatorController(_character.GetComponent<Animator>());
        _audio = new AudioController(_character.transform, audioInfo);
        _bodyParts = new BodyPartsController(_character.transform);
        _effects = new EffectsController(_character.transform);
    }

    public void StartMovement()
    {
        _audio.StartMovement();
        _animator.StartMovement();
    }

    public void StopMovement()
    {
        _audio.StopMovement();
        _animator.StopMovement();
    }

    public void ChangeSpeed(float multiplier)
    {
        _audio.SetSpeed(multiplier);
        _animator.ChangeSpeed(multiplier);
    }

    public void Attack()
    {
        _animator.Attack();
        _audio.Attack();
    }

    public void Explode(float duration)
    {
        _bodyParts.Explode(duration);
        _audio.Attack();
    }
    public void TryAddEffect(EffectInfo info)
    {
        _effects.TryAddEffect(info);
    }

    public void TryRemoveEffect(EffectInfo info)
    {
        _effects.TryRemoveEffect(info);
    }

    public void RemoveAllEffects()
    {
        _effects.RemoveAllEffects();
    }

    public void OnDamaged(int damage)
    {
        _audio.OnDamaged();
        _bodyParts.OnDamaged();
    }

    public void OnDied()
    {
        _audio.OnDied();
        _animator.OnDied();
        _bodyParts.OnDied();
        _bodyParts.Dissolved += OnDissolved;
    }

    public void Dissolve()
    {
        _bodyParts.Dissolve();
    }

    private void OnDissolved()
    {
        Dissolved?.Invoke(_character);
    }

    public void Destroy()
    {
        _audio.OnDestroy();
        _bodyParts.OnDestroy();
        _bodyParts.Dissolved -= OnDissolved;

        _animator = null;
        _audio = null;
        _bodyParts = null;
        _effects = null;
    }
}

