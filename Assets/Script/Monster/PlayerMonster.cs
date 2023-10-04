using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMonster : Monster
{
    //カメラ
    Camera mainCamera;
    //画面内にいる敵
    public List<GameObject> objectsInView = new List<GameObject>();

    // Start is called before the first frame update
    public override void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    public override void Update()
    {
        
        //画面チェック
        if(DetectEnemiesInScreen())
        {
            target = GetClosestObject();

            //進行方向
           Vector3 moveVec = target.transform.position - transform.position;
           moveVec = moveVec.normalized;

             //ターゲットの距離
            float targetDistance = Vector3.Distance(target.transform.position,transform.position);

            if(paramerter.attackDistance < targetDistance)
            {
                //ターゲット移動
                transform.position += paramerter.speed * moveVec * Time.deltaTime;
            }
            //攻撃中じゃなければ攻撃
            else if(attackFlag == false)
            {
                
                Invoke("Action",paramerter.attackInterval);

                //デバッグ用ダメージ演出
                GameObject spawnText = Instantiate(damageText,target.transform.position + new Vector3( 0.0f, 3.0f, 0.0f), Quaternion.identity);
                spawnText.GetComponent<TextMeshPro>().text = paramerter.attack.ToString();
                attackFlag = true;
            }
           

        }
        else
        {
            //前進
            transform.position += paramerter.speed * transform.forward * Time.deltaTime;
        }
    }

    //ターゲットへ攻撃
    public override void Action()
    {
        if(target == true)
        {
            target.GetComponent<EnemyMonster>().ChangeHP(paramerter.attack);
            attackFlag = false;

            target = null;
        }
        
    }

    //画面内に敵がいるかチェック
    //いなければfalse
    bool DetectEnemiesInScreen()
    {
        bool view = false;

         objectsInView.Clear();
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(obj.transform.position);
            // ビューポート座標が (0,0) と (1,1) の間にあるかどうかを確認
            if (viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
            {
                if(obj.GetComponent<EnemyMonster>())
                {
                    objectsInView.Add(obj);

                    view = true;
                }
                
            }
        }

        return view;
    }

    //一番近いオブジェクトを取得
    public GameObject GetClosestObject()
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in objectsInView)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance)
            {
                closestObject = obj;
                shortestDistance = distance;
            }
        }
        return closestObject;
    }
}
