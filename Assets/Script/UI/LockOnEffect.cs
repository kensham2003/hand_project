using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnEffect : MonoBehaviour
{
    /// <summary>
    /// 表示するオブジェクト（位置）
    /// </summary>
    private GameObject m_target = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_target != null)
        {
            transform.position = m_target.transform.position;
        }
    }

    public void SetTarget(GameObject obj)
    {
        m_target = obj;
    }
}
