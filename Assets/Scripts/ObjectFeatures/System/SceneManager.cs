using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public AssetReference SampleSceneAddress;
    private AsyncOperationHandle<SceneInstance> sampleLoadHandle;
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
        sampleLoadHandle = Addressables.LoadSceneAsync(SampleSceneAddress, LoadSceneMode.Additive);
    }
}
