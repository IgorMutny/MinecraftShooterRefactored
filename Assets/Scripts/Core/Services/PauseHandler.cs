using System;
using UnityEngine;

public class PauseHandler : IService
{
    private bool _isPaused;

    public event Action<bool> PauseSwitched;

    public PauseHandler()
    {
        Time.timeScale = 1;
        _isPaused = false;
    }

    public void Destroy()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = false;
    }

    public void SwitchPause()
    {
        if (_isPaused == false)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
        _isPaused = true;
        PauseSwitched?.Invoke(_isPaused);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
        _isPaused = false;
        PauseSwitched?.Invoke(_isPaused);
    }
}

