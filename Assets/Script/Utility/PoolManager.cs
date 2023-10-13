using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    Dictionary<string, ObjectPool<GameObject>> poolDict = new Dictionary<string, ObjectPool<GameObject>>();
    //ObjectPool<GameObject> pool;

    public GameObject Prefab {get; private set;}

    new void Awake()
    {
        base.Awake();
        //pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    GameObject OnCreatePooledObject(){
        return Instantiate(Prefab);
    }

    void OnGetFromPool(GameObject obj){
        obj.SetActive(true);
    }

    void OnReleaseToPool(GameObject obj){
        obj.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject obj){
        OnDestroyPooledObject(obj);
    }

    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation){
        if(!poolDict.ContainsKey(prefab.name)){
            ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
            poolDict.Add(prefab.name, newPool);
        }
        Prefab = prefab;
        GameObject obj = poolDict[prefab.name].Get();
        obj.name = prefab.name;
        Transform tf = obj.transform;
        tf.position = position;
        tf.rotation = rotation;
        return obj;
    }

    public void ReleaseGameObject(GameObject obj){
        if(poolDict.ContainsKey(obj.name)){
            poolDict[obj.name].Release(obj);
        }
        //pool.Release(obj);
    }
}
