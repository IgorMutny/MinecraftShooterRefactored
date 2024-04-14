using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

    public static void Register<T>(T service) where T : IService
    {
        if (_services.ContainsKey(typeof(T)) == false)
        {
            _services.Add(typeof(T), service);
        }
        else
        {
            throw new Exception("Unable to register service: service locator already contains it");
        }
    }

    public static void Unregister<T>() where T : IService
    {
        if (_services.ContainsKey(typeof(T)) == true)
        {
            _services.Remove(typeof(T));
        }
        else
        {
            throw new Exception("Unable to unregister service: service locator does not contain it");
        }
    }

    public static T Get<T>() where T : IService
    {
        if (_services.ContainsKey(typeof(T)) == true)
        {
            return (T)_services[typeof(T)];
        }
        else
        {
            throw new Exception("Unable to provide service: service locator does not contain it");
        }
    }

    public static void GetServicesAmount()
    {
        UnityEngine.Debug.Log("Registered services: " + _services.Count);
    }
}
