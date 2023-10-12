using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Search;

public class Fade : MonoBehaviour
{
    public enum FADE_MODE{
        FADE_IN,
        FADE_OUT,
        FADE_NONE,
    }

    //フェード前のα値
    private float m_prevAlpha;
    //フェード後のα値
    private float m_targetAlpha;
    //フェードの時間
    private float m_fadeTime;
    //フェード中現在の時間
    private float m_nowFadeTime = 0;
    //オーバーレイ画像
    private Image m_image;
    private bool m_isFading;
    private string m_nextSceneName;
    private FADE_MODE m_fadeMode;
    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        SetFadeIn(1f);
    }

    // Update is called once per frame
    void Update()
    {
        //シーンの最初数フレームのunscaledDeltaTimeが異常に大きいのでフェードの処理をしない
        if(Time.unscaledDeltaTime > 0.1f)return;

        if(m_isFading){
            //フェード
            Color color = m_image.color;
            color.a = Mathf.Lerp(m_prevAlpha, m_targetAlpha, m_nowFadeTime);
            m_nowFadeTime += Time.unscaledDeltaTime / m_fadeTime;
            m_image.color = color;

            //フェードアウト
            if(m_fadeMode == FADE_MODE.FADE_OUT){
                if(m_image.color.a >= 1f){
                    if(m_nextSceneName == "ENDGAME"){
                        EndGame();
                        return;
                    }
                    else{
                        SceneManager.LoadScene(m_nextSceneName);
                    }
                }
            }
            //フェードイン
            else if(m_fadeMode == FADE_MODE.FADE_IN){
                if(m_image.color.a <= 0f){
                    m_fadeMode = FADE_MODE.FADE_NONE;
                    m_isFading = false;
                }
            }
        }
    }

    //フェードインを設定
    public void SetFadeIn(float duration){
        m_fadeMode = FADE_MODE.FADE_IN;
        m_prevAlpha = 1f;
        StartFade(0f, duration);
    }

    //フェードアウトを設定
    public void SetFadeOut(float duration, string sceneName){
        m_fadeMode = FADE_MODE.FADE_OUT;
        m_nextSceneName = sceneName;
        m_prevAlpha = 0f;
        StartFade(1f, duration);
    }

    //フェードの共通処理
    private void StartFade(float targetAlpha, float fadeTime){
        m_targetAlpha = targetAlpha;
        m_fadeTime = fadeTime;
        m_nowFadeTime = 0f;
        m_isFading = true;
    }

    //ゲーム終了処理
    private void EndGame(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
