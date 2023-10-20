using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CpuCountDown : MonoBehaviour
{
    [SerializeField]CpuMain m_cpu;

    /// <summary>
    /// カウントダウンUI
    /// </summary> <summary>
    /// 
    /// </summary>
    private Image m_countDownImage;
    private TextMeshProUGUI m_countDownText;

    /// <summary>
    /// カウントダウンが始まったフラグ
    /// </summary>
    private bool m_countDown = false;

    private float m_initCountDownTime;
    private float m_countDownTime;
    // Start is called before the first frame update
    void Start()
    {
        m_countDownImage = GetComponent<Image>();
        m_countDownText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        m_initCountDownTime = m_cpu.GetCotuntDownTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_cpu.GetCountDownFlag())
        {
            Visible();
        }
        else
        {
            Hidden();
        }

        //0になったらゲームオーバー
        if(m_countDownTime == 0 && m_countDown)
        {
            CPULoad load;
            load.raiseRate = m_cpu.GetGameOverPercentage();
            load.impactTime = -1;
            m_cpu.UsageRegister(load);
        }
    }

    private void Visible()
    {
        if(m_countDown == false)
        {
            m_countDown = true;
            m_countDownTime = m_initCountDownTime;

            InvokeRepeating("CountDown",1.0f,1.0f);
        }

        m_countDownImage.enabled = true;
        m_countDownText.enabled = true;
        m_countDownText.text = m_countDownTime.ToString();
    }

    private void Hidden()
    {
        if(m_countDown == true)
        {
            m_countDown = false;
            m_countDownTime = m_initCountDownTime;

            CancelInvoke("CountDown");
        }

        m_countDownImage.enabled = false;
        m_countDownText.enabled = false;
    }

    private void CountDown()
    {
        m_countDownTime--;
    }
}
