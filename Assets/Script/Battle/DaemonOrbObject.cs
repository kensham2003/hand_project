using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaemonOrbObject : MonoBehaviour
{
    //速度
    [Tooltip("移動速度")]
    [SerializeField] public float speed = 2;
    //攻撃力
    [Tooltip("攻撃力")]
    [SerializeField] public float damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * new Vector3(0,0,1) * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyMonster>().ChangeHP(damage);
            Destroy(this.gameObject);
        }
        
    }
}
