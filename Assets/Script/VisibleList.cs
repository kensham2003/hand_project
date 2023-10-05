using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleList : MonoBehaviour
{
    private List<GameObject> VisibleObjects = new List<GameObject>();

    void Start(){
        VisibleObjects = new List<GameObject>();
    }

    public List<GameObject> GetVisibleList(){
        return VisibleObjects;
    }

    public int AddVisibleObject(GameObject gameObject){
        VisibleObjects.Add(gameObject);
        return VisibleObjects.Count - 1;
    }

    public void RemoveVisibleObject(int index){
        VisibleObjects.RemoveAt(index);
    }
}
