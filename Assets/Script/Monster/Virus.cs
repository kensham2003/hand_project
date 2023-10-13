using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : EffectMonster
{
    /// <summary>
    /// 効果が切れるまでの時間
    /// </summary>
    [SerializeField] private float m_lifeTime = 10;
    
    /// <summary>
    /// ダメージ量
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] private float m_damageValue = 2;
    
    /// <summary>
    /// ダメージを与える間隔
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField]  float m_damageInterval = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        Invoke("Death",m_lifeTime);
        InvokeRepeating("Damage",m_damageInterval,m_damageInterval);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    void Damage()
    {
        foreach (GameObject obj in GameObject.Find("Managers").GetComponent<VisibleList>().GetVisibleList())
        {
            EnemyMonster em = obj.GetComponent<EnemyMonster>();
            if(em != null)
            {
                em.ChangeHP(m_damageValue);
            }
        }
    }
}
