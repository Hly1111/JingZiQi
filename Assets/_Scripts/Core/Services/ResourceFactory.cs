using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public interface IResourceFactory : IService
{
    void LoadPrefabAsync();
    T GetResource<T>(string name) where T : Object;
}

public class ResourceFactory : IResourceFactory
{
    private readonly Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    
    public void LoadPrefabAsync()
    {
        string tokenPath = Path.Combine(Application.streamingAssetsPath, "token");
        var bundle = AssetBundle.LoadFromFile(tokenPath);
        if (bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle from " + tokenPath);
            return;
        }

        foreach (var asset in bundle.LoadAllAssets())
        {
            _resources[asset.name] = asset;
        }
    }
    
    public T GetResource<T>(string name) where T : Object
    {
        if (_resources.TryGetValue(name, out var resource))
        {
            return resource as T;
        }
        Debug.LogError("Resource not found: " + name);
        return null;
    }
}