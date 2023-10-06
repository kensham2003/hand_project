using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLock : MonoBehaviour
{
    [Tooltip("自滅するまでの時間")]
    [SerializeField] public float lifeTime = 5;

    [Tooltip("ダメージ量")]
    [SerializeField] public float damage = 20;

    //ロックした敵
    GameObject lockEnemy;
    //ロックフラグ
    bool lockFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && lockFlag == false)
        {
            
            //ロックした敵設定
            lockEnemy = other.gameObject;
            lockFlag = true;

            lockEnemy.GetComponent<EnemyMonster>().SetStatus(Status.stop);

            //ダメージ
            lockEnemy.GetComponent<EnemyMonster>().ChangeHP(damage);

            Invoke("Death",lifeTime);
        }
    }

    void Death()
    {
        lockEnemy.GetComponent<EnemyMonster>().SetStatus(Status.move);

        Destroy(this.gameObject);
    }
}