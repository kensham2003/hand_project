using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EnemyMonsterType
{
    A,
    B
}

public class EnemyMonster : Monster
{
    //敵の基本仕様
    [Tooltip("敵の基本仕様")]
    public EnemyMonsterType enemyMonsterType;

    //カメラ
    Camera mainCamera;
    //画面内にいるPlayerMonster
    public List<GameObject> objectsInView = new List<GameObject>();

    //CPU
    CpuMain cpumain;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        mainCamera = Camera.main;

        status = Status.idle;

        cpumain = GameObject.Find("Main Camera").GetComponent<CpuMain>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        switch(status)
        {
            
            case Status.idle:
            Idle();
            break;
            
            case Status.move:
            Move();
            break;

            case Status.attack:
            Attack();
            break;
        }
        
    }

    public override void Action()
    {
        attackFlag = false;
        if(target != null && paramerter.attackDistance >= targetDistance)
        {
            target.GetComponent<PlayerMonster>().ChangeHP(paramerter.attack);

            cpuMain.UsageRegister(paramerter.attackLoad);
            Debug.Log("攻撃 : " + paramerter.attackLoad.raiseRate);

            target = null;
        }
    }

    public override void Death()
    {
        if(visibleFlag){
            OnBecameInvisibleFromCamera();
        }
        cpuMain.UsageRegister(paramerter.DestroyLoad);
        Debug.Log("消失 : " + paramerter.DestroyLoad.raiseRate);
        Destroy(this.gameObject);
        //InstantiateManager.Instance.DestroyMonster(this.gameObject);
    }

    //AタイプのUpdate
    void UpdateTypeA()
    {
        if(DetectEnemiesInScreen())
        {
            target = GetClosestBossPlayerMonster();

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
            else if(attackFlag == false &&paramerter.attackDistance >= targetDistance)
            {                
                status = Status.attack;
            }
           

        }
        else
        {
            //前進
            transform.position -= paramerter.speed * transform.forward * Time.deltaTime;
        }
    }

    //BタイプのUpdate
    void UpdateTypeB()
    {
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
            else if(attackFlag == false &&paramerter.attackDistance >= targetDistance)
            {                
                status = Status.attack;
            }
           

        }
        else
        {
            //前進
            transform.position -= paramerter.speed * transform.forward * Time.deltaTime;
        }
    }

    //---------------------------------------------------ここから下は仮後でマネージャーにまとめる
    //画面内に敵がいるかチェック
    //いなければfalse
    bool DetectEnemiesInScreen()
    {
        bool view = false;
        objectsInView.Clear();
        foreach(GameObject obj in visibleList.GetVisibleList()){
            if(obj == null)continue;
            if(obj.GetComponent<PlayerMonster>()){
                objectsInView.Add(obj);
                view = true;
            }
        }
        return view;
    }

    //一番近いオブジェクトを取得
    public GameObject GetClosestObject()
    {
        //一番近いプレイヤーモンスター
        GameObject closestPlayerMonster = GetClosestPlayerMonster();
        //一番近いボスプレイヤーモンスター
        GameObject closestBossPlayerMonster = GetClosestBossPlayerMonster();

        //通常のプレイヤーモンスターがいなければボスをターゲット
        if(closestPlayerMonster == null)
        {
            return closestBossPlayerMonster;
        }
        //通常のプレイヤーモンスターがいれば一番近い通常の敵をターゲット
        else
        {
            return closestPlayerMonster;
        }
    }

    //一番近いPlatyerMonster取得
    public GameObject GetClosestPlayerMonster()
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in objectsInView)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance && !obj.GetComponent<PlayerBossMonster>())
            {
                closestObject = obj;
                shortestDistance = distance;
            }
        }
        return closestObject;
    }

    //一番近いBossEnemy取得
    public GameObject GetClosestBossPlayerMonster()
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in objectsInView)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance && obj.GetComponent<PlayerBossMonster>())
            {
                closestObject = obj;
                shortestDistance = distance;
            }
        }
        return closestObject;
    }

    //待機
    void Idle()
    {
        status = Status.move;
    }

    //移動
    void Move()
    {
        switch(enemyMonsterType)
        {
            //Aタイプ
            case EnemyMonsterType.A:
            UpdateTypeA();
            break;

            //Bタイプ
            case EnemyMonsterType.B:
            UpdateTypeB();
            break;
        }
    }

    //攻撃
    void Attack()
    {
        if(DetectEnemiesInScreen())
        {
            target = GetClosestObject();

            //進行方向
           Vector3 moveVec = target.transform.position - transform.position;
           moveVec = moveVec.normalized;

             //ターゲットの距離
            targetDistance = Vector3.Distance(target.transform.position,transform.position);
            if(attackFlag == false &&paramerter.attackDistance >= targetDistance)
            {                
                Invoke("Action",paramerter.attackInterval);

                attackFlag = true;
            }
            else
            {
                status = Status.move;
            }

           

        }
        else
        {
            status = Status.idle;
        }
    }
}
