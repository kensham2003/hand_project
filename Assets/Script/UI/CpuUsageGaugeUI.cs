using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CpuUsageGaugeUI : MonoBehaviour
{
    [Header("CPUメイン")]
    [SerializeField] CpuMain cpuMain;

    [Header("線形補間のかかる時間")]
    [SerializeField]float fillLerpTime = 0.25f;

    //ゲージのスプライト
    private Image image;
    //画像のフィル量
    private float oldFillAmount, newFillAmount;
    //線形補間の現在時間
    private float nowLerpTime = 0f;

    void Start(){
        image = GetComponent<Image>();
        //イベント登録
        cpuMain.OnUsageChanged += ChangeUsageUI;
    }

    private void Update() {
        if(oldFillAmount != newFillAmount){
            FillAmountLerpSmooth();
        }
    }

    private void OnDestroy() {
        //なくてもいけそう
        cpuMain.OnUsageChanged -= ChangeUsageUI;
    }

    //スプライトのフィル量の線形補間（スムーズ版）
    private void FillAmountLerpSmooth(){
        image.fillAmount = Mathf.Lerp(oldFillAmount, newFillAmount, Mathf.SmoothStep(0f, 1f, nowLerpTime / fillLerpTime));
        if(nowLerpTime >= fillLerpTime){
            oldFillAmount = newFillAmount;
        }
        nowLerpTime += Time.unscaledDeltaTime;
    }

    public void ChangeUsageUI(float usage){
        nowLerpTime = 0f;
        //最大100想定
        newFillAmount = usage / 100;
    }
}
