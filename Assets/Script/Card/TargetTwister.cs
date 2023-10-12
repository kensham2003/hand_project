using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTwister : SpellCard
{
    //ドロー回数
    [SerializeField] int drawCount = 2;
    //ドローするカード
    [SerializeField] GameObject drawCard;
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
            hands.GetComponent<Hands>().TargetTwisterDraw(drawCard);
        }
        hands.GetComponent<Hands>().RemoveCard(handsCardNum);
    }
}
