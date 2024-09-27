using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

    public static ServiceLocator Instance { get; private set; }

    public static void Initialize() => 
        Instance = new ServiceLocator();

    public T Get<T>() where T : IService
    {
        var name = typeof(T).Name;

        if (!_services.ContainsKey(name))
            throw new InvalidOperationException($"{name} is not registered");

        return (T)_services[name];
    }

    public void Register<T>(T service) where T : IService
    {
        var name = typeof(T).Name;

        if (_services.ContainsKey(name))
            throw new InvalidOperationException($"{name} already registered");

        _services.Add(name, service);
    }

    public void Unregister<T>() where T : IService
    {
        var name = typeof(T).Name;

        if (!_services.ContainsKey(name))
            throw new InvalidOperationException($"{name} is not registered");

        _services.Remove(name);
    }
}