using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MonsterCard : Card
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

     //効果発動
    protected override void CardEffect(RaycastHit hit)
    {
        //デバッグ用
        // CPULoad cpuLoad;
        // cpuLoad.raiseRate = 10.0f;
        // cpuLoad.impactTime = 7.0f;
        // CpuMain.Instance.UsageRegister(cpuLoad);
        
        //スポーン
        // GameObject.Find("InstantiateManager").GetComponent<InstantiateManager>().
        // InstantiateMonster(cardID, hit.point, Quaternion.identity);
        m_instantiateManager.InstantiateMonster(m_cardID, hit.point + new Vector3(0,1.0f,0.0f), Quaternion.identity);

        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }

    protected override void SetCardInfoText()
    {
        MonsterParamerter mp = CardMonsterDictionary.Instance.GetMonsterParamerter(m_cardID);
        //カードテキスト表示
        m_cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(true, m_cardName, mp.hp.ToString(), mp.attack.ToString(), mp.speed.ToString(), m_cardText);
    }
}
