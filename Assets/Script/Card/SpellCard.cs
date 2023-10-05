using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelCard : Card
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        image.color = Color.blue;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

     //効果発動
    public override void CardEffect(RaycastHit hit)
    {
        Instantiate(SpawnObject, hit.point, Quaternion.identity);
    }
}
