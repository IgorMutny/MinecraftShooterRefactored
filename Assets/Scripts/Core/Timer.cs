using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action Tick;

    private void FixedUpdate()
    {
        Tick?.Invoke();
    }
}
