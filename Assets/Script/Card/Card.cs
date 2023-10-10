using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    //カード名
    [SerializeField] protected string cardName;
    //カードの詳細テキスト
    [SerializeField] protected string cardText;
    //スプライト
    protected Sprite sprite;
    //カードID
    [SerializeField] protected int cardID;
    //押されているフラグ
    protected bool pressed = false;
    //マウスがカード上
    public bool horverd = false;
    //マウス位置
    protected Vector2 mousePos;
    //初期位置
    protected Vector2 initPos;
    
    protected Image image;

    //imageの初期サイズ
    Vector2 imageInitSize;
    //imageのホバーしているときのサイズ
    Vector2 imageHorverSize;
    //すべてのカードで一枚でもホバーしていたらTrue
    
    //デバッグ用演出
    [SerializeField] protected GameObject damageText;

    protected InstantiateManager instantiateManager;
    //手札
    protected GameObject hands;
    //手札の中の何枚目のカードか
    protected int handsCardNum;
    // Start is called before the first frame update

    //カード情報まとめ
    GameObject cardInfoUI;
    GameObject cardNameUI;
    GameObject cardTextUI;
    public bool ourhorvered = false;
    public bool onceHorverd = false;
    public virtual void Start()
    {
        initPos = GetComponent<RectTransform>().anchoredPosition;
        image = GetComponent<Image>();


        imageInitSize = GetComponent<RectTransform>().sizeDelta;
        imageHorverSize = GetComponent<RectTransform>().sizeDelta * 2;
        instantiateManager = GameObject.Find("Managers").GetComponent<InstantiateManager>();

        //手札
        hands = GameObject.Find ("Hands");

        cardInfoUI = GameObject.Find("CardInfo");
        cardNameUI = GameObject.Find("CardName");
        cardTextUI = GameObject.Find("CardText");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Vector2 CardPos = GetComponent<RectTransform>().anchoredPosition;
        mousePos = Input.mousePosition;
        var CardSize = GetComponent<RectTransform>().sizeDelta;
        
        
        //一枚でもホバーしているか
        ourhorvered = false;
        foreach (Card obj in hands.GetComponent<Hands>().GetHandsCard())
        {
            if(obj.handsCardNum == handsCardNum)continue;

            if(obj.horverd == true)
            {
                ourhorvered = true;
            }
        }

        if(CardPos.x + CardSize.x > mousePos.x && CardPos.x - CardSize.x < mousePos.x &&
        CardPos.y + CardSize.y > mousePos.y && CardPos.y - CardSize.y < mousePos.y && ourhorvered == false)
        {
            horverd = true;
            Debug.Log("horver");
        }
        else
        {
            horverd = false;
            Debug.Log("Unhorver");
        }

        if(horverd == true)
        {
           //画像の大きさ変更
           GetComponent<RectTransform>().sizeDelta = imageHorverSize;

            if(Input.GetMouseButton(0))
            {
                press();
            }
            else
            {
                release();
            }

            //カードテキスト表示
            cardInfoUI.GetComponent<Image>().color = new Color(255,255,255,0.5f);
            cardNameUI.GetComponent<TextMeshProUGUI>().text = cardName;
            cardTextUI.GetComponent<TextMeshProUGUI>().text = cardText;
            
        }
        else
        {
            //画像の大きさ変更
            GetComponent<RectTransform>().sizeDelta = imageInitSize;

            //仮の処理
            Object[] allGameObject = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            
            //一枚でもホバーしているか
            onceHorverd = false;
            foreach (Card obj in hands.GetComponent<Hands>().GetHandsCard())
            {
                if(obj.handsCardNum == handsCardNum)continue;
                if(obj.horverd)
                {
                    onceHorverd = true;
                }
            }

            if(onceHorverd == false)
            {
                //Debug.Log(GameObject.Find("CardInfo"));
                //カードテキスト非表示
                cardInfoUI.GetComponent<Image>().color = new Color(255,255,255,0.0f);
                cardNameUI.GetComponent<TextMeshProUGUI>().text = "";
                cardTextUI.GetComponent<TextMeshProUGUI>().text = "";

                
            }
           
        }
        
        if(pressed)
        {
            GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }
    }

     //効果発動
    public virtual  void CardEffect(RaycastHit hit)
    {
        ;
    }

    //押されたら
     void press()
    {
        pressed = true;
    }

    //離したら
    public void release()
    {
        pressed = false;

        if(GetComponent<RectTransform>().anchoredPosition.y > 200)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                CardEffect(hit);

                //デバッグヒットしたオブジェクトの名前  
                Debug.Log(hit.collider.gameObject.name);
            }
        }

        GetComponent<RectTransform>().anchoredPosition = initPos;
    }

    //何枚目か設定
    public void SetHandsCardNum(int n)
    {
        handsCardNum = n;
    }

    //初期位置変更
    public void SetInitPos(Vector2 pos)
    {
        initPos = pos;
    }
}
