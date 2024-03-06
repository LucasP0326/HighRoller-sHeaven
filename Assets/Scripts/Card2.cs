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

    public int cardValue; //Numerical value for the cards

    GameManager2 gm;
    public DeckBuilder3 deckBuilder; // Reference to the DeckBuilder script

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
        if (deckBuilder != null)
        {
            //Debug.Log("Deck Builder Found");
            // Check if the card's parent is an available deck slot
            if (deckBuilder.customPlayerDeck.Count < 10)
            {
                for (int i = 0; i < deckBuilder.availableDeckSlots.Length; i++)
                {
                    if (transform.parent == deckBuilder.availableDeckSlots[i])
                    {
                        // Add the card to the current deck
                        deckBuilder.customPlayerDeck.Add(this);
                        deckBuilder.AddToCurrentDeck(transform);
                        return;
                    }
                }
            }
            else
            {
                Debug.Log("Player Deck cannot exceed 10 cards!");
            }

            // Check if the card's parent is a current deck slot
            for (int i = 0; i < deckBuilder.currentDeckSlots.Length; i++)
            {
                if (transform.parent == deckBuilder.currentDeckSlots[i])
                {
                    // Remove the card from the current deck
                    deckBuilder.customPlayerDeck.Remove(this);
                    deckBuilder.RemoveFromCurrentDeck(transform);
                    return;
                }
            }
        }
        else if (deckBuilder == null)
        {
            //Debug.Log("Deck Builder Not Found");
            if (!hasBeenPlayed && playerCard)
            {
                Debug.Log("Player card selected");
                hasBeenPlayed = true;

                gm.PlayerPlayCard(this);
            }
        }
    }
}
