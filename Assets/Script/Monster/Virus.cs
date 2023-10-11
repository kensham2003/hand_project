using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : EffectMonster
{
    //死ぬまでの時間
    [SerializeField]  float lifeTime = 10;
    //ダメージ量
    [SerializeField]  float damageValue = 2;
    //ダメージ間隔
    [SerializeField]  float damageInterval = 1;
    // Start is called before the first frame update
    void Start()
    {

        Invoke("Death",lifeTime);

        InvokeRepeating("Damage",damageInterval,damageInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Damage()
    {
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            EnemyMonster em = obj.GetComponent<EnemyMonster>();
            if(em != null)
            {
                em.ChangeHP(damageValue);
            }
        }
    }
}
