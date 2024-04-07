using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerWrapper : IService
{
    private Timer _timer;
    private List<TimerSignal> _signals = new List<TimerSignal>();
    private List<TimerSignal> _signalsToRemove = new List<TimerSignal>();

    public event Action Tick;

    public TimerWrapper()
    {
        GameObject timerSample =
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().Timer;
        _timer = GameObject.Instantiate(timerSample).GetComponent<Timer>();
        _timer.Tick += OnTick;
    }

    private void OnTick()
    {
        Tick?.Invoke();

        foreach (TimerSignal signal in _signals)
        {
            signal.OnTick();
        }

        foreach(TimerSignal signal in _signalsToRemove)
        {
            _signals.Remove(signal);
        }

        _signalsToRemove.Clear();
    }

    public void Destroy()
    {
        _timer.Tick -= OnTick;
        GameObject.Destroy(_timer.gameObject);
    }

    public TimerSignal SetSignal(float seconds, Action action)
    {
        TimerSignal signal = new TimerSignal(seconds, action);
        _signals.Add(signal);
        signal.Ready += RemoveSignal;
        return signal;
    }

    public void RemoveSignal(TimerSignal signal)
    {
        if (signal != null)
        {
            signal.Ready -= RemoveSignal;
            _signalsToRemove.Add(signal);
        }
    }
}
