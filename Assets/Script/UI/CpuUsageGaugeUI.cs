using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CpuUsageGaugeUI : MonoBehaviour
{
    [SerializeField] CpuMain cpuMain;
    private Image image;

    private float oldFillAmount, newFillAmount;
    [SerializeField]float fillLerpTime = 0.25f;

    private float nowLerpTime = 0f;

    void Start(){
        image = GetComponent<Image>();
        cpuMain.OnUsageChanged += ChangeUsageUI;
    }

    private void Update() {
        if(image.fillAmount != newFillAmount){
            FillAmountLerpSmooth();
        }
    }

    private void OnDestroy() {
        cpuMain.OnUsageChanged -= ChangeUsageUI;
    }

    private void FillAmountLerpSmooth(){
        image.fillAmount = Mathf.Lerp(oldFillAmount, newFillAmount, Mathf.SmoothStep(0f, 1f, nowLerpTime / fillLerpTime));
        if(nowLerpTime >= fillLerpTime){
            oldFillAmount = newFillAmount;
        }
        nowLerpTime += Time.unscaledDeltaTime;
    }

    public void ChangeUsageUI(float usage){
        nowLerpTime = 0f;
        newFillAmount = usage / 100;
    }
}
