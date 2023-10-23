using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskKill : EffectMonster
{
    private CpuMain m_cpu;

    /// <summary>
    /// CPUパラメーター
    /// </summary>
    [SerializeField]CPULoad m_cpuLoad;

    protected override void Start()
    {
        m_cpu = GameObject.Find("Managers").GetComponent<CpuMain>();

        foreach (GameObject obj in GameObject.Find("Managers").GetComponent<VisibleList>().GetVisibleList())
        {
            if(obj == null)continue;
            if(obj.gameObject.tag == "Player" || obj.gameObject.name == "FireWall")
            {
                m_instantiateManager.DestroyMonster(obj);
            }
        }

        //CPU削減
        m_cpu.UsageRegister(m_cpuLoad);

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
