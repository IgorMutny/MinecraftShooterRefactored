using System;
using UnityEngine;

public abstract class Effect
{
    protected float _duration;
    protected float _period;
    protected float _periodCounter;
    protected float _value;
    protected Character _character;
    protected AppliedEffects _appliedEffects;

    public event Action<Effect> Expired;

    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void SetPeriod(float period)
    {
        _period = period;
        _periodCounter = 0;
    }

    public void SetValue(float value)
    {
        _value = value;
    }

    public void SetCharacter(Character character)
    {
        _character = character;
    }

    public void SetAppliedEffects(AppliedEffects appliedEffects)
    {
        _appliedEffects = appliedEffects;
    }

    public void OnTick()
    {
        if (_duration <= 0)
        {
            Expired?.Invoke(this);
        }

        _duration -= Time.fixedDeltaTime;

        OnTickExtended();
    }

    protected abstract void OnTickExtended();
}
