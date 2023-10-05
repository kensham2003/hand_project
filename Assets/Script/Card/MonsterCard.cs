using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MonsterCard : Card
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        image.color = Color.red;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

     //効果発動
    public override void CardEffect(RaycastHit hit)
    {
        //for(int i = 0; i < 30; i++){
            Instantiate(SpawnObject, hit.point, Quaternion.identity);
            //デバッグ用
            // CPULoad cpuLoad;
            // cpuLoad.raiseRate = 10.0f;
            // cpuLoad.impactTime = 7.0f;
            // CpuMain.Instance.UsageRegister(cpuLoad);
        //}
    }
}
