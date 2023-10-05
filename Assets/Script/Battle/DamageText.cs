using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private Camera mainCam;

    //速度
    [SerializeField] protected float speed = 3.0f;

    private void Start()
    {
        mainCam = Camera.main;

        Invoke("Delete",2.0f);
    }
    private void Update()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);

        //前進
        transform.position += speed * new Vector3(0,0,1).normalized * Time.deltaTime;
    }

    void Delete()
    {
        Destroy(this.gameObject);
    }
}
