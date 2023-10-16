using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Status
{
    idle,
    move,
    attack,
    stop,
    death,
    //ここから下はユニバーサルクロス用
    ucm,
    uca
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
    [SerializeField] public MonsterParamerter m_parameter;
    //モンスターのステータス
    [Tooltip("モンスターのステータス")]
    [SerializeField] public Status m_status;
    //攻撃しているかのフラグ
    [Tooltip("攻撃しているか")]
    protected bool m_attackFlag;
    //攻撃するターゲット
    [Tooltip("攻撃ターゲット")]
    public GameObject m_target;
    //ターゲットの距離
    protected float m_targetDistance;
     //デバッグ用ダメージ演出オブジェクト
     [Tooltip("デバッグ用ダメージ演出オブジェクト")]
    [SerializeField] protected GameObject m_damageText;
    
    //初期マテリアル
    public Material m_initMaterial;
    //デバッグ用ダメージマテリアル
    [SerializeField] Material m_debugMaterial;

    //ViewList(画面に映っているモンスター)のインデックス
    protected int m_visibleListIndex; //映っていない場合は-1
    protected bool m_visibleFlag = false;
    protected VisibleList m_visibleList;
    public VisibleList visibleList
    {
        get {return m_visibleList;}
        set { m_visibleList = value; }
    }

    protected CpuMain m_cpuMain;
    public CpuMain cpuMain
    {
        get {return m_cpuMain;}
        set { m_cpuMain = value; }
    }
    
    protected InstantiateManager m_instantiateManager;
    public InstantiateManager instantiateManager
    {
        get {return m_instantiateManager;}
        set { m_instantiateManager = value; }
    }
    
    protected bool m_isDead;
    public bool isDead{
        get{return m_isDead;}
        set{m_isDead = value;}
    }

    protected bool m_preview = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        m_initMaterial = GetComponent<Renderer>().material;
        if(visibleList == null){
            visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
        }
        if(cpuMain == null){
            cpuMain = GameObject.Find("Managers").GetComponent<CpuMain>();
        }

        m_parameter.maxHp = m_parameter.hp;
        cpuMain.UsageRegister(m_parameter.constantLoad);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckVisible();
    }

    public virtual void Action()
    {
        
    }

    public virtual void ChangeHP(float val)
    {
        m_parameter.hp -= val;

        //デバッグ用ダメージ演出
        GameObject spawnText = Instantiate(m_damageText,transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = val.ToString();

        if(m_parameter.hp < 0)
        {
            m_parameter.hp = 0;
            Death();
        }

        //デバッグ用
        GetComponent<Renderer>().material = m_debugMaterial;
        Invoke("ResetMaterial",0.25f);
        
    }

    //マテリアルを戻す
    void ResetMaterial()
    {
        GetComponent<Renderer>().material = m_initMaterial;
    }

    public virtual void Death()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// カメラが映らなくなる時の処理
    /// </summary>
    protected virtual void OnBecameVisibleFromCamera() {
        m_visibleListIndex = m_visibleList.AddVisibleObject(this.gameObject);
        //Debug.Log("my index = " + visibleListIndex);
    }

    /// <summary>
    /// カメラが映るようになる時の処理
    /// </summary>
    protected virtual void OnBecameInvisibleFromCamera() {
        //不具合ですでに-1になっている時は処理しない
        if(m_visibleListIndex < 0)return;

        m_visibleList.RemoveVisibleObject(m_visibleListIndex);
        m_visibleFlag = false;
        m_visibleListIndex = -1;
    }

    /// <summary>
    /// カメラが映っているかチェック
    /// </summary>
    /// <returns></returns>
    protected bool IsVisibleFromCamera(){
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds);
    }

    protected virtual void CheckVisible(){
        if(m_visibleFlag == IsVisibleFromCamera())return;

        if(m_visibleFlag){
            //OnBecameInvisibleの処理
            OnBecameInvisibleFromCamera();
        }
        else{
            //OnBecameVisibleの処理
            OnBecameVisibleFromCamera();
        }
        //visibleFlagの状態を保存
        m_visibleFlag = !m_visibleFlag;
    }

    protected void OnDestroy() {
        //VisibleListから自分を削除
        if(m_visibleFlag){
            OnBecameInvisibleFromCamera();
        }
    }
        
    /// <summary>
    /// モンスターの能力上昇
    /// </summary>
    /// <param name="var"></param>
    public void UpHP(float var)
    {
        m_parameter.hp += var;

        if(m_parameter.hp > m_parameter.maxHp)
        {
            m_parameter.hp = m_parameter.maxHp;
        }
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(0,255,0,1);
    }
    
    public void UpSpeed(float var)
    {
        m_parameter.speed += var;
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(0,0,255,1);
    }
    
    public void UpAttack(float var)
    {
        m_parameter.attack += var;
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(255,0,0,1);
    }
    
    public void UpCoolTime(float var)
    {
        m_parameter.attackInterval -= var;

        if(m_parameter.attackInterval < 1)
        {
            m_parameter.attackInterval = 1;
        }
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "-"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(255,255,0,1);
    }

    public void SetStatus(Status st)
    {
        m_status = st;
    }


    public void SetTarget(GameObject obj)
    {
        m_target = obj;
    }

    /// <summary>
    /// Monsterをプレビュー状態にする
    /// </summary> <summary>
    /// 
    /// </summary>
    public void SetPreview(bool b)
    {
        m_preview = b;
    }
}
