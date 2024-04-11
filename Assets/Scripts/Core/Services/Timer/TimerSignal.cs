using System;
using UnityEngine;

public class TimerSignal
{
    private float _counter;
    private Action _action;
    private string _tag;

    public event Action<TimerSignal> Ready;

    public TimerSignal(float counter, Action action, string tag = null)
    {
        _counter = counter;
        _action = action;
        _tag = tag;
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

    ~TimerSignal()
    { 
        if (_tag != null)
        {
            Debug.Log($"TimerSignal {_tag} destroyed in memory");
        }
    }
}
