using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//カード名:ニアレストレイバー
//カード効果:味方モンスター1体の攻撃力+2

public enum UpType
{
    HP,
    Speed,
    Attack,
    CoolTime
}

public class NearestNeighbor : SpellCard
{

    //上昇対象
    [Tooltip("上昇対象")]
    [SerializeField] UpType type;

    //上昇量
    [Tooltip("上昇量")]
    [SerializeField] float value;

    // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    //効果発動
    public override void CardEffect(RaycastHit hit)
    {
        
        if(hit.collider.gameObject.GetComponent<PlayerMonster>() != null && hit.collider.gameObject.GetComponent<PlayerBossMonster>() == null)
        {
            //PlayerMonster対象
            

            //パラメータによって演出を変更
            switch(type)
            {
                case UpType.HP:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpHP(value);
                break;

                case UpType.Speed:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpSpeed(value);
                break;

                case UpType.Attack:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpAttack(value);
                break;

                case UpType.CoolTime:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpCoolTime(value);
                break;

            }

            hands.GetComponent<Hands>().RemoveCard(handsCardNum);
        }
        else if(hit.collider.gameObject.GetComponent<PlayerBossMonster>() != null)
        {
            //CPU対象
            //obj生成＆パラメータ設定（種類、上昇量）
            GameObject obj =  instantiateManager.InstantiateMonster(11, hit.point, Quaternion.identity);
            obj.GetComponent<CPUTargetNearestNeighbor>().SetParamerter(type,value);
            hands.GetComponent<Hands>().RemoveCard(handsCardNum);
        }

        
    }
}
