using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour
{
    private void Awake()
    {
        IResourceFactory rf= new ResourceFactory();
        ServiceProvider.Instance.RegisterService<IResourceFactory>(rf);
        rf.LoadPrefabAsync();
    }

    private void Start()
    {
        SceneManager.LoadScene("S1");
    }
}