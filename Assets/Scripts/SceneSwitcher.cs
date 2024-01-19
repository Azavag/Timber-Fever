using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CheckSdkReady();

    [SerializeField] AdvManager advManager;

    private void Start()
    {
#if UNITY_EDITOR
        SwitchScene();
#endif
    }
    private void Update()
    {
#if !UNITY_EDITOR
        CheckSdkReady();
#endif
    }
    public void SwitchScene()
    {
        advManager.ShowAdv();
        SceneManager.LoadScene(1);
    }
}


