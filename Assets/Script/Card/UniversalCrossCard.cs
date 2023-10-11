using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCrossCard : SpellCard
{
    // Start is called before the first frame update
    public override void Start()
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
        if(hit.collider.gameObject.GetComponent<PlayerBossMonster>() != null || hit.collider.gameObject.GetComponent<EnemyMonster>() != null)
        {
            //CPUかエネミー
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if(obj.tag == "Player")
                {
                    PlayerMonster pm = obj.GetComponent<PlayerMonster>();

                    pm.SetStatus(Status.ucm);
                    pm.SetTarget(hit.collider.gameObject);
                }
            }

            hands.GetComponent<Hands>().RemoveCard(handsCardNum);
        }

        
    }
}
