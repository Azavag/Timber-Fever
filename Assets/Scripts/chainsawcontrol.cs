using Unity.VisualScripting;
using UnityEngine;

public class chainsawcontrol : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject floatingtext;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tree")
        {
            Trees tree = other.GetComponent<Trees>();
            if (!tree.canBeCutDown)
                return;
            //Bodynin rigidbodysini ve collider�n� �ekiyoruz.
            Rigidbody treerb = other.GetComponent<Rigidbody>();
            CapsuleCollider treeCollider = other.GetComponent<CapsuleCollider>();
            //Bodynin kesildikten sonra istenilen noktaya d��mesi i�in kuvvet uyguluyoruz.
            treerb.AddForce(transform.forward * 50);
            treerb.AddForce(transform.up * 15);
            //Para textini a�ac�n reward� olarak g�ncelliyoruz.
            float finalMoneyReward = other.GetComponent<Trees>().moneyReward * gameManager.GetComponent<GameManager>().incomeMultiply;
            floatingtext.GetComponent<TextMesh>().text = finalMoneyReward.ToString("0.") + "$";

            //Para textini olu�turuyoruz           
            Instantiate(floatingtext, other.transform.position, Quaternion.identity);
            //A�a� kesildi�i i�in paray� artt�r�yoruz.
            gameManager.GetComponent<GameManager>().money += other.GetComponent<Trees>().moneyReward * gameManager.GetComponent<GameManager>().incomeMultiply;
            gameManager.GetComponent<GameManager>().moneyText.text = gameManager.GetComponent<GameManager>().money.ToString("0") + "$";
            Progress.Instance.playerInfo.moneyCount = gameManager.GetComponent<GameManager>().money;
            YandexSDK.Save();
            //Rootun art�k bo� oldu�unu belirtiyoruz ve parenttan ay�r�yoruz.
            other.GetComponentInParent<root>().isRootEmpty = true;
            //Destroy(other.transform.parent.gameObject); //Pivot bo� objeyi siliyoruz
            other.transform.SetParent(null);
            //Bodynin d��mesi i�in gravity a��yoruz ve trigger �zelli�ini kapat�yoruz.
            treerb.useGravity = true;
            treeCollider.isTrigger = false;
            tree.canBeCutDown = false;
        }

    }
}
