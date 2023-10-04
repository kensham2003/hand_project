using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //体力
    [SerializeField] protected int hp;
    //速度
    [SerializeField] protected float speed;
    //ID
    [SerializeField] protected int monsterID;
    //攻撃力
    [SerializeField] protected int attack;
    //攻撃距離
    [SerializeField] protected int attackDistance;
    //攻撃間隔
    [SerializeField] protected float attackInterval;
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
        hp -= val;

        Debug.Log(val);

        if(hp < 0)
        {
            hp = 0;
            Death();
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }


}
