using System;
using UnityEngine;

public class TimerSignal
{
    private float _counter;
    private Action _action;
    private float _multiplier;

    public event Action<TimerSignal> Ready;

    public TimerSignal(float counter, Action action, float multiplier)
    {
        _counter = counter;
        _action = action;
        _multiplier = multiplier;
    }

    public void ChangeMultiplier(float multiplier)
    { 
        _multiplier = multiplier;
    }

    public void OnTick()
    {
        if (_counter > 0)
        {
            _counter -= Time.fixedDeltaTime * _multiplier;

            if (_counter <= 0 )
            {
                _action();
                Ready?.Invoke(this);
            }
        }
    }
}
