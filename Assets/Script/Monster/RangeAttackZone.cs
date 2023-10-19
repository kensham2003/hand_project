using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲攻撃を受けるプレイヤーモンスターを管理するクラス
/// </summary>
public class RangeAttackZone : MonoBehaviour
{
    [SerializeField]private Monster m_monster;

    private List<Monster> m_monstersInRange = new List<Monster>();

    private float m_damageValue = 1;
    /// <summary>
    /// 範囲内のPlayerMonsterのリストを返す
    /// </summary>
    /// <returns></returns>
    
    void Start()
    {
        m_damageValue = m_monster.m_parameter.attack;
    }
    public List<Monster> GetMonstersInRange(){
        return m_monstersInRange;
    }

    private void OnEnable() {
        m_monstersInRange = new List<Monster>();
        //AddToList(m_playerMonster);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")){
            Monster m = other.GetComponent<Monster>();
            if(m){
                /* AddToList(m);
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit); */

                m.ChangeHP(m_damageValue);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")){
            PlayerMonster pm = other.GetComponent<PlayerMonster>();
            if(pm){
                RemoveFromList(pm);
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
            }
        }
    }

    /// <summary>
    /// PlayerMonsterをリストに追加
    /// </summary>
    /// <param name="playerMonster"></param>
    private void AddToList(Monster playerMonster){
        m_monstersInRange.Add(playerMonster);

        
    }

    /// <summary>
    /// PlayerMonsterをリストから削除
    /// </summary>
    /// <param name="playerMonster"></param>
    /// <returns>削除が成功したかどうか</returns>
    private bool RemoveFromList(Monster Monster){
        return m_monstersInRange.Remove(Monster);
    }
}
