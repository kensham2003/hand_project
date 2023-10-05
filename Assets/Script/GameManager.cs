using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    enum type{
        A,
        B,
        C,
    }

    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
