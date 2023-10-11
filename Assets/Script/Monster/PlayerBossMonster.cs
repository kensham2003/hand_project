using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossMonster : PlayerMonster
{
    [Header("ダメージ持続時間（-1:永続的）")]
    [SerializeField]private float damageTime = -1;

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

    //ターゲットへ攻撃
    public override void Action()
    {
       
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }

    public override void ChangeHP(float val)
    {
        base.ChangeHP(val);
        CPULoad cpuLoad = new CPULoad{raiseRate = val, impactTime = damageTime};
        cpuMain.UsageRegister(cpuLoad);
    }

}
