using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusButton : MonoBehaviour
{
    [SerializeField] private Button bonusButton;
    [SerializeField] private GameManager gameManager;
    [Header("Bonus Timer")]
    [SerializeField] private int speedMultiply;
    [SerializeField] private TextMeshProUGUI bonusTimerText;
    [SerializeField] private float bonusTimeInterval;
    bool isBonusTime;
    [SerializeField] private TextMeshProUGUI multiplyText;
    [Header("ADS Delay Timer")]
    [SerializeField] private TextMeshProUGUI delayTimerText;
    [SerializeField] private float delayTimeInterval;
    [SerializeField] Image fadeImage;
    [SerializeField] Image adsImage;
    bool isDelayTime;

    private float timer;


    [SerializeField] AdvManager advManager;
    bool isRewarded;
    private void Awake()
    {
        bonusButton.onClick.AddListener(ClickBonusButton);
    }
    // Start is called before the first frame update
    void Start()
    {    
        StartDelayTime();
    }

    private void Update()
    {
        if (isDelayTime)
        {
            timer -= Time.deltaTime;
            UpdateText(delayTimerText);
            FadeImageFill();
            if (timer < 0)
                StopDelayTime();
        }
        if (isBonusTime)
        {
            timer -= Time.deltaTime;
            UpdateText(bonusTimerText);
            
            if (timer < 0)
                StopBonusTime();
        }        
    }

    void ClickBonusButton()
    {
#if !UNITY_EDITOR
        advManager.ShowRewardedAdv();
#else
        GetBonus();
#endif

    }
    //Â jslib
    public void GetBonus()
    {
        if(isRewarded)
        {
            gameManager.speedMultiply = speedMultiply;
            StartBonusTime();
            NotReadyForReward();
        }       
    }

    public void GetReadyForReward()
    {
        isRewarded = true;
    }
    public void NotReadyForReward()
    {
        isRewarded = false;
    }
    
    void StartBonusTime()
    {
        isBonusTime = true;
        bonusTimerText.enabled = true;
        adsImage.enabled = false;
        bonusButton.interactable = false;
        multiplyText.enabled = false;
        ResetTimer(bonusTimeInterval);
        UpdateText(bonusTimerText);
    }
    void StopBonusTime()
    {
        isBonusTime = false;
        gameManager.speedMultiply = 2;
        adsImage.enabled = true;
        multiplyText.enabled = true;
        StartDelayTime();
    }
    void StartDelayTime()
    {
        ResetTimer(delayTimeInterval);
        UpdateText(delayTimerText);
        bonusButton.interactable = false;
        delayTimerText.enabled = true;
        bonusTimerText.enabled = false;
        isDelayTime = true;
        fadeImage.fillAmount = 1;
    }
    void StopDelayTime()
    {
        bonusButton.interactable = true;
        isDelayTime = false;
        delayTimerText.enabled = false;
        ResetTimer(delayTimeInterval);
    }
    void FadeImageFill()
    {
        fadeImage.fillAmount = timer/delayTimeInterval;

    }
    void UpdateText(TextMeshProUGUI _text)
    {
        _text.text = timer.ToString("0");
    }

    void ResetTimer(float interval)
    {
        timer = interval;
    }
}
