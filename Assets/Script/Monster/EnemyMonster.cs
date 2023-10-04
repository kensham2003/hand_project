using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonster : Monster
{
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        //前進
        transform.position -= speed * transform.forward * Time.deltaTime;
    }
}
