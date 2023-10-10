using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CpuUsageUI : MonoBehaviour
{
    [SerializeField] CpuMain cpuMain;

    void Awake(){
        cpuMain.OnUsageChanged += ChangeUsageUI;
        GetComponent<TextMeshProUGUI>().text = ((int)cpuMain.Usage).ToString() + "%";
        
    }

    private void OnDestroy() {
        cpuMain.OnUsageChanged -= ChangeUsageUI;
    }

    public void ChangeUsageUI(float usage){
        GetComponent<TextMeshProUGUI>().text = ((int)usage).ToString() + "%";
    }
}
