using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Language : MonoBehaviour
{
    public string currentLanguage;
    public static Language Instance;

    [DllImport("__Internal")]
    private static extern string GetLang();
  
    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
#if !UNITY_EDITOR
            currentLanguage = GetLang();          
#endif
        }
        else
            Destroy(gameObject);
    }

}
