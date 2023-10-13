using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// モンスターの生成を管理するクラス
/// </summary>
public class InstantiateManager : MonoBehaviour
{
    /// <summary>
    /// 画面の範囲内にあるオブジェクトのリスト
    /// </summary>
    [Header("可視リスト")]
    [SerializeField]private VisibleList m_visibleList;

    /// <summary>
    /// CPUメイン
    /// </summary>
    [Header("CPUメイン")]
    [SerializeField]private CpuMain m_cpuMain;

    private void Awake(){
        //m_visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
    }

    /// <summary>
    /// モンスターを生成する
    /// </summary>
    /// <param name="cardId">カードID</param>
    /// <param name="position">スポーン位置情報</param>
    /// <param name="rotation">スポーン時の回転情報</param>
    /// <returns>生成されたモンスター</returns>
    public GameObject InstantiateMonster(int cardId, Vector3 position, Quaternion rotation){
        GameObject monsterPrefab = CardMonsterDictionary.Instance.GetMonsterPrefab(cardId);
        GameObject monsterObj = PoolManager.Instance.GetGameObject(monsterPrefab, position, rotation);
        Monster m = monsterObj.GetComponent<Monster>();
        m.visibleList = m_visibleList;
        m.cpuMain = m_cpuMain;
        m.instantiateManager = this;
        m.m_paramerter = CardMonsterDictionary.Instance.GetMonsterParamerter(cardId);
        m.isDead = false;
        //追加　ID設定
        m.m_paramerter.monsterID = cardId;
        cpuMain.UsageRegister(m.m_paramerter.spawnLoad);
        //Debug.Log("生成 : " + m.paramerter.spawnLoad.raiseRate);
        return monsterObj;
    }

    /// <summary>
    /// モンスターを削除（オブジェクトプールにリリース）
    /// </summary>
    /// <param name="monster">モンスターのインスタンス</param>
    public void DestroyMonster(GameObject monster){
        PoolManager.Instance.ReleaseGameObject(monster);
        cpuMain.UsageRegister(monster.GetComponent<Monster>().m_paramerter.DestroyLoad);
        //Debug.Log("消失 : " + monster.GetComponent<Monster>().paramerter.DestroyLoad.raiseRate);
    }

    /// <summary>
    /// エフェクトの生成
    /// <para>ここでやるかは要検討</para>
    /// </summary>
    /// <param name="effectId">エフェクトID</param>
    /// <param name="position">スポーン位置情報</param>
    /// <param name="rotation">スポーン時の回転情報</param>
    public void InstantiateEffect(int effectId, Vector3 position, Quaternion rotation){
        //エフェクトの生成
    }
}
