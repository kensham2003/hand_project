using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDeckInfo : SingletonMonoBehaviour<CustomDeckInfo>
{
    [SerializeField] private List<string> m_customDeckCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCustomDeck(List<string> deck)
    {
        m_customDeckCard = deck;
    }

    public List<string> GetCustomDeck()
    {
        return m_customDeckCard;
    }
}
