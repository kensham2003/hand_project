using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MersenneTwister : SpellCard
{
    //ドロー回数
    [SerializeField] int drawCount = 2;
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
        for(int i = 0; i < drawCount;i++)
        {
            hands.GetComponent<Hands>().MersenneTwisterDraw();
        }
        hands.GetComponent<Hands>().RemoveCard(handsCardNum);
    }
}
