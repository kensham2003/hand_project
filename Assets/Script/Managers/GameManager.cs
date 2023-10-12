using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool clearFlag = false;

    [Range(0f, 0.1f)]
    [SerializeField] float lagInterval = 0.0f;

    public AnimationCurve cpuUsage_LagIntervalCurve;

    [SerializeField] CpuMain cpuMain;
    [SerializeField] GameObject clearText;
    //リトライ用のボタン
    [SerializeField] GameObject retryButton;
    [SerializeField] SceneChange canvas;
    [SerializeField] string gameoverSceneName;
    [SerializeField] string clearSceneName;
    Coroutine lagCoroutine = null;

    void Start()
    {
        Time.timeScale = 1f;
        cpuMain.OnUsageFull += GameOver;
        lagCoroutine = StartCoroutine(LagSimulate());
    }

    private void OnDestroy() {
        //cpuMain.OnUsageFull -= GameClear;
        //Debug.Log("unsub");
    }
    void Update()
    {
        if(clearFlag){return;}
        EvaluateLagInterval(cpuMain.Usage);
        
    }

    //ラグ間隔取得
    void EvaluateLagInterval(float cpu){
        lagInterval = cpuUsage_LagIntervalCurve.Evaluate(cpu / (float)100);
    }

    //クリア処理
    public void GameOver(){
        
        //clearText.SetActive(true);
        //retryButton.SetActive(true);
        StopCoroutine(lagCoroutine);
        //Time.timeScale = 0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if(!clearFlag){
            canvas.ChangeScene(gameoverSceneName);
        }
        clearFlag = true;
    }

    IEnumerator LagSimulate(){
        while(true){
            //0なら処理しない
            if(lagInterval <= 0.0f){
                yield return null;
                continue;
            }
            //止まったり動いたりする
            Time.timeScale = 0f;
            //WaitForSecondsをキャッシュして使う
            yield return LagTimer.Get(lagInterval); //+ランダムノイズでも？
            Time.timeScale = 1f;
            yield return LagTimer.Get(lagInterval);
        }
    }
}
