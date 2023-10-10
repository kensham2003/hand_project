using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : SpellCard
{
    

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
        
        //スポーン
        GameObject.Find("InstantiateManager").GetComponent<InstantiateManager>().
        InstantiateMonster(cardID, hit.point, Quaternion.identity);

        hands.GetComponent<Hands>().RemoveCard(handsCardNum);
    }
}
