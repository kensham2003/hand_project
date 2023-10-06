using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleList : MonoBehaviour
{
    private List<GameObject> VisibleObjects = new List<GameObject>();
    private List<int> emptyIndex = new List<int>(); //空いているスロットのリスト

    void Start(){
        VisibleObjects = new List<GameObject>();
    }

    public List<GameObject> GetVisibleList(){
        return VisibleObjects;
    }

    //VisibleObjectに追加
    public int AddVisibleObject(GameObject gameObject){
        //空いているところあったらそこに挿入
        if(emptyIndex.Count != 0){
            int lastEmptyIndex = emptyIndex[emptyIndex.Count-1];
            //本当に空いているかチェック
            if(VisibleObjects[lastEmptyIndex] == null){
                VisibleObjects[lastEmptyIndex] = gameObject;
                emptyIndex.RemoveAt(emptyIndex.Count-1); //O(1)
                return lastEmptyIndex;
            }
        }
        //空いていないなら最後に挿入
        VisibleObjects.Add(gameObject);
        return VisibleObjects.Count - 1;
    }

    //VisibleObjectから削除
    public void RemoveVisibleObject(int index){
        VisibleObjects[index] = null;
        //indexを空いているスロットに登録
        emptyIndex.Add(index);
    }

    void Update(){
        //デバッグ用
        // Debug.Log("visible objects: " + VisibleObjects.Count);
        // Debug.Log("empty index: " + emptyIndex.Count);
    }
}
