using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetVisibleCardInfo(false,"","");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カード情報の視認切り替え
    public void SetVisibleCardInfo(bool f,string cardName,string cardText)
    {
        if(f)
        {
            GetComponent<Image>().color = new Color(255,255,255,0.5f);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cardName;
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cardText;
        }
        else
        {
            GetComponent<Image>().color = new Color(255,255,255,0.0f);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
