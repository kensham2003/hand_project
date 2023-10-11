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
    death
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
    [SerializeField] public MonsterParamerter paramerter;
    //モンスターのステータス
    [Tooltip("モンスターのステータス")]
    [SerializeField] public Status status;
    //攻撃しているかのフラグ
    [Tooltip("攻撃しているか")]
    protected bool attackFlag;
    //攻撃するターゲット
    [Tooltip("攻撃ターゲット")]
    protected GameObject target;
    //ターゲットの距離
    protected float targetDistance;
     //デバッグ用ダメージ演出オブジェクト
     [Tooltip("デバッグ用ダメージ演出オブジェクト")]
    [SerializeField] protected GameObject damageText;
    
    //初期マテリアル
    public Material initMaterial;
    //デバッグ用ダメージマテリアル
    [SerializeField] Material debugMaterial;

    //ViewList(画面に映っているモンスター)のインデックス
    protected int visibleListIndex; //映っていない場合は-1
    protected bool visibleFlag = false;
    private VisibleList _visibleList;
    public VisibleList visibleList
    {
        get {return _visibleList;}
        set { _visibleList = value; }
    }

    private CpuMain _cpuMain;
    public CpuMain cpuMain
    {
        get {return _cpuMain;}
        set { _cpuMain = value; }
    }
    
    private InstantiateManager _instantiateManager;
    public InstantiateManager instantiateManager
    {
        get {return _instantiateManager;}
        set { _instantiateManager = value; }
    }
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        
        initMaterial = GetComponent<Renderer>().material;
        if(visibleList == null){
            visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
        }
        if(cpuMain == null){
            cpuMain = GameObject.Find("Managers").GetComponent<CpuMain>();
        }

        paramerter.maxHp = paramerter.hp;
        cpuMain.UsageRegister(paramerter.constantLoad);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CheckVisible();
    }

    public virtual void Action()
    {
        
    }

    public virtual void ChangeHP(float val)
    {
        paramerter.hp -= val;

        //デバッグ用ダメージ演出
        GameObject spawnText = Instantiate(damageText,transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = val.ToString();

        if(paramerter.hp < 0)
        {
            paramerter.hp = 0;
            Death();
        }

        //デバッグ用
        GetComponent<Renderer>().material = debugMaterial;
        Invoke("ResetMaterial",0.25f);
        
    }

    //マテリアルを戻す
    void ResetMaterial()
    {
        GetComponent<Renderer>().material = initMaterial;
    }

    public virtual void Death()
    {
        Destroy(this.gameObject);
    }

    //カメラが映らなくなる時の処理
    protected virtual void OnBecameVisibleFromCamera() {
        visibleListIndex = _visibleList.AddVisibleObject(this.gameObject);
    }

    //カメラが映るようになる時の処理
    protected virtual void OnBecameInvisibleFromCamera() {
        //不具合ですでに-1になっている時は処理しない
        if(visibleListIndex < 0)return;

        _visibleList.RemoveVisibleObject(visibleListIndex);
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
        paramerter.hp += var;

        if(paramerter.hp > paramerter.maxHp)
        {
            paramerter.hp = paramerter.maxHp;
        }
    }
    
    public void UpSpeed(float var)
    {
        paramerter.speed += var;
    }
    
    public void UpAttack(float var)
    {
        paramerter.attack += var;
    }
    
    public void UpCoolTime(float var)
    {
        paramerter.attackInterval -= var;

        if(paramerter.attackInterval < 1)
        {
            paramerter.attackInterval = 1;
        }
    }

    public void SetStatus(Status st)
    {
        status = st;
    }

}
