using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    /// <summary>
    /// デッキ構成
    /// </summary>
    [SerializeField] private List<GameObject> m_cardsPrefab;

    /// <summary>
    /// カスタムデッキを使うフラグ
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] private bool m_customFlag = false;

    [SerializeField]private CustomDeckInfo m_customDeck;

    // Start is called before the first frame update
    void Start()
    {
        if(m_customFlag)
        {
            m_cardsPrefab.Clear();
            m_customDeck = GameObject.Find("Singletons").GetComponent<CustomDeckInfo>();
            foreach(string st in m_customDeck.GetCustomDeck())
            {
                m_cardsPrefab.Add((GameObject)Resources.Load("Card/"+st));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// デッキからドロー
    /// </summary>
    /// <param name="random"></param>
    /// <param name="num"></param>
    /// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <param name="random"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public GameObject Draw()
    {
        return  Instantiate (m_cardsPrefab[(Random.Range(0, m_cardsPrefab.Count))]);
    }

    public GameObject Draw(int num)
    {
        return  Instantiate (m_cardsPrefab[num]);
    }
}
