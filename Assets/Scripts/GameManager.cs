using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject cameraMain;
    public GameObject cameraPosLv2;
    public GameObject cameraPosLv3;
    public GameObject cameraPosLv4;
    public GameObject cameraPosLv5;
    public float areaLv1 = 1f;
    public float areaLv2 = 1.5f;
    public float areaLv3 = 2f;
    public float areaLv4 = 2.5f;
    public float areaLv5 = 3f;
    public int sawLengthLv1 = 20;
    public int sawLengthLv2 = 53;
    public int sawLengthLv3 = 88;
    public int sawLengthLv4 = 124;
    public int sawLengthLv5 = 161;
    
    public GameObject groundTop;
    public TextMeshProUGUI moneyText;
    public Button mergeButton;
    public Button addRootButton;
    public Button sizeUpButton;
    public Button incomeButton;
    public TextMeshProUGUI treePriceText;
    public TextMeshProUGUI expandPriceText;
    public TextMeshProUGUI incomePriceText;
    public TextMeshProUGUI incomeMultiplierText;
    public GameObject maxLevelobj;
    public List<GameObject> rootsLv1;
    public List<GameObject> rootsLv2;
    public List<GameObject> rootsLv3;
    public List<GameObject> rootsLv4;
    public List<GameObject> rootsLv5;
    public GameObject sawMain;
    public int rotationSpeed = 40;
    //Aðaç büyümesini ve testere hýzýný kontrol eder
    public int speedMultiply = 2;
    public float money;
    public float expandPrice = 100f;
    public float treePrice = 15f;
    public float incomePrice = 30f;
    public float expandMultiply = 2f;
    public float incomeMultiply = 1f;
    public float incomePriceMultiplier = 1.07f;
    public float treePriceMultiplier = 1.05f;
    public bool canMerge = false;
    public bool moveCam = false;

    private bool level2isActive = false;
    private bool level3isActive = false;
    private bool level4isActive = false;
    private bool level5isActive = false;
    public float areaLength;
    public Vector3 targetCamPosition;
    private Vector3 offSet = new Vector3(0.05f, 0.05f, -0.05f);


    public List<bool> isActiveList = new List<bool>();
    private List<List<GameObject>> rootsList = new List<List<GameObject>>();

    [SerializeField] private ArrowAnimation arrowAnimation;
    private AdvManager advManager;


    private void Awake()
    {
        advManager = FindObjectOfType<AdvManager>();
    }
    private void Start()
    {
        //advManager.ShowAdv();
        //Merge kontrol için aktiflikleri ve rootlarý listlere ekliyorum
        isActiveList.Add(level2isActive);
        isActiveList.Add(level3isActive);
        isActiveList.Add(level4isActive);
        isActiveList.Add(level5isActive);

        rootsList.Add(rootsLv2);
        rootsList.Add(rootsLv3);
        rootsList.Add(rootsLv4);
        rootsList.Add(rootsLv5);
      
        StartSetup();
    }

    void StartSetup()
    {
        expandPrice = Progress.Instance.playerInfo.expandPrice;
        expandPriceText.text = expandPrice.ToString("0");

        treePrice = Progress.Instance.playerInfo.treePrice;
        treePriceText.text = treePrice.ToString("0");

        incomePrice = Progress.Instance.playerInfo.incomePrice;
        incomePriceText.text = incomePrice.ToString("0");

        incomeMultiply = Progress.Instance.playerInfo.incomeMulti;
        incomeMultiplierText.text = "X" + incomeMultiply.ToString("f1");

        money = Progress.Instance.playerInfo.moneyCount;
        moneyText.text = money.ToString("0");

        for(int i = 0; i <= Progress.Instance.playerInfo.sizeLevel; i++)
        {
            OpenNewLevel(i);
        }

        for (int counter = 0; counter < Progress.Instance.playerInfo.areLevel1RootsActivate.Length; counter++)
        {
            if (!Progress.Instance.playerInfo.areLevel1RootsActivate[counter])
                break;
            else rootsLv1[counter].GetComponent<root>().ActivateRoot();
        }
        for (int counter = 0; counter < Progress.Instance.playerInfo.areLevel2RootsActivate.Length; counter++)
        {
            if (!Progress.Instance.playerInfo.areLevel2RootsActivate[counter])
                break;
            else rootsLv2[counter].GetComponent<root>().ActivateRoot();
        }
        for (int counter = 0; counter < Progress.Instance.playerInfo.areLevel3RootsActivate.Length; counter++)
        {
            if (!Progress.Instance.playerInfo.areLevel3RootsActivate[counter])
                break;
            else rootsLv3[counter].GetComponent<root>().ActivateRoot();
        }
        for (int counter = 0; counter < Progress.Instance.playerInfo.areLevel4RootsActivate.Length; counter++)
        {
            if (!Progress.Instance.playerInfo.areLevel4RootsActivate[counter])
                break;
            else rootsLv4[counter].GetComponent<root>().ActivateRoot();
        }
        for (int counter = 0; counter < Progress.Instance.playerInfo.areLevel5RootsActivate.Length; counter++)
        {
            if (!Progress.Instance.playerInfo.areLevel5RootsActivate[counter])
                break;
            else rootsLv5[counter].GetComponent<root>().ActivateRoot();
        }

        CheckRoots();
    }
    
    public void addNewRoot(List<GameObject> _root)
    {
        for (int i = 0; i < _root.Count; i++)
        {
            if (money >= treePrice)
            {
                if (!_root[i].GetComponent<root>().isRootActive)
                {
                    money -= treePrice;
                    moneyText.text = money.ToString("f0");
                    _root[i].GetComponent<root>().ActivateRoot();
                    break;
                }
            }
        }
        
    }

    //When the button is clicked, root is added to lv1
    public void rootButtonClick()
    {
        addNewRoot(rootsLv1);
        SaveActivateRootStates(Progress.Instance.playerInfo.areLevel1RootsActivate,
            rootsLv1);
        treePrice = treePrice * treePriceMultiplier;
        treePriceText.text = treePrice.ToString("f0");
        Progress.Instance.playerInfo.moneyCount = money;
        Progress.Instance.playerInfo.treePrice = treePrice;
        YandexSDK.Save();
        CheckRoots();
    }
    void SaveActivateRootStates(bool[] statesArray, List<GameObject> rootsObjects)
    {
        for(int i = 0;i < rootsObjects.Count;i++) 
        { 
            if(rootsObjects[i].GetComponent<root>().isRootActive)
                statesArray[i] = true;
            else statesArray[i] = false;
        }
        YandexSDK.Save();
    }

    public bool IsRootFull(List<GameObject> _roots)
    {
        if (_roots[_roots.Count-1].GetComponent<root>().isRootActive)
            return true;
        else return false;
       
    }
    public void OpenNewLevel(int _level)
    {
        //Gelen level deðiþkenine göre levelleri açýyoruz
        if(_level == 2)
        {
            //boruyu uzatýyoruz
            sawMain.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, sawLengthLv2);
            //chain ana objesinin içerisindeki gerekli child olan testereyi aktif ediyoruz
            sawMain.transform.GetChild(1).gameObject.SetActive(true);
            //Cameranýn hareket etmesi için true yapýyoruz.
            moveCam = true;
            //alanýn açýlmasý için deðeri ayarlýyoruz
            areaLength = areaLv2;
            //target positionu set ediyoruz
            targetCamPosition = cameraPosLv2.transform.position;
            ExpandGroundTop(1f);
            ExpandCameraPosition(1f);
            //level2 nin aktif olduðunu belirtiyoruz
            isActiveList[0] = true;
            //Level2 den root açýyoruz.
        }
        else if (_level == 3)
        {
            sawMain.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, sawLengthLv3);
            sawMain.transform.GetChild(2).gameObject.SetActive(true);
            moveCam = true;
            areaLength = areaLv3;
            targetCamPosition = cameraPosLv3.transform.position;
            ExpandGroundTop(1f);
            ExpandCameraPosition(1f);
            isActiveList[1] = true;
        }
        else if (_level == 4)
        {
            sawMain.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, sawLengthLv4);
            sawMain.transform.GetChild(3).gameObject.SetActive(true);
            moveCam = true;
            areaLength = areaLv4;
            targetCamPosition = cameraPosLv4.transform.position;
            ExpandGroundTop(1f);
            ExpandCameraPosition(1f);
            isActiveList[2] = true;
        }
        else if (_level == 5)
        {
            sawMain.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, sawLengthLv5);
            sawMain.transform.GetChild(4).gameObject.SetActive(true);
            moveCam = true;
            areaLength = areaLv5;
            targetCamPosition = cameraPosLv5.transform.position;
            ExpandGroundTop(1f);
            ExpandCameraPosition(1f);
            isActiveList[3] = true;
        }
    }
    public void CloseAllRoot (List<GameObject> _root)
    {
        foreach (GameObject child in _root) 
        { 
            child.GetComponent<root>().CloseRoot();
        }
    }
    public void IncreaseIncome()
    {
        money -= incomePrice;
        moneyText.text = money.ToString("f0");
        incomePrice *= incomePriceMultiplier;
        incomeMultiply += 0.1f;
        incomeMultiplierText.text = "X" + incomeMultiply.ToString("f1");
        incomePriceText.text = incomePrice.ToString("f0");
        Progress.Instance.playerInfo.moneyCount = money;
        Progress.Instance.playerInfo.incomePrice = incomePrice;
        Progress.Instance.playerInfo.incomeMulti = incomeMultiply;
        YandexSDK.Save();

    }

    //Merge yapma
    public void Merge()
    {
        if (canMerge)
        {
            canMerge = false;

            CloseAllRoot(rootsLv1);
            SaveActivateRootStates(Progress.Instance.playerInfo.areLevel1RootsActivate,
                                            rootsLv1);        
            mergeButton.GetComponent<Button>().interactable = false;
            mergeButton.gameObject.SetActive(false);
            //level2 açýksa ve dolu deðilse
            if (isActiveList[0] && !IsRootFull(rootsLv2))
            {
                addNewRoot(rootsLv2);
                SaveActivateRootStates(Progress.Instance.playerInfo.areLevel2RootsActivate,
                                        rootsLv2);
            }
            //level2 doluysa
            else
            {
                CloseAllRoot(rootsLv2);
                SaveActivateRootStates(Progress.Instance.playerInfo.areLevel2RootsActivate,
                                            rootsLv2);
                if (isActiveList[1] && !IsRootFull(rootsLv3))
                {
                    addNewRoot(rootsLv3);
                    SaveActivateRootStates(Progress.Instance.playerInfo.areLevel3RootsActivate,
                                            rootsLv3);
                }
                //level3 doluysa
                else
                {
                    CloseAllRoot(rootsLv3);
                    SaveActivateRootStates(Progress.Instance.playerInfo.areLevel3RootsActivate,
                                            rootsLv3);
                    if (isActiveList[2] && !IsRootFull(rootsLv4))
                    {
                        addNewRoot(rootsLv4);
                        SaveActivateRootStates(Progress.Instance.playerInfo.areLevel4RootsActivate,
                                                rootsLv4);
                    }
                    //level 4 doluysa
                    else
                    {
                        CloseAllRoot(rootsLv4);
                        SaveActivateRootStates(Progress.Instance.playerInfo.areLevel4RootsActivate,
                                            rootsLv4);
                        if (isActiveList[3] && !IsRootFull(rootsLv5))
                        {
                            addNewRoot(rootsLv5);
                            SaveActivateRootStates(Progress.Instance.playerInfo.areLevel5RootsActivate,
                                                    rootsLv5);
                        }
      
                    }
                }
                
            }
            
        }
    }

    private void Update()
    {
        //sizeup alýnabilir 5. level açýksa baþka level kalmadýðý için alýnamaz olucak.
        if(money >= expandPrice && !isActiveList[3] && !gameObject.GetComponent<AccelerateChainSaw>().isMovingBack)
        {
            sizeUpButton.interactable = true;
        }
        else
        {
            //level 5 aktif ise son level olduðu için image i açýyorum.
            if (isActiveList[3])
            {
                maxLevelobj.SetActive(true);
                expandPriceText.text = "-";
            }
            sizeUpButton.interactable = false;
        }
        //Income button aktiflik
        if(money >= incomePrice)
        {
            incomeButton.interactable = true;
        }
        else
        {
            incomeButton.interactable = false;
        }

        //addtree alýnabilir
        if (money >= treePrice && !canMerge && !IsRootFull(rootsLv1))
        {
            addRootButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            addRootButton.GetComponent<Button>().interactable = false;
        }

    }

    void CheckRoots()
    {
        if (IsRootFull(rootsLv1) && !canMerge)
        {
            arrowAnimation.ShowHint();
            //If level2 is missing
            if (isActiveList[0])
            {
                //If level2 is not full
                if (!IsRootFull(rootsList[0]))
                {
                    canMerge = true;
                    mergeButton.gameObject.SetActive(true);
                    mergeButton.GetComponent<Button>().interactable = true;
                    arrowAnimation.HideHint();
                }
                //level2 doluysa
                else
                {
                    //level3 açýksa
                    if (isActiveList[1])
                    {
                        //level3 dolu deðilse
                        if (!IsRootFull(rootsList[1]))
                        {
                            canMerge = true;
                            mergeButton.gameObject.SetActive(true);
                            mergeButton.GetComponent<Button>().interactable = true;
                            arrowAnimation.HideHint();
                        }
                        //level3 doluysa
                        else
                        {
                            //level4 açýksa
                            if (isActiveList[2])
                            {
                                //level4 dolu deðilse
                                if (!IsRootFull(rootsList[2]))
                                {
                                    canMerge = true;
                                    mergeButton.gameObject.SetActive(true);
                                    mergeButton.GetComponent<Button>().interactable = true;
                                    arrowAnimation.HideHint();
                                }
                                //level 4 doluysa
                                else
                                {
                                    //level5 açýksa
                                    if (isActiveList[3])
                                    {
                                        //level5 dolu deðilse
                                        if (!IsRootFull(rootsList[3]))
                                        {
                                            canMerge = true;
                                            mergeButton.gameObject.SetActive(true);
                                            mergeButton.GetComponent<Button>().interactable = true;
                                            arrowAnimation.HideHint();
                                        }
                                        else
                                        {
                                            Debug.Log("Game Over");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

        }
    }

    public void ExpandLevel()
    {
        //para yetiyorsa
        if(money >= expandPrice)
        {
            money -= expandPrice;           
            moneyText.text = money.ToString("f0");
            expandPrice *= expandMultiply;
            expandPriceText.text = expandPrice.ToString("f0");
            Progress.Instance.playerInfo.moneyCount = money;
            Progress.Instance.playerInfo.expandPrice = expandPrice;
            YandexSDK.Save();
            
            //level2 açýk deðilse açýyoruz
            if (!isActiveList[0])
            {
                OpenNewLevel(2);
                Progress.Instance.playerInfo.sizeLevel = 2;
            }
            else
            {
                //level2 açýk ve 3 açýk deðilse
                if (!isActiveList[1])
                {
                    OpenNewLevel(3);
                    Progress.Instance.playerInfo.sizeLevel = 3;
                }
                else
                {
                    //level3 açýk ve 4 açýk deðilse
                    if (!isActiveList[2])
                    {
                        OpenNewLevel(4);
                        Progress.Instance.playerInfo.sizeLevel = 4;
                    }
                    else
                    {
                        //level4 açýk ve 5 açýk deðilse
                        if (!isActiveList[3])
                        {
                            OpenNewLevel(5);
                            Progress.Instance.playerInfo.sizeLevel = 5;
                        }
                    }
                }
            }
        }
        CheckRoots();
    }

    void ExpandCameraPosition(float expandDuration)
    { 
        cameraMain.transform.DOLocalMove(targetCamPosition, expandDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill()
            .Play();
    }

    void ExpandGroundTop(float expandDuration)
    {
        groundTop.transform.DOScale(areaLength, expandDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill()
            .Play();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        sawMain.transform.Rotate(rotationSpeed * speedMultiply * Time.deltaTime * Vector3.up);
    }
}
