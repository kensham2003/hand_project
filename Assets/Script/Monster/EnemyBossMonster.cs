using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMonster : EnemyMonster
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        CheckVisible();
    }

    public override void Action()
    {
        
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }
}
