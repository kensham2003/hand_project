using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaemonOrbObject : EffectMonster
{
    //速度
    [Tooltip("移動速度")]
    [SerializeField] public float speed = 2;
    //攻撃力
    [Tooltip("攻撃力")]
    [SerializeField] public float damage = 20;

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        //移動
        transform.position += speed * new Vector3(0,0,1) * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //ダメージ
            collision.gameObject.GetComponent<EnemyMonster>().ChangeHP(damage);
            Destroy(this.gameObject);
        }
        
    }
}
