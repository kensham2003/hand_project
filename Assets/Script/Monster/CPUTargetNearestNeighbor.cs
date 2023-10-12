using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUTargetNearestNeighbor : EffectMonster
{
    // Start is called before the first frame update

    //対象モンスターたち
    private List<PlayerMonster> monsters = new List<PlayerMonster>();
    //間隔
    private float interval = 1;
    //自滅までの時間
    private float lifeTime = 5;

    //上昇対象
    private UpType type;

    //上昇量
    private float value;
    void Start()
    {
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            PlayerMonster pm = obj.GetComponent<PlayerMonster>();
            if(pm != null)
            {
                monsters.Add(pm);
            }
        }

        InvokeRepeating("PowerUp",0,interval);
        Invoke("Death",lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PowerUp()
    {
        foreach(PlayerMonster pm in monsters)
        {
            //パラメータによって演出を変更
            switch(type)
            {
                case UpType.HP:
                pm.UpHP(value);
                break;

                case UpType.Speed:
                pm.UpSpeed(value);
                break;

                case UpType.Attack:
                pm.UpAttack(value);
                break;

                case UpType.CoolTime:
                pm.UpCoolTime(value);
                break;

            }
            
        }
    }

    public void SetParamerter(UpType t,float v)
    {
        type = t;
        value = v;
    }
}
