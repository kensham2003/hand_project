using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] Fade m_fade;
    [SerializeField] GameObject m_confirmPanel;

    void Awake(){
        if(m_fade != null){
            m_fade.gameObject.SetActive(true);
        }
        if(m_confirmPanel != null){
            m_confirmPanel.SetActive(false);
        }
    }

    public void ChangeScene(string sceneName){
        m_fade.SetFadeOut(1f, sceneName);
    }

    public void SetConfirmation(bool b){
        m_confirmPanel.SetActive(b);
    }

    //ゲーム終了
    public void EndGame(){
        m_fade.SetFadeOut(1f, "ENDGAME");
    }
}
