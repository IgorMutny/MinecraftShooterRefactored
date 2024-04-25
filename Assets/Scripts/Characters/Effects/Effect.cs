using System;
using UnityEngine;

public abstract class Effect
{
    protected float _duration;
    protected float _period;
    protected float _value;
    protected Character _character;
    protected AppliedEffects _appliedEffects;
    protected TimerWrapper _timer;
    protected TimerSignal _expiredSignal;
    protected bool _isActive;

    public EffectInfo Info { get; private set; }

    public event Action<Effect> Expired;

    public void Initialize(EffectInfo info)
    {
        Info = info;
        _duration = info.Duration;
        _period = info.Period;
        _value = info.Value;
        _timer = ServiceLocator.Get<TimerWrapper>();
        _expiredSignal = _timer.AddSignal(_duration, Expire);
        _isActive = true;

        InitializeExtended();
    }

    public void SetDuration(float duration)
    {
        _duration = duration;

        if (_expiredSignal != null)
        {
            _timer.RemoveSignal(_expiredSignal);
        }

        _timer.AddSignal(_duration, Expire);
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
        OnTickExtended();
    }

    protected virtual void InitializeExtended() { }

    protected virtual void OnTickExtended() { }

    private void Expire()
    {
        _isActive = false;
        Expired?.Invoke(this);
    }
}
