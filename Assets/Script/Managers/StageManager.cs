using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
    /// <summary>
    /// 最大ステージ数
    /// </summary>
    [Header("最大ステージ数")]
    public int m_maxStage = 3;
    private int m_currentStage;
    /// <summary>
    /// 現在のステージ
    /// </summary>
    public int currentStage
    {
        get { return m_currentStage; }
        set { m_currentStage = value; }
    }

    new private void Awake() {
        base.Awake();
        m_currentStage = 1;
    }

    public int GetNextStage(){
        return m_currentStage + 1;
    }

    public bool IsLastStage(){
        return m_currentStage >= m_maxStage;
    }
}
