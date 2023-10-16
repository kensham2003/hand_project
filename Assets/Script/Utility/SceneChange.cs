using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を行うするクラス
/// </summary>
public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// フェードのスクリプト
    /// </summary>
    [Header("フェードのスクリプト")]
    [SerializeField] private Fade m_fade;

    /// <summary>
    /// 終了確認パネル
    /// </summary>
    [Header("終了確認パネル")]
    [SerializeField] private GameObject m_confirmPanel;

    private void Awake(){
        if(m_fade != null){
            m_fade.gameObject.SetActive(true);
        }
        if(m_confirmPanel != null){
            m_confirmPanel.SetActive(false);
        }
    }

    /// <summary>
    /// シーン遷移を設定
    /// </summary>
    /// <param name="sceneName">次のシーン名</param>
    public void ChangeScene(string sceneName){
        m_fade.SetFadeOut(1f, sceneName);
    }

    /// <summary>
    /// 終了確認パネルを有効・無効化
    /// </summary>
    /// <param name="b"></param>
    public void SetConfirmation(bool b){
        m_confirmPanel.SetActive(b);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void EndGame(){
        m_fade.SetFadeOut(1f, "ENDGAME");
    }
}
