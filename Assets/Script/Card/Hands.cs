using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    /// <summary>
    /// 所持カードの枚数
    /// </summary>
    private int m_cardCount;

    /// <summary>
    /// 最大枚数
    /// </summary>
    [SerializeField] private int m_maxCount = 5;
    private CardSlot slot;
    
    /// <summary>
    /// 所持カード
    /// </summary>
    private List<Card> m_handsCard;
    
    /// <summary>
    /// キャンバス
    /// </summary>
    private GameObject m_canvas;
    
    /// <summary>
    /// デッキ
    /// </summary>
    private GameObject m_deck;

    /// <summary>
    /// ドロー間隔
    /// </summary>
    [SerializeField] private float m_drawInterval = 1.0f;

    /// <summary>
    /// ロックオンエフェクト
    /// </summary>
    [SerializeField] private GameObject m_lockOnEffect;

    [SerializeField] private GameObject m_spawLockEffect = null;

    // Start is called before the first frame update
    private void Start()
    {
        m_handsCard = new List<Card>(m_maxCount);
        
        m_canvas = GameObject.Find ("Cards");
        m_deck = GameObject.Find ("Deck");
        
        //限界までドロー
        for(int i = 0;i < m_maxCount;i++)
        {
            Draw();
        }

        InvokeRepeating("Draw",0.0f,m_drawInterval);
    }

    // Update is called once per frame
    private void Update()
    {

        //クリックした対象をターゲットにする（ユニバーサルクロス）
        if(Input.GetMouseButtonUp(0) && GetHorverHandsCard() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f,(1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("PlayerMonster"))))
            {

                if(hit.collider.gameObject.GetComponent<EnemyMonster>() != null || hit.collider.gameObject.GetComponent<PlayerBossMonster>() != null)
                {
                    int i = 0;
                    //シーン上のPlayerMonster収集&生成
                    foreach(GameObject obj in GameObject.Find("Managers").GetComponent<VisibleList>().GetVisibleList())
                    {
                        
                        if(obj == null)continue;
                        if(obj.GetComponent<PlayerMonster>() != null && obj.GetComponent<PlayerBossMonster>() == null)
                        {
                            i++;
                            obj.GetComponent<PlayerMonster>().SetStatus(Status.ucm);
                            obj.GetComponent<PlayerMonster>().SetTarget(hit.collider.gameObject);
                        }
                    }

                    if(m_spawLockEffect != null)
                    {
                        Destroy(m_spawLockEffect);
                    }
                    //ロックオンエフェクト生成
                    m_spawLockEffect = Instantiate(m_lockOnEffect,hit.collider.gameObject.transform.position,Quaternion.identity);
                }
            }
        }
    }

    public void RemoveCard(int n)
    {
        Destroy(m_handsCard[n].gameObject);
        m_handsCard.RemoveAt(n);
        m_cardCount--;

        ChangePosition();
        
    }

    /// <summary>
    /// デッキからカード生成
    /// </summary>
    public void Draw()
    {
        if(m_cardCount < m_maxCount)
        {
            GameObject temp = m_deck.GetComponent<Deck>().Draw();
            m_handsCard.Add(temp.GetComponent<Card>());
            temp.transform.SetParent (m_canvas.transform,false); 
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_cardCount * 280 + 140 , 100 );
            temp.GetComponent<Card>().SetHandsCardNum(m_cardCount);
            m_cardCount++;
        }

    }

    /// <summary>
    /// メルセンヌツイスター用ドロー
    /// </summary>
    public void MersenneTwisterDraw()
    {
        GameObject temp = m_deck.GetComponent<Deck>().Draw();
        m_handsCard.Add(temp.GetComponent<Card>());
        temp.transform.SetParent (m_canvas.transform,false); 
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_cardCount * 280 + 140 , 100 );
        temp.GetComponent<Card>().SetHandsCardNum(m_cardCount);
        m_cardCount++;

    }

    /// <summary>
    /// ターゲットツイスター用ドロー
    /// </summary>
    /// <param name="drawcard"></param>
    public void TargetTwisterDraw(GameObject drawcard)
    {
        GameObject temp = Instantiate (drawcard);
        m_handsCard.Add(temp.GetComponent<Card>());
        temp.transform.SetParent (m_canvas.transform,false); 
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_cardCount * 280 + 140 , 100 );
        temp.GetComponent<Card>().SetHandsCardNum(m_cardCount);
        m_cardCount++;
    }

    /// <summary>
    /// カード位置調整
    /// </summary>
    void ChangePosition()
    {
        int i = 0;
        foreach(Card obj in m_handsCard)
        {
            Vector2 pos = new Vector2(i * 280 + 140 , 100 );
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            //カードの初期位置設定
            obj.SetInitPos(pos);
            //何枚目のカードか設定
            obj.SetHandsCardNum(i);
            i++;
        }
    }

    public List<Card>GetHandsCard()
    {
        return m_handsCard;
    }

    /// <summary>
    /// 手札の中に一枚でもホバーしているか
    /// </summary>
    /// <returns></returns>
    private bool GetHorverHandsCard()
    {
        foreach(Card card in m_handsCard)
        {
            if(card.m_hovered)
            {
                return true;
            }
        }

        return false;
    }
}
