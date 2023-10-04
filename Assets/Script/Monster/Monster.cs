using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    idle,
    attack,
    death
}

//CPU負荷単位
[System.Serializable]
public struct  CPULoad
{
    //上昇率
    public float raiseRate;
    //影響時間
    public float impactTime;
}

[System.Serializable]
public struct  MonsterParamerter
{
    //体力
    public int hp;
    //速度
    public float speed;
    //ID
    public int monsterID;
    //攻撃力
    public int attack;
    //攻撃距離
    public int attackDistance;
    //攻撃間隔
    public float attackInterval;

    //CPU系
    //常時
    public CPULoad constantLoad;
    //出現
    public CPULoad spawnLoad;
    //攻撃
    public CPULoad attackLoad;
    //消失
    public CPULoad DestroyLoad;


}

public class Monster : MonoBehaviour
{
    //モンスターのパラメーター
    [SerializeField] public MonsterParamerter paramerter;
    //モンスターのステータス
    [SerializeField] public Status status;
    //攻撃しているかのフラグ
    protected bool attackFlag;
    //攻撃するターゲット
    protected GameObject target;
     //デバッグ用ダメージ演出オブジェクト
    [SerializeField] protected GameObject damageText;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Action()
    {
        
    }

    public void ChangeHP(int val)
    {
        paramerter.hp -= val;

        if(paramerter.hp < 0)
        {
            paramerter.hp = 0;
            Death();
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }


}
