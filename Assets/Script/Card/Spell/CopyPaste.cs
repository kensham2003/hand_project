using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste : SpellCard
{
    //半径
    [Tooltip("半径")]
    [SerializeField] float spawnRange = 2;
     // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    //効果発動
    public override void CardEffect(RaycastHit hit)
    {
        
        if(hit.collider.gameObject.tag == "Player")
        {
            Vector3 direction;
            Vector3 spawnPosition = hit.point;

            direction.x = Random.Range(-1.0f,1.0f);
            direction.z = Random.Range(-1.0f,1.0f);
            direction.y = 0;

            spawnPosition += direction * spawnRange;

            //スポーン
            GameObject.Find("InstantiateManager").GetComponent<InstantiateManager>().
            InstantiateMonster(hit.collider.gameObject.GetComponent<PlayerMonster>().paramerter.monsterID, spawnPosition, Quaternion.identity);
        }

        
    }
}
