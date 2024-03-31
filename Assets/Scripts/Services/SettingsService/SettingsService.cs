using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsService : IService
{
    private Dictionary<Type, ScriptableObject> _settings = new Dictionary<Type, ScriptableObject>();

    public void Register<T>(T settings) where T : ScriptableObject
    {
        if (_settings.ContainsKey(typeof(T)) == false)
        {
            _settings.Add(typeof(T), settings);
        }
        else
        {
            throw new Exception("Unable to register scriptable object: it is already registered");
        }
    }

    public T Get<T>() where T : ScriptableObject
    {
        if (_settings.ContainsKey(typeof(T)) == true)
        {
            return (T)_settings[typeof(T)];
        }
        else
        {
            throw new Exception("Unable to provide scriptable object: it isn't registered");
        }
    }
}
