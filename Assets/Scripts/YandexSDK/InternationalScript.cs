using TMPro;
using UnityEngine;

public class InternationalScript : MonoBehaviour
{
    [TextArea]
    [SerializeField] string _ru;
    [TextArea]
    [SerializeField] string _en;

    private void Start()
    {
        if(Language.Instance.currentLanguage == "ru")
            GetComponent<TextMeshProUGUI>().text = _ru;
        else 
            GetComponent<TextMeshProUGUI>().text = _en;        
    }
}
