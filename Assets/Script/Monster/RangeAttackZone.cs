using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲攻撃を受けるモンスターを管理するクラス
/// </summary>
public class RangeAttackZone : MonoBehaviour
{
    [SerializeField]private PlayerMonster m_playerMonster;

    private List<PlayerMonster> m_playerMonstersInRange = new List<PlayerMonster>();

    /// <summary>
    /// 範囲内のPlayerMonsterのリストを返す
    /// </summary>
    /// <returns></returns>
    public List<PlayerMonster> GetPlayerMonstersInRange(){
        return m_playerMonstersInRange;
    }

    private void Start() {
        AddToList(m_playerMonster);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            PlayerMonster pm = other.GetComponent<PlayerMonster>();
            if(pm){
                AddToList(pm);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            PlayerMonster pm = other.GetComponent<PlayerMonster>();
            if(pm){
                RemoveFromList(pm);
            }
        }
    }

    /// <summary>
    /// PlayerMonsterをリストに追加
    /// </summary>
    /// <param name="playerMonster"></param>
    private void AddToList(PlayerMonster playerMonster){
        m_playerMonstersInRange.Add(playerMonster);
    }

    /// <summary>
    /// PlayerMonsterをリストから削除
    /// </summary>
    /// <param name="playerMonster"></param>
    /// <returns>削除が成功したかどうか</returns>
    private bool RemoveFromList(PlayerMonster playerMonster){
        return m_playerMonstersInRange.Remove(playerMonster);
    }
}
