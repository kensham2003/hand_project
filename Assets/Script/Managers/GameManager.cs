using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームマネージャ
/// </summary>
public class GameManager : MonoBehaviour
{
    #region public/serialized
    /// <summary>
    /// CPU使用量とラグ間隔の関係カーブ
    /// </summary>
    [Header("CPU-ラグ間隔の関係カーブ")]
    public AnimationCurve m_cpuUsage_LagIntervalCurve;

    /// <summary>
    /// CPUメイン
    /// </summary>
    [Header("CPUメイン")]
    [SerializeField] CpuMain m_cpuMain;

    /// <summary>
    /// SceneChangeを持っているオブジェ（現在はCanvas）
    /// </summary>
    [Header("Canvas")]
    [SerializeField] SceneChange m_canvas;

    /// <summary>
    /// ゲームオーバーのシーン名
    /// </summary>
    [Header("ゲームオーバーのシーン名")]
    [SerializeField] string m_gameoverSceneName;

    /// <summary>
    /// ゲームクリアのシーン名
    /// </summary>
    [Header("ゲームクリアのシーン名")]
    [SerializeField] string m_clearSceneName;

    [Header("エネミーマネージャ")]
    [SerializeField] EnemyManager m_enemyManager;

    [Header("UIタイマー")]
    [SerializeField] UITimer m_uiTimer;

    #endregion
    
    #region private
    /// <summary>
    /// クリアしたかどうか
    /// </summary>
    private bool m_clearFlag = false;

    /// <summary>
    /// ラグ間隔
    /// </summary>
    private float m_lagInterval = 0.0f;

    /// <summary>
    /// ラグを模擬するコルーチン
    /// </summary>
    Coroutine m_lagCoroutine = null;
    #endregion


    private void Start()
    {
        Time.timeScale = 1f;
        m_cpuMain.OnUsageFull += GameOver;
        if(m_enemyManager){
            m_enemyManager.OnAllEnemyCleared += GameClear;
        }
        if(m_uiTimer){
            m_uiTimer.OnTimerZero += GameClear;
        }
        m_lagCoroutine = StartCoroutine(LagSimulate());
    }

    private void Update()
    {
        if(m_clearFlag){return;}
        LagManager.Instance.lagInterval = EvaluateLagInterval(m_cpuMain.Usage);
        
    }

    /// <summary>
    /// カーブからラグ間隔を計算
    /// </summary>
    /// <param name="cpu">CPU使用量</param>
    float EvaluateLagInterval(float cpu){
        //m_lagInterval = m_cpuUsage_LagIntervalCurve.Evaluate(cpu / (float)100);
        return m_cpuUsage_LagIntervalCurve.Evaluate(cpu / (float)100);
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver(){
        StopCoroutine(m_lagCoroutine);
        //シーン遷移を一回だけ呼ぶ
        if(!m_clearFlag){
            m_canvas.FadeChangeScene(m_gameoverSceneName);
        }
        m_clearFlag = true;
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void GameClear(){
        StopCoroutine(m_lagCoroutine);
        //シーン遷移を一回だけ呼ぶ
        if(!m_clearFlag){
            m_canvas.FadeChangeScene(m_clearSceneName);
        }
        m_clearFlag = true;
    }

    /// <summary>
    /// ラグのシミュレーション
    /// </summary>
    /// <returns></returns>
    IEnumerator LagSimulate(){
        while(true){
            //0なら処理しない
            if(m_lagInterval <= 0.0f){
                yield return null;
                continue;
            }
            //止まったり動いたりする
            Time.timeScale = 0f;
            //WaitForSecondsをキャッシュして使う
            yield return LagTimer.Get(m_lagInterval); //+ランダムノイズでも？
            Time.timeScale = 1f;
            yield return LagTimer.Get(m_lagInterval);
        }
    }
}
