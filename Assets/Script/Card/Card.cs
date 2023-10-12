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
    [SerializeField] protected Sprite sprite;
    //カードID
    [SerializeField] protected int cardID;
    //押されているフラグ
    protected bool pressed = false;
    //マウスがカード上
    public bool hovered = false;
    //マウス位置
    protected Vector2 mousePos;
    //初期位置
    protected Vector2 initPos;
    
    protected Image image;

    //imageの初期サイズ
    Vector2 imageInitSize;
    //imageのホバーしているときのサイズ
    Vector2 imageHoverSize;
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
    protected GameObject cardInfoUI;


    
    public virtual void Start()
    {
        initPos = GetComponent<RectTransform>().anchoredPosition;
        image = GetComponent<Image>();


        imageInitSize = GetComponent<RectTransform>().sizeDelta;
        imageHoverSize = GetComponent<RectTransform>().sizeDelta * 2;
        instantiateManager = GameObject.Find("Managers").GetComponent<InstantiateManager>();

        //手札
        hands = GameObject.Find ("Hands");
        //カード情報UI
        cardInfoUI = GameObject.Find("CardInfo");
        //透明部分をレイキャストに当たらない（スプライトから「Read\Write」をチェックする）
        image.alphaHitTestMinimumThreshold = 0.5f;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
        
        
        //一枚でもホバーしているか
        bool oneceHorvered = false;
        foreach (Card obj in hands.GetComponent<Hands>().GetHandsCard())
        {
            if(obj.handsCardNum == handsCardNum)continue;
            if(obj.hovered == true)
            {
                oneceHorvered = true;
            }
        }

        //マウスがカードの上にあるか判断
        //horverd = CheckMouseOnCard();
        //Debug.Log(gameObject.name + " : " + horverd);

        if(hovered == true && oneceHorvered == false)
        {
           //画像の大きさ変更
           GetComponent<RectTransform>().sizeDelta = imageHoverSize;

            if(Input.GetMouseButton(0))
            {
                press();
            }
            else
            {
                release();
            }

            //カードテキスト表示
            //cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(true,cardName,cardText);
            SetCardInfoText();
        }
        else
        {
            //画像の大きさ変更
            GetComponent<RectTransform>().sizeDelta = imageInitSize;
            
            //一枚でもホバーしているか
            oneceHorvered = false;
            foreach (Card obj in hands.GetComponent<Hands>().GetHandsCard())
            {
                if(obj.handsCardNum == handsCardNum)continue;
                if(obj.hovered)
                {
                    oneceHorvered = true;
                }
            }

            if(oneceHorvered == false)
            {
                //Debug.Log(GameObject.Find("CardInfo"));
                //カードテキスト非表示
                cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(false,"","");
            }
           
        }
        
        if(pressed)
        {
            //CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
            float scale = (float)Screen.width / 1920;
            //Debug.Log(scale);
            GetComponent<RectTransform>().anchoredPosition = Input.mousePosition / scale;
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

    protected virtual void SetCardInfoText(){}
    

    //カードの上にマウスがあるか判断
    bool CheckMouseOnCard()
    {
        Vector2 CardPos = GetComponent<RectTransform>().anchoredPosition;
        mousePos = Input.mousePosition;
        //Debug.Log(mousePos);
        var CardSize = GetComponent<RectTransform>().sizeDelta;

        if(CardPos.x + CardSize.x > mousePos.x && CardPos.x - CardSize.x < mousePos.x &&
        CardPos.y + CardSize.y > mousePos.y && CardPos.y - CardSize.y < mousePos.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
