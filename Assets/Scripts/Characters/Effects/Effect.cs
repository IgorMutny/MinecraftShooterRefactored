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

    public EffectInfo Info { get; private set; }

    public event Action<Effect> Expired;

    public void SetInfo(EffectInfo info)
    {
        Info = info;
        _duration = info.Duration;
        _period = info.Period;
        _periodCounter = 0;
        _value = info.Value;
    }

    public void SetDuration(float duration)
    { 
        _duration = duration;
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
