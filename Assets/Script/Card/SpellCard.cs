using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_image.sprite = m_sprite;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 効果発動
    /// </summary>
    /// <param name="hit"></param>
    protected override void CardEffect(RaycastHit hit)
    {
       //スポーン
        // GameObject.Find("InstantiateManager").GetComponent<InstantiateManager>().
        // InstantiateMonster(cardID, hit.point, Quaternion.identity);
        m_instantiateManager.InstantiateMonster(m_cardID, hit.point, Quaternion.identity);

        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }

    protected override void SetCardInfoText()
    {
        //カードテキスト表示
        m_cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(true,m_cardName,m_cardText);
    }
}