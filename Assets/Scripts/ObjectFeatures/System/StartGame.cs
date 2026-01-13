using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public AssetReference SystemSceneAddress;
    private AsyncOperationHandle<SceneInstance> systemLoadHandle;
    public event Action StartGameEvent;
    private void OnEnable()
    {
        StartGameEvent += OnStartGame;
    }
    private void OnDisable()
    {
        StartGameEvent -= OnStartGame;
    }
    private void Start()
    {
        StartGameEvent?.Invoke();
    }

    public void OnStartGame()
    {
        systemLoadHandle = Addressables.LoadSceneAsync(SystemSceneAddress, LoadSceneMode.Additive);
    }
}
