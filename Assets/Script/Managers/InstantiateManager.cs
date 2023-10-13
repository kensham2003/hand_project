using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class InstantiateManager : MonoBehaviour
{
    //[Header("カード一覧のCSVファイル")]
    //public TextAsset cardMonsterCSV;

    //private Dictionary<int, GameObject> cardMonsterDict = new Dictionary<int, GameObject>();
    //private Dictionary<int, MonsterParamerter> cardMonsterParamDict = new Dictionary<int, MonsterParamerter>();

    [Header("可視リスト")]
    [SerializeField]private VisibleList visibleList;

    [Header("CPUメイン")]
    [SerializeField]private CpuMain cpuMain;

    private void Awake(){
        visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
    }

    public GameObject InstantiateMonster(int cardId, Vector3 position, Quaternion rotation){
        GameObject monsterPrefab = CardMonsterDictionary.Instance.GetMonsterPrefab(cardId);
        GameObject monsterObj = PoolManager.Instance.GetGameObject(monsterPrefab, position, rotation);
        
        Monster m = monsterObj.GetComponent<Monster>();
        m.visibleList = visibleList;
        m.cpuMain = cpuMain;
        m.instantiateManager = this;
        m.m_paramerter = CardMonsterDictionary.Instance.GetMonsterParamerter(cardId);
        m.isDead = false;
        //追加　ID設定
        m.m_paramerter.monsterID = cardId;
        cpuMain.UsageRegister(m.m_paramerter.spawnLoad);
        //Debug.Log("生成 : " + m.paramerter.spawnLoad.raiseRate);
        return monsterObj;
    }

    public void DestroyMonster(GameObject monster){
        PoolManager.Instance.ReleaseGameObject(monster);
        cpuMain.UsageRegister(monster.GetComponent<Monster>().m_paramerter.DestroyLoad);
        //Debug.Log("消失 : " + monster.GetComponent<Monster>().paramerter.DestroyLoad.raiseRate);
    }

    public void InstantiateEffect(int effectId, Vector3 position, Quaternion rotation){
        //エフェクトの生成
    }
}
