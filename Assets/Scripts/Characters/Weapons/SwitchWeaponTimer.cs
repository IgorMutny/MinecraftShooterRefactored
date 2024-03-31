using System;
using UnityEngine;

public class SwitchWeaponTimer
{
    private float _timer;
    private float _timerCurrent;
    private int _index;

    bool _isWeaponTaken = false;
    bool _isWeaponReady = false;

    public event Action<int> WeaponTaken;
    public event Action WeaponReady;

    public SwitchWeaponTimer(float timer, int index)
    {
        _timer = timer;
        _timerCurrent = timer;
        _index = index;
    }

    public void OnTick()
    {
        _timerCurrent -= Time.fixedDeltaTime;

        if (_timerCurrent <= _timer / 2 && _isWeaponTaken == false)
        {
            WeaponTaken?.Invoke(_index);
            _isWeaponTaken = true;
        }

        if (_timerCurrent <= 0 && _isWeaponReady == false)
        {
            WeaponReady?.Invoke();
            _isWeaponReady = true;
        }
    }
}
