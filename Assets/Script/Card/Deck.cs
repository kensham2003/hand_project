using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    /// <summary>
    /// デッキ構成
    /// </summary>
    [SerializeField] private List<GameObject> cardsPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Draw()
    {
        return  Instantiate (cardsPrefab[(Random.Range(0, cardsPrefab.Count))]);
    }
}
