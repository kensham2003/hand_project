using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangedCpuUsage : MonoBehaviour
{
    /// <summary>
    /// テキストの上る速度
    /// </summary>
    [Header("テキストの上る速度")]
    [SerializeField] float m_ySpeed = 0.05f;

    /// <summary>
    /// フェードにかかる秒数
    /// </summary>
    [Header("フェードにかかる秒数")]
    [SerializeField] float m_fadeTime = 1f;

    /// <summary>
    /// テキストのα値
    /// </summary>
    [Header("テキストのα値")]
    [SerializeField] AnimationCurve m_textAlpha;
    private TextMeshProUGUI m_text;
    private RectTransform m_rectTransform;
    private float m_currentTime;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_rectTransform = GetComponent<RectTransform>();
        m_currentTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_currentTime += Time.fixedDeltaTime;
        TextFade();
        if(m_currentTime > m_fadeTime){
            Destroy(this.gameObject);
        }
    }

    public void SetText(int changed){
        if(changed == 0)return;
        m_text = GetComponent<TextMeshProUGUI>();
        string text = "";
        if(changed > 0){
            m_text.color = Color.red;
            text += "+";
        }
        else{
            m_text.color = Color.blue;
            text += "-";
        }
        text += Mathf.Abs(changed).ToString() + "%";
        
        m_text.text = text;
    }

    /// <summary>
    /// テキストのフェード演出
    /// </summary>
    private void TextFade(){
        //位置
        Vector2 pos = m_rectTransform.anchoredPosition;
        Debug.Log(pos.y);
        pos.y += m_ySpeed;
        m_rectTransform.anchoredPosition = pos;
        //α値
        float alpha = m_textAlpha.Evaluate(m_currentTime / m_fadeTime);
        Color color = m_text.color;
        color.a = alpha;
        m_text.color = color;
    }
}
