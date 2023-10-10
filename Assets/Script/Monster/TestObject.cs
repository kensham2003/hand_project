using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    int LifeTime;
    // Start is called before the first frame update
    void Start()
    {
        LifeTime = 120;
    }

    void Update(){
        LifeTime--;
        if(LifeTime < 0){
            DestroyThis();
        }
    }

    void DestroyThis(){
        //InstantiateManager.Instance.DestroyMonster(this.gameObject);
    }
}
