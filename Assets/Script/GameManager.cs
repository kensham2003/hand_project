using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    bool clearFlag = false;

    [Range(0f, 0.1f)]
    [SerializeField] float lagInterval = 0.0f;

    public AnimationCurve cpuUsage_LagIntervalCurve;

    [SerializeField] CpuMain cpuMain;
    [SerializeField] GameObject clearText;

    Coroutine lagCoroutine = null;

    void Start()
    {
        cpuMain.OnUsageFull += GameClear;
        lagCoroutine = StartCoroutine(LagSimulate());
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
    public void GameClear(){
        clearFlag = true;
        clearText.SetActive(true);
        StopCoroutine(lagCoroutine);
        Time.timeScale = 0f;
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
