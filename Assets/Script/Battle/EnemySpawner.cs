using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成マネージャ")]
    [SerializeField] private InstantiateManager m_instantiateManager;
    
    /// <summary>
    /// 一体の敵をスポーン
    /// </summary>
    /// <param name="monsterId"></param>
    public void SpawnEnemy(int monsterId){
        Vector3 pos = GetRandomPosition(GetComponent<Collider>().bounds);
        m_instantiateManager.InstantiateMonster(monsterId, pos, Quaternion.identity);
    }


    public void SpawnEnemies(int monsterId, int loopCount){
        for(int i = 0; i < loopCount; i++){
            SpawnEnemy(monsterId);
        }
    }

    //自分のコライダーの範囲内のランダムな位置を返す（高さは一番低い方）
    private Vector3 GetRandomPosition(Bounds bounds){
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
