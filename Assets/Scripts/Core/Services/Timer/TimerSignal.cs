using System;
using UnityEngine;

public class TimerSignal
{
    private float _counter;
    private Action _action;

    public event Action<TimerSignal> Ready;

    public TimerSignal(float counter, Action action)
    {
        _counter = counter;
        _action = action;
    }

    public void OnTick()
    {
        if (_counter > 0)
        {
            _counter -= Time.fixedDeltaTime;

            if (_counter <= 0 )
            {
                _action();
                Ready?.Invoke(this);
            }
        }
    }
}
