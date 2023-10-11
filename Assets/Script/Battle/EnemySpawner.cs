using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成マネージャ")]
    [SerializeField] private InstantiateManager instantiateManager;
    
    //敵をスポーン
    public void SpawnEnemy(int monsterId){
        Vector3 pos = GetRandomPosition(GetComponent<Collider>().bounds);
        instantiateManager.InstantiateMonster(monsterId, pos, Quaternion.identity);
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
