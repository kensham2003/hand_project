using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    //所持カードの枚数
    public int cardCount;
    //最大枚数
    [SerializeField] int maxCount = 5;
    public CardSlot slot;
    //所持カード
    public List<Card> handsCard;
    
    //キャンバス
    private GameObject canvas;
    //デッキ
    private GameObject deck;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find ("Canvas");
        deck = GameObject.Find ("Deck");
        
        InvokeRepeating("Draw",2.0f,2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveCard(int n)
    {
        Destroy(handsCard[n].gameObject);
        handsCard.RemoveAt(n);
        cardCount--;

        ChangePosition();
        
    }

    public void Draw()
    {
        if(cardCount < maxCount)
        {
            //テスト用カード生成
            GameObject temp = deck.GetComponent<Deck>().Draw();
            handsCard.Add(temp.GetComponent<Card>());
            temp.transform.SetParent (canvas.transform,false); 
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(cardCount * 280 + 140 , 100 );
            temp.GetComponent<Card>().SetHandsCardNum(cardCount);
            cardCount++;
        }

    }

    void ChangePosition()
    {
        int i = 0;
        foreach(Card obj in handsCard)
        {
            Vector2 pos = new Vector2(i * 280 + 140 , 100 );
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            obj.SetInitPos(pos);
            obj.SetHandsCardNum(i);
            i++;
        }
    }
}
