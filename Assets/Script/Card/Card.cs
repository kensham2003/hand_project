using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //カード名
    protected string cardName;
    //カードの詳細テキスト
    protected string cardText;
    //スプライト
    protected Sprite sprite;
    //カードID
    [SerializeField] protected int cardID;
    //押されているフラグ
    protected bool pressed = false;
    //マウスがカード上
    protected bool horverd = false;
    //マウス位置
    protected Vector2 mousePos;
    //初期位置
    protected Vector2 initPos;
    
    protected Image image;

    //imageの初期サイズ
    Vector2 imageInitSize;
    //imageのホバーしているときのサイズ
    Vector2 imageHorverSize;

    // Start is called before the first frame update
    public virtual void Start()
    {
        initPos = GetComponent<RectTransform>().anchoredPosition;
        image = GetComponent<Image>();


        imageInitSize = GetComponent<RectTransform>().sizeDelta;
        imageHorverSize = GetComponent<RectTransform>().sizeDelta * 2;
    }

    // Update is called once per frame
    public virtual void Update()
    {
         Vector2 CardPos = GetComponent<RectTransform>().anchoredPosition;
        mousePos = Input.mousePosition;
        var CardSize = GetComponent<RectTransform>().sizeDelta;
        

        if(CardPos.x + CardSize.x > mousePos.x && CardPos.x - CardSize.x < mousePos.x &&
        CardPos.y + CardSize.y > mousePos.y && CardPos.y - CardSize.y < mousePos.y)
        {
            horverd = true;
        }
        else
        {
            horverd = false;
        }

        if(horverd)
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

            
        }
        else
        {
            //画像の大きさ変更
           GetComponent<RectTransform>().sizeDelta = imageInitSize;
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
            }
        }

        GetComponent<RectTransform>().anchoredPosition = initPos;
    }
}
