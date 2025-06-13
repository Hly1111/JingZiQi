using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceProvider : Singleton<ServiceProvider>
{
    private Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
    
    public void RegisterService<T>(T service) where T : IService
    {
        Type key = typeof(T);
        if (!key.IsInterface)
        {
            Debug.LogError($"[ServiceProvider] [Register] 非接口的注册键: {key.Name}");
            return;
        }
        
        if (service == null)
        {
            Debug.LogError($"[ServiceProvider] [Register] 空服务: {key.Name}");
            return;
        }
        
        if (Exists<T>())
        {
            Debug.LogWarning($"[ServiceProvider] [Register] 覆盖注册: {key.Name}");
        }
        _services[key] = service;
    }
    
    public T GetService<T>() where T : IService
    {
        Type key = typeof(T);
        if (!key.IsInterface)
        {
            Debug.LogError($"[ServiceProvider] [GetService] 非接口: {key.Name}");
            return default;
        }
        
        if (!Exists<T>())
        {
            Debug.LogError($"[ServiceProvider] [GetService] 不存在: {key.Name}");
            return default;
        }

        IService service = _services[key];
        if (service == null)
        {
            Debug.LogError($"[ServiceProvider] [GetService] 空服务: {key.Name}");
            return default;
        }
        
        if (!key.IsAssignableFrom(service.GetType()))
        {
            Debug.LogError($"[ServiceProvider] [GetService] 类型不匹配: {key.Name}");
            return default;
        }
        return (T)service;
    }
    
    public void UnregisterService<T>() where T : IService
    {
        Type key = typeof(T);
        if (!Exists<T>())
        {
            Debug.LogWarning($"[ServiceProvider] [UnregisterService] 不存在: {key.Name}");
            return;
        }
        _services.Remove(key);
    }
    
    public void UnregisterAllServices()
    {
        _services.Clear();
    }

    private bool Exists<T>() where T : IService
    {
        return _services.ContainsKey(typeof(T));
    }
}