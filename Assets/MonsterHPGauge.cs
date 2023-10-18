using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPGauge : MonoBehaviour
{
    [SerializeField] private Image m_hpGauge;

    [SerializeField] private Monster m_monster;

    private void Start() {
        //m_hpGauge = GetComponent<Image>();
        SetGaugeFill();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// HPゲージを更新
    /// </summary>
    public void SetGaugeFill(){
        MonsterParamerter mp = m_monster.GetParamerter();
        float amount = mp.hp / mp.maxHp;
        m_hpGauge.fillAmount = amount;
    }
}
