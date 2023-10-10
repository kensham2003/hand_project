using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    //カード位置
    public List<Vector2> cardPositions = new List<Vector2>(3);

    //最大枚数
    [SerializeField] int maxCount = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //指定したカードのポジション取得
    public Vector2 GetCardPosition(int n)
    {
        if(n > maxCount)
        {
            return cardPositions[n];
        }
        //エラー
        else
        {
            return new Vector2(-1,-1);
        }
        
    }

    //カードスロット追加
    public void AddCardSlot(Vector2 n)
    {
        if(cardPositions.Count < maxCount)
        {
            cardPositions.Add(n);
        }
    }

    //カードスロット減少
    public void DecCardSlot(int n)
    {
        if(cardPositions.Count > 0)
        {
            cardPositions.RemoveAt(n);
        }
    }
}
