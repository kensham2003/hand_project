using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
           GameObject target = GetClosestObject();

            //進行方向
           Vector3 moveVec = target.transform.position - transform.position;
           moveVec = moveVec.normalized;

             //ターゲットの距離
            float targetDistance = Vector3.Distance(target.transform.position,transform.position);

            if(attackDistance < targetDistance)
            {
                //ターゲット移動
                transform.position += speed * moveVec * Time.deltaTime;
            }
           

        }
        else
        {
            //前進
            transform.position += speed * transform.forward * Time.deltaTime;
        }
    }

    public override void Action()
    {
        ;
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
