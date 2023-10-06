using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterObject : MonoBehaviour
{
    //ダメージを与える範囲
    [Tooltip("ダメージを与える範囲")]
    [SerializeField] public float damageRange = 3.0f;
    //ダメージ量
    [Tooltip("ダメージ量")]
    [SerializeField] public float damage = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if(obj.tag == "Enemy")
            {
                //距離で比較
                float distance = Vector3.Distance(obj.transform.position,transform.position);
                if(distance < damageRange)
                {
                    obj.GetComponent<EnemyMonster>().ChangeHP(damage);
                }
            }
        }

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
