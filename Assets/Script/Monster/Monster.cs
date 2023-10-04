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
    [Tooltip("上昇率")]
    public float raiseRate;
    //影響時間
    [Tooltip("影響時間")]
    public float impactTime;
}

[System.Serializable]
public struct  MonsterParamerter
{
    //体力
    [Tooltip("体力")]
    public int hp;
    //速度
    [Tooltip("移動速度")]
    public float speed;
    //ID
    [Tooltip("識別番号")]
    public int monsterID;
    //攻撃力
    [Tooltip("攻撃力")]
    public int attack;
    //攻撃距離
    [Tooltip("攻撃範囲")]
    public int attackDistance;
    //攻撃間隔
    [Tooltip("クールタイム")]
    public float attackInterval;

    //CPU系
    //常時
    [Tooltip("常時CPU影響")]
    public CPULoad constantLoad;
    //出現
    [Tooltip("出現時CPU影響")]
    public CPULoad spawnLoad;
    //攻撃
    [Tooltip("攻撃時CPU影響")]
    public CPULoad attackLoad;
    //消失
    [Tooltip("消失時CPU影響")]
    public CPULoad DestroyLoad;


}

public class Monster : MonoBehaviour
{
    //モンスターのパラメーター
    [Tooltip("モンスターのパラメーター")]
    [SerializeField] public MonsterParamerter paramerter;
    //モンスターのステータス
    [Tooltip("モンスターのステータス")]
    [SerializeField] public Status status;
    //攻撃しているかのフラグ
    [Tooltip("攻撃しているか")]
    protected bool attackFlag;
    //攻撃するターゲット
    [Tooltip("一番近いターゲット")]
    protected GameObject target;
     //デバッグ用ダメージ演出オブジェクト
     [Tooltip("デバッグ用ダメージ演出オブジェクト")]
    [SerializeField] protected GameObject damageText;
    
    //初期マテリアル
    public Material initMaterial;
    //デバッグ用ダメージマテリアル
    [SerializeField] Material debugMaterial;


    // Start is called before the first frame update
    public virtual void Start()
    {
        Debug.Log("Start");
        initMaterial = GetComponent<Renderer>().material;
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

        //デバッグ用
        GetComponent<Renderer>().material = debugMaterial;
        Invoke("ResetMaterial",0.25f);
        
    }

    //マテリアルを戻す
    void ResetMaterial()
    {
        GetComponent<Renderer>().material = initMaterial;
    }

    void Death()
    {
        Destroy(this.gameObject);
    }


}
