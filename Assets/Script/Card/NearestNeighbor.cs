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
        
        if(hit.collider.gameObject.tag == "Player")
        {
            //デバッグ用演出
            GameObject spawnText = Instantiate(damageText,hit.point + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);

            //パラメータによって演出を変更
            switch(type)
            {
                case UpType.HP:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpHP(value);
                spawnText.GetComponent<TextMeshPro>().text = "+"+value.ToString();
                spawnText.GetComponent<TextMeshPro>().color = new Color(0,255,0,1);
                break;

                case UpType.Speed:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpSpeed(value);
                spawnText.GetComponent<TextMeshPro>().text = "+"+value.ToString();
                spawnText.GetComponent<TextMeshPro>().color = new Color(0,0,255,1);
                break;

                case UpType.Attack:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpAttack(value);
                spawnText.GetComponent<TextMeshPro>().text = "+"+value.ToString();
                spawnText.GetComponent<TextMeshPro>().color = new Color(255,0,0,1);
                break;

                case UpType.CoolTime:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpCoolTime(value);
                spawnText.GetComponent<TextMeshPro>().text = "-"+value.ToString();
                spawnText.GetComponent<TextMeshPro>().color = new Color(255,255,0,1);
                break;

            }

            hands.GetComponent<Hands>().RemoveCard(handsCardNum);
        }

        
    }
}
