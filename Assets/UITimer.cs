using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UITimer : MonoBehaviour
{
    [Header("クリアまでの秒数")]
    [SerializeField]private float m_clearTime = 30f;

    public bool m_timerActive;

    private TextMeshProUGUI m_timerText;

    private float m_currentTime;

    public event Action OnTimerZero = delegate{};

    // Start is called before the first frame update
    void Start()
    {
        m_currentTime = 0f;
        m_timerActive = true;
        m_timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_timerActive){
            m_currentTime += Time.deltaTime;
            SetUIText();
            if(m_currentTime >= m_clearTime){
                OnTimerZero();
            }
        }
    }

    private void SetUIText(){
        float countdownTime = m_clearTime - m_currentTime;
        if(countdownTime < 0f){countdownTime = 0f;}
        int seconds = Mathf.FloorToInt(countdownTime);
        int mseconds = Mathf.FloorToInt((countdownTime - seconds) * 100);

        string uiTime = string.Format("{0:00}:{1:00}", seconds, mseconds);
        m_timerText.text = uiTime;
    }
}
