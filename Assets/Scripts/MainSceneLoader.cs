using System.Collections;
using UnityEngine;

public class MainSceneLoader : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    [SerializeField] ProgressLoadedChecker progressLoadedChecker;
    bool isSceneLoaded;
    private void Awake()
    {
        foreach (GameObject obj in objects) 
        { 
            obj.SetActive(false);
        }
       
    }

    private void LateUpdate()
    {
        if (progressLoadedChecker.isProgressLoaded && !isSceneLoaded)
            LoadGame();
    }
    public void LoadGame()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
        isSceneLoaded = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        Silence(!focus);
        if (!focus)
            PauseGame();
        else UnpauseGame();
    }
    private void OnApplicationPause(bool pause)
    {
        Silence(pause);
        if (pause)
            PauseGame();
        else UnpauseGame();
    }
    void Silence(bool silence)
    {
        AudioListener.pause = silence;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

}
