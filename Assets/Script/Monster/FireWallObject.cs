using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireWallObject : EffectMonster
{
    [Tooltip("自滅するまでの時間")]
    [SerializeField] public float lifeTime = 10;
    [Tooltip("ダメージ間隔")]
    [SerializeField] public float damageInterval = 2;
    [Tooltip("ダメージ量")]
    [SerializeField] public float damage = 2;

    //ダメージを与える敵リスト
    List<GameObject> targetEnemys = new List<GameObject>();
    //ダメージを与えるフラグ
    bool damageFlag = false;

    public override void Start()
    {
        Invoke("Death", lifeTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!targetEnemys.Contains(collision.gameObject))
            {
                //ターゲットに追加
                targetEnemys.Add(collision.gameObject);
                if (!damageFlag)
                {
                    damageFlag = true;
                    InvokeRepeating("Damage", damageInterval, damageInterval);
                }
            }
        }
    }
    
    void Damage()
    {
        foreach (GameObject obj in targetEnemys) 
        {
            //ターゲットにダメージ
            if(obj != null)
            {
                obj.GetComponent<EnemyMonster>().ChangeHP(damage);
            }
        }
    }
}