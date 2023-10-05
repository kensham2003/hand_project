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
        base.Start();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //画面チェック
        if(DetectEnemiesInScreen())
        {
            target = GetClosestEnemy();
            if(target == null)
            //待機
            {

            }
            //移動
            else
            {
                //進行方向
                Vector3 moveVec = target.transform.position - transform.position;
                moveVec = moveVec.normalized;

                //ターゲットの距離
                targetDistance = Vector3.Distance(target.transform.position,transform.position);

                if(paramerter.attackDistance < targetDistance)
                {
                    //ターゲット移動
                    transform.position += paramerter.speed * moveVec * Time.deltaTime;
                }
                //攻撃中じゃなければ攻撃
                else if(attackFlag == false && paramerter.attackDistance > targetDistance)
                {                
                    Invoke("Action",paramerter.attackInterval);

                    attackFlag = true;
                }
            }
            
           

        }
    }

    //ターゲットへ攻撃
    public override void Action()
    {
        if(target == true && paramerter.attackDistance >= targetDistance)
        {
            target.GetComponent<EnemyMonster>().ChangeHP(paramerter.attack);
            attackFlag = false;

            //デバッグ用ダメージ演出
            GameObject spawnText = Instantiate(damageText,target.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
            spawnText.GetComponent<TextMeshPro>().text = paramerter.attack.ToString();

            target = null;
        }
        
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }

    //---------------------------------------------------ここから下は仮後でマネージャーにまとめる
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
        //一番近いエネミー
        GameObject closestEnmey = GetClosestEnemy();
        //一番近いボスエネミー
        GameObject closestBossEnmey = GetClosestBossEnemy();

        //通常の敵がいなければボスをターゲット
        if(closestEnmey == null)
        {
            return closestBossEnmey;
        }
        //通常の敵がいれば一番近い通常の敵をターゲット
        else
        {
            return closestEnmey;
        }
    }

    //一番近いEnemy取得
    public GameObject GetClosestEnemy()
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in objectsInView)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance && !obj.GetComponent<EnemyBossMonster>())
            {
                closestObject = obj;
                shortestDistance = distance;
            }
        }
        return closestObject;
    }

    //一番近いBossEnemy取得
    public GameObject GetClosestBossEnemy()
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in objectsInView)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance && obj.GetComponent<EnemyBossMonster>())
            {
                closestObject = obj;
                shortestDistance = distance;
            }
        }
        return closestObject;
    }
}
