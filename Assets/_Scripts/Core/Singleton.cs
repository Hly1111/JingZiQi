using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    Debug.LogError($"Can't find an instance of {typeof(T).Name}. Please ensure it is added to the scene.");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Debug.LogWarning($"Repeated instance of {typeof(T).Name} detected. Destroying the new instance.");
                Destroy(gameObject);
            }
        }
    }
}