using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject _prefab;
    private readonly int _poolSize;
    private readonly Queue<GameObject> _pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int poolSize)
    {
        _poolSize = poolSize;
        _prefab = prefab;
        
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Object.Instantiate(_prefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    
    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            GameObject obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.Log(_prefab.name + " overflow, creating a new instance.");
            GameObject obj = Object.Instantiate(_prefab);
            obj.SetActive(true);
            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        if (_pool.Count < _poolSize)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(null);
            _pool.Enqueue(obj);
        }
        else
        {
            Object.Destroy(obj.gameObject);
        }
    }
}