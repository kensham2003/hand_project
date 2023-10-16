using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireWallObject : EffectMonster
{
    /// <summary>
    /// 効果が切れるまでの時間
    /// </summary>
    [Tooltip("自滅するまでの時間")]
    [SerializeField] private float m_lifeTime = 10;
    
    /// <summary>
    /// ダメージを与える間隔
    /// </summary>
    [Tooltip("ダメージ間隔")]
    [SerializeField] private float m_damageInterval = 2;

    /// <summary>
    /// ダメージ量
    /// </summary> <summary>
    /// 
    /// </summary>
    [Tooltip("ダメージ量")]
    [SerializeField] private float m_damage = 2;

    /// <summary>
    /// ダメージを与える敵リスト
    /// </summary> <summary>
    /// 
    /// </summary>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    List<GameObject> m_targetEnemys = new List<GameObject>();
    
    /// <summary>
    /// ダメージを与えるフラグ
    /// </summary>
    private bool m_damageFlag = false;

    protected override void Start()
    {
        Invoke("Death", m_lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!m_targetEnemys.Contains(collision.gameObject))
            {
                //ターゲットに追加
                m_targetEnemys.Add(collision.gameObject);
                if (!m_damageFlag)
                {
                    m_damageFlag = true;
                    InvokeRepeating("Damage", m_damageInterval, m_damageInterval);
                }
            }
        }
    }
    
    private void Damage()
    {
        foreach (GameObject obj in m_targetEnemys) 
        {
            //ターゲットにダメージ
            if(obj != null)
            {
                obj.GetComponent<EnemyMonster>().ChangeHP(m_damage);
            }
        }
    }
}