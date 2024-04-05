using System;
using System.Collections.Generic;
using UnityEngine;

public class AppliedEffects
{
    private Character _character;
    private List<Effect> _effects = new List<Effect>();
    private List<Effect> _effectsToRemove = new List<Effect>();

    private float _speedMultiplier;
    private float _damageMultiplier;

    public float SpeedMultiplier { get; private set; }
    public float DamageMultiplier { get; private set; }

    public event Action<float> SpeedMultiplierChanged;
    public event Action<float> DamageMultiplierChanged;

    public AppliedEffects(Character character)
    {
        _character = character;
    }

    public void OnTick()
    {
        _speedMultiplier = 1f;
        _damageMultiplier = 1f;

        foreach (Effect effect in _effects)
        {
            effect.OnTick();
        }

        RemoveExpiredEffects();
        SetSpeedMultiplier();
        SetDamageMultiplier();
    }

    private void SetSpeedMultiplier()
    {
        if (_speedMultiplier != SpeedMultiplier)
        {
            SpeedMultiplierChanged?.Invoke(_speedMultiplier);
        }

        SpeedMultiplier = _speedMultiplier;
    }

    private void SetDamageMultiplier()
    {
        if (_damageMultiplier != DamageMultiplier)
        {
            DamageMultiplierChanged?.Invoke(_damageMultiplier);
        }

        DamageMultiplier = _damageMultiplier;
    }

    private void RemoveExpiredEffects()
    {
        foreach (Effect effect in _effectsToRemove)
        {
            _effects.Remove(effect);
        }

        _effectsToRemove.Clear();
    }

    public void TryAddEffect<T>(float duration, float period, float value) where T : Effect, new()
    {
        foreach (Effect effect in _effects)
        {
            if (effect.GetType() == typeof(T))
            {
                effect.SetDuration(duration);
                return;
            }
        }

        AddEffect<T>(duration, period, value);
    }

    private void AddEffect<T>(float duration, float period, float value) where T : Effect, new()
    {
        Effect effect = new T();
        effect.SetCharacter(_character);
        effect.SetAppliedEffects(this);
        effect.SetDuration(duration);
        effect.SetPeriod(period);
        effect.SetValue(value);
        effect.Expired += OnExpired;
        _effects.Add(effect);
    }

    public void MultiplySpeedMultiplier(float value)
    {
        _speedMultiplier *= value;
    }

    public void MultiplyDamageMultiplier(float value)
    {
        _damageMultiplier *= value;
    }

    public void OnExpired(Effect effect)
    {
        effect.Expired -= OnExpired;
        _effectsToRemove.Add(effect);
    }

    public void RemoveAllEffects()
    {
        _effects.Clear();
    }

    public void OnDied()
    {
        foreach(Effect effect in _effects)
        {
            effect.Expired -= OnExpired;
        }

        _effects.Clear();
    }
}
