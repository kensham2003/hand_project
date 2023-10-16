using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class MonsterCard : Card
{

    /// <summary>
    /// プレビューオブジェクト
    /// </summary> <summary>
    /// 
    /// </summary>
    private GameObject m_previewObject;
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

        //image表示＆非表示を設定
        m_image.enabled = !m_pressed;

        if(m_pressed)
        {
            //プレビュー表示
            if(m_previewObject == null)
            {
                m_previewObject = m_instantiateManager.InstantiateMonster(m_cardID,new Vector3(0,1.0f,0.0f), Quaternion.identity);
                m_previewObject.GetComponent<PlayerMonster>().SetPreview(true);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, m_layerMask))
            {
                m_previewObject.transform.position = hit.point;
            }
        }
        else
        {
            //プレビュー非表示
            if(m_previewObject != null)
            {   
                Destroy(m_previewObject);
            }
        }
    }

     //効果発動
    protected override void CardEffect(RaycastHit hit)
    {
        //デバッグ用
        // CPULoad cpuLoad;
        // cpuLoad.raiseRate = 10.0f;
        // cpuLoad.impactTime = 7.0f;
        // CpuMain.Instance.UsageRegister(cpuLoad);

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
