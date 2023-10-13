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
    [SerializeField] public MonsterParamerter m_paramerter;
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
    protected int visibleListIndex; //映っていない場合は-1
    protected bool visibleFlag = false;
    protected VisibleList _visibleList;
    public VisibleList visibleList
    {
        get {return _visibleList;}
        set { _visibleList = value; }
    }

    protected CpuMain _cpuMain;
    public CpuMain cpuMain
    {
        get {return _cpuMain;}
        set { _cpuMain = value; }
    }
    
    protected InstantiateManager _instantiateManager;
    public InstantiateManager instantiateManager
    {
        get {return _instantiateManager;}
        set { _instantiateManager = value; }
    }
    
    protected bool _isDead;
    public bool isDead{
        get{return _isDead;}
        set{_isDead = value;}
    }

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

        m_paramerter.maxHp = m_paramerter.hp;
        cpuMain.UsageRegister(m_paramerter.constantLoad);
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
        m_paramerter.hp -= val;

        //デバッグ用ダメージ演出
        GameObject spawnText = Instantiate(m_damageText,transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = val.ToString();

        if(m_paramerter.hp < 0)
        {
            m_paramerter.hp = 0;
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

    //カメラが映らなくなる時の処理
    protected virtual void OnBecameVisibleFromCamera() {
        visibleListIndex = _visibleList.AddVisibleObject(this.gameObject);
        //Debug.Log("my index = " + visibleListIndex);
    }

    //カメラが映るようになる時の処理
    protected virtual void OnBecameInvisibleFromCamera() {
        //不具合ですでに-1になっている時は処理しない
        if(visibleListIndex < 0)return;

        _visibleList.RemoveVisibleObject(visibleListIndex);
        visibleFlag = false;
        visibleListIndex = -1;
    }

    //カメラが映っているかチェック
    protected bool IsVisibleFromCamera(){
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds);
    }

    protected virtual void CheckVisible(){
        if(visibleFlag == IsVisibleFromCamera())return;

        if(visibleFlag){
            //OnBecameInvisibleの処理
            OnBecameInvisibleFromCamera();
        }
        else{
            //OnBecameVisibleの処理
            OnBecameVisibleFromCamera();
        }
        //visibleFlagの状態を保存
        visibleFlag = !visibleFlag;
    }

    protected void OnDestroy() {
        //VisibleListから自分を削除
        if(visibleFlag){
            OnBecameInvisibleFromCamera();
        }
    }
        
    //モンスターの能力上昇
    public void UpHP(float var)
    {
        m_paramerter.hp += var;

        if(m_paramerter.hp > m_paramerter.maxHp)
        {
            m_paramerter.hp = m_paramerter.maxHp;
        }
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(0,255,0,1);
    }
    
    public void UpSpeed(float var)
    {
        m_paramerter.speed += var;
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(0,0,255,1);
    }
    
    public void UpAttack(float var)
    {
        m_paramerter.attack += var;
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(255,0,0,1);
    }
    
    public void UpCoolTime(float var)
    {
        m_paramerter.attackInterval -= var;

        if(m_paramerter.attackInterval < 1)
        {
            m_paramerter.attackInterval = 1;
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
}
