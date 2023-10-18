using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMonster : Monster
{
    //カメラ
    Camera mainCamera;
    //画面内にいる敵
    [SerializeField] List<GameObject> m_objectsInView = new List<GameObject>();

    /// <summary>
    /// 範囲攻撃のスクリプト
    /// </summary>
    [SerializeField]private RangeAttackZone m_rangeAttackZone;

    /// <summary>
    /// 範囲攻撃受ける時生成する爆風オブジェクト
    /// </summary>
    [SerializeField]private GameObject m_explosion;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;

        //ステータス設定
        m_status = Status.move;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(LagManager.Instance.canUpdate == false)return;
        base.Update();

        if(m_preview)return;
        
        switch(m_status)
        {
            //target = GetClosestObject();
            //if(target == null)
            ////待機
            //{

            //}
            ////移動
            //else
            //{
            //    //進行方向
            //    Vector3 moveVec = target.transform.position - transform.position;
            //   moveVec = moveVec.normalized;

            //    //ターゲットの距離
            //    targetDistance = Vector3.Distance(target.transform.position,transform.position);

            //    if(paramerter.attackDistance < targetDistance)
            //    {
            //        //ターゲット移動
            //        transform.position += paramerter.speed * moveVec * Time.deltaTime;
            //    }
            //    //攻撃中じゃなければ攻撃
            //    else if(attackFlag == false && paramerter.attackDistance > targetDistance)
            //    {                
            //        Invoke("Action",paramerter.attackInterval);

            //        attackFlag = true;
            //    }
            //}
            case Status.idle:
            Idle();
            break;
            case Status.move:
            Move();
            break;
            case Status.attack:
            Attack();
            break;
            case Status.ucm:
            UCMove();
            break;
            case Status.uca:
            UCAttack();
            break;
        }

        
    }

    //ターゲットへ攻撃
    public override void Action()
    {
        m_attackFlag = false;
        
        if(m_target != null && m_parameter.attackDistance >= m_targetDistance)
        {
            if(m_parameter.attackDistance < 5.99f || m_target.GetComponent<EnemyBossMonster>() != null){
                m_target.GetComponent<EnemyMonster>().ChangeHP(m_parameter.attack);
            }
            else{
                m_target.GetComponent<EnemyMonster>().ChangeHPInRange(m_parameter.attack);
            }
            
            cpuMain.UsageRegister(m_parameter.attackLoad);
            //Debug.Log("攻撃 : " + paramerter.attackLoad.raiseRate);

            m_target = null;
        }
        else if(m_status != Status.ucm)
        {
            m_status = Status.idle;
        }
        
    }
    
    public override void Death()
    {
        if(isDead)return;
        if(m_visibleFlag){
            OnBecameInvisibleFromCamera();
            m_visibleFlag = false;
        }
        cpuMain.UsageRegister(m_parameter.DestroyLoad);
        //Debug.Log("消失 : " + paramerter.DestroyLoad.raiseRate);
        CPULoad constant = new CPULoad{raiseRate = -1 * m_parameter.constantLoad.raiseRate, impactTime = -1};
        cpuMain.UsageRegister(constant);
        isDead = true;
        //Debug.Log("death");
        //InstantiateManager.Instance.DestroyMonster(this.gameObject);
        instantiateManager.DestroyMonster(this.gameObject);
    }

    //---------------------------------------------------ここから下は仮後でマネージャーにまとめる
    //画面内に敵がいるかチェック
    //いなければfalse
    bool DetectEnemiesInScreen()
    {
        bool view = false;
        m_objectsInView.Clear();
        foreach(GameObject obj in visibleList.GetVisibleList())
        {
            if(obj == null)continue;
            if(obj.GetComponent<EnemyMonster>()){
                m_objectsInView.Add(obj);
                view = true;
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
        foreach (GameObject obj in m_objectsInView)
        {
            if(obj == null)continue;
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
        foreach (GameObject obj in m_objectsInView)
        {
            if(obj == null)continue;
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance && obj.GetComponent<EnemyBossMonster>())
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
        //画面チェック
        if(DetectEnemiesInScreen())
        {
            m_status = Status.move;
        }
    }

    //移動
    void Move()
    {
        //画面チェック
        if(DetectEnemiesInScreen())
        {
            m_target = GetClosestObject();
            if(m_target == null)
            //待機
            {
                m_status = Status.idle;
            }
            //移動
            else
            {
                //進行方向
                Vector3 moveVec = m_target.transform.position - transform.position;
                moveVec = moveVec.normalized;

                //ターゲットの距離
                m_targetDistance = Vector3.Distance(m_target.transform.position,transform.position);

                if(m_parameter.attackDistance < m_targetDistance)
                {
                    //ターゲット移動
                    transform.position += m_parameter.speed * moveVec * Time.deltaTime;
                }
                //攻撃中じゃなければ攻撃
                else if(m_attackFlag == false && m_parameter.attackDistance > m_targetDistance)
                {                
                    m_status = Status.attack;
                }
            }
        }
        else
        {
            //画面に敵がいなければIdleへ
            m_status = Status.idle;
        }
    }

    //攻撃
    void Attack()
    {
        m_target = GetClosestObject();
        if(m_target == null)
        //待機
        {
            m_status = Status.idle;
        }
        //攻撃
        else if(m_target != null)
        {
            m_targetDistance = Vector3.Distance(m_target.transform.position,transform.position);
            
            if(m_attackFlag == false && m_parameter.attackDistance > m_targetDistance)
            {                
                Invoke("Action",m_parameter.attackInterval);
                {
                    m_attackFlag = true;
                }
            }
            else
            {
                m_status = Status.move;
            }
        }
    }

    //ユニバーサルクロス用移動
    void UCMove()
    {
        if(m_target == null)
        {
            m_status = Status.idle;
            
            return;
        }
        
        //進行方向
        Vector3 moveVec = m_target.transform.position - transform.position;
        moveVec = moveVec.normalized;

        //ターゲットの距離
        m_targetDistance = Vector3.Distance(m_target.transform.position,transform.position);

        if(m_parameter.attackDistance < m_targetDistance)
        {
            //ターゲット移動
           transform.position += m_parameter.speed * moveVec * Time.deltaTime;
        }
        else
        {
            if(m_target.GetComponent<EnemyMonster>() != null)
            {
                //ターゲットが敵だったらucaへ
                m_status = Status.uca;
            }
            else
            {
                //ターゲットが敵ではなければidleへ
                m_status = Status.idle;
            }
            
        }
    }

    //ユニバーサルクロス用攻撃
    void UCAttack()
    {
        if(m_target == null)
        {
            //死んでいたらidleへ
            m_status = Status.idle;
            return;
        }

        m_targetDistance = Vector3.Distance(m_target.transform.position,transform.position);
            
        if(m_attackFlag == false && m_parameter.attackDistance > m_targetDistance)
        {                
            Invoke("Action",m_parameter.attackInterval);
            {
                m_attackFlag = true;
            }
        }
        else
        {
            m_status = Status.ucm;
        }
    }

    /// <summary>
    /// 範囲内の味方もダメージを食らう
    /// </summary>
    /// <param name="val">ダメージ量</param>
    public void ChangeHPInRange(float val){
        if(m_rangeAttackZone){
            Instantiate(m_explosion, transform.position, Quaternion.identity);
            foreach(PlayerMonster pm in m_rangeAttackZone.GetPlayerMonstersInRange()){
                pm.ChangeHP(val);
            }
        }
        else{
            this.ChangeHP(val);
        }
    }
}
