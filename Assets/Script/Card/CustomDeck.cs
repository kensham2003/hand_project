using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDeck : MonoBehaviour
{
    /// <summary>
    /// オリジナルのデッキの中身
    /// </summary>
    [SerializeField] private List<string> m_customDeckCard;
    [SerializeField] List<DeckSlot> m_customDeckSlot;
    [SerializeField] private CustomDeckInfo m_customDeckInfo;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i < m_customDeckSlot.Count;i++)
        {
            m_customDeckCard.Add("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < 3;i++)
        {   
            if(m_customDeckSlot[i].GetSlot() == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log("not null");
                m_customDeckCard[i] = m_customDeckSlot[i].GetSlot().name.Remove(m_customDeckSlot[i].GetSlot().name.Length - 7);
            }
        } 
    }

    public void Setting()
    {
        Debug.Log("Setting");
        m_customDeckInfo.SetCustomDeck(m_customDeckCard);
    } 
}
