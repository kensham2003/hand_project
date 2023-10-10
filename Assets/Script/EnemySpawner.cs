using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private InstantiateManager instantiateManager;
    
    public void SpawnEnemy(int monsterId){
        Vector3 pos = GetRandomPosition(GetComponent<Collider>().bounds);
        instantiateManager.InstantiateMonster(monsterId, pos, Quaternion.identity);
    }

    private Vector3 GetRandomPosition(Bounds bounds){
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
