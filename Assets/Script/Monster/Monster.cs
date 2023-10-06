using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [SerializeField] public float raiseRate;
    //影響時間
    [Tooltip("影響時間")]
    [SerializeField] public float impactTime;
}

[System.Serializable]
public struct  MonsterParamerter
{
    //体力
    [Tooltip("体力")]
    [SerializeField] public float hp;
    //体力上限
    [Tooltip("体力最大値（ここはいじらない）")]
    public float maxHp;
    //速度
    [Tooltip("移動速度")]
    [SerializeField] public float speed;
    //ID
    [Tooltip("識別番号")]
    [SerializeField] public int monsterID;
    //攻撃力
    [Tooltip("攻撃力")]
    [SerializeField] public float attack;
    //攻撃距離
    [Tooltip("攻撃範囲")]
    [SerializeField] public float attackDistance;
    //攻撃間隔
    [Tooltip("クールタイム")]
    [SerializeField] public float attackInterval;

    //CPU系
    //常時
    [Tooltip("常時CPU影響")]
    [SerializeField] public CPULoad constantLoad;
    //出現
    [Tooltip("出現時CPU影響")]
    [SerializeField] public CPULoad spawnLoad;
    //攻撃
    [Tooltip("攻撃時CPU影響")]
    [SerializeField] public CPULoad attackLoad;
    //消失
    [Tooltip("消失時CPU影響")]
    [SerializeField] public CPULoad DestroyLoad;


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
    [Tooltip("攻撃ターゲット")]
    protected GameObject target;
    //ターゲットの距離
    protected float targetDistance;
     //デバッグ用ダメージ演出オブジェクト
     [Tooltip("デバッグ用ダメージ演出オブジェクト")]
    [SerializeField] protected GameObject damageText;
    
    //初期マテリアル
    public Material initMaterial;
    //デバッグ用ダメージマテリアル
    [SerializeField] Material debugMaterial;

    //ViewList(画面に映っているモンスター)のインデックス
    protected int viewListIndex; 
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        
        initMaterial = GetComponent<Renderer>().material;

        paramerter.maxHp = paramerter.hp;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Action()
    {
        
    }

    public void ChangeHP(float val)
    {
        paramerter.hp -= val;

        //デバッグ用ダメージ演出
        GameObject spawnText = Instantiate(damageText,transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = val.ToString();

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

    public virtual void Death()
    {
        Destroy(this.gameObject);
    }

    //モンスターの能力上昇
    public void UpHP(float var)
    {
        paramerter.hp += var;

        if(paramerter.hp > paramerter.maxHp)
        {
            paramerter.hp = paramerter.maxHp;
        }
    }
    public void UpSpeed(float var)
    {
        paramerter.speed += var;
    }
    public void UpAttack(float var)
    {
        paramerter.attack += var;
    }
    public void UpCoolTime(float var)
    {
        paramerter.attackInterval -= var;

        if(paramerter.attackInterval < 1)
        {
            paramerter.attackInterval = 1;
        }
    }

}
