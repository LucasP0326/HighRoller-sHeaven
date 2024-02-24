using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card2 : MonoBehaviour
{
    public bool hasBeenPlayed;
    public bool playerCard;
    public int handIndex;
    public GameManager2.CardType cardType; // Added variable for card type

    GameManager2 gm;

    // Start is called before the first frame update
    private void Start()
    {
        gm = FindObjectOfType<GameManager2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (!hasBeenPlayed && playerCard)
        {
            Debug.Log("Player card selected");
            hasBeenPlayed = true;

            gm.PlayerPlayCard(this);
        }
    }
}
