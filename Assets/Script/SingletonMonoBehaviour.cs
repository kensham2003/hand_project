using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance{
        get{
            if(_instance == null){
                //インスタンスがあるか調べる
                _instance = (T)FindObjectOfType(typeof(T));
                if(_instance == null){
                    //ないなら作る
                    SetupInstance();
                }
                else{
                    //すでにある
                }
            }

            return _instance;
        }
    }

    public virtual void Awake(){
        //重複回避
        RemoveDuplicates();
    }

    private static void SetupInstance(){
        _instance = (T)FindObjectOfType(typeof(T));
        if(_instance == null){
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;

            _instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }

    private void RemoveDuplicates(){
        if(_instance == null){
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
}
