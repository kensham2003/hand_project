using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCopyPaste : EffectMonster
{
    //半径
    [Tooltip("半径")]
    [SerializeField] float spawnRange = 2;
    public List<GameObject> t;
    //シーン上にあるすべてのPlayerMonsterID
    // Start is called before the first frame update
    void Start()
    {
        //シーン上のPlayerMonster収集&生成
        foreach(GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if(obj.tag == "Player")
            {
                Spawn(obj,obj.GetComponent<PlayerMonster>().paramerter.monsterID);
            }
        }

        Death();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn(GameObject obj,int id)
    {
        Vector3 direction;
        Vector3 spawnPosition = obj.transform.position;

        direction.x = Random.Range(-1.0f,1.0f);
        direction.z = Random.Range(-1.0f,1.0f);
        direction.y = 0;

        spawnPosition += direction * spawnRange;

        //スポーン
        GameObject.Find("Managers").GetComponent<InstantiateManager>().
        InstantiateMonster(id, spawnPosition, Quaternion.identity);
    }


}
