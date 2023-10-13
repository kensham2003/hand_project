using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private Camera m_mainCam;

    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField] private float m_speed = 3.0f;

    private void Start()
    {
        m_mainCam = Camera.main;

        Invoke("Delete",2.0f);
    }
    private void Update()
    {
        transform.LookAt(transform.position + m_mainCam.transform.rotation * Vector3.forward, m_mainCam.transform.rotation * Vector3.up);

        //前進
        transform.position += m_speed * new Vector3(0,0,1).normalized * Time.deltaTime;
    }

    private void Delete()
    {
        Destroy(this.gameObject);
    }
}
