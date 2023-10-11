using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CpuMain : MonoBehaviour
{
    private float usage;
    public float Usage
    {
        get { return usage; }
        set { usage = value; }
    }
    
    //使用率変わった時呼ぶイベント
    public event Action<float> OnUsageChanged = delegate{};

    //使用率が100%になった時呼ぶイベント
    public event Action OnUsageFull = delegate{};

    // new void Awake(){
    //     base.Awake();
    //     usage = 0;
    // }
    private void Awake() {
        usage = 0;
    }

    private void UsageChange(float amount){
        usage += amount;
        if(usage >= 100){
            usage = 100;
            //UI更新・クリア処理
            OnUsageChanged(usage);
            OnUsageFull();
            return;
        }
        if(usage <= 0){
            usage = 0;
        }
        //UI更新
        OnUsageChanged(usage);
    }

    //CPU使用率に変動を登録
    public void UsageRegister(CPULoad cpuLoad){
        UsageChange(cpuLoad.raiseRate);

        //常時上昇ならここまで
        if(cpuLoad.impactTime < 0)return;

        StartCoroutine(RemoveUsage(cpuLoad));
    }

    IEnumerator RemoveUsage(CPULoad cpuLoad){
        yield return new WaitForSeconds(cpuLoad.impactTime);
        UsageChange(-1 * cpuLoad.raiseRate);
    }
}
