using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public float moneyCount = 0;
    public int sizeLevel = 0;
    public float expandPrice = 100f;
    public float incomePrice = 30f;
    public float incomeMulti = 1.0f;
    public float treePrice = 15f;

    public bool[] areLevel1RootsActivate = new bool[8] 
    { true, false, false, false, false, false, false, false };
    public bool[] areLevel2RootsActivate = new bool[8];
    public bool[] areLevel3RootsActivate = new bool[8];
    public bool[] areLevel4RootsActivate = new bool[8];
    public bool[] areLevel5RootsActivate = new bool[8];
}


public class Progress : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public static Progress Instance;
    [SerializeField] YandexSDK yandexSDK;
   
    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            yandexSDK.Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       
    }

}



