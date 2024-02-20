using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;
    public GameManager.CardType cardType; // Added variable for card type

    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        if (gm == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnMouseDown()
    {
        if (!hasBeenPlayed && gm != null)
        {
            Debug.Log("Left mouse button clicked!");
            hasBeenPlayed = true;
            gm.PlayerDiscard(this); // Notify the GameManager that this card has been played

            // Get a random card from the AI's deck
            Card aiCard = gm.opponentDeck[UnityEngine.Random.Range(0, gm.opponentDeck.Count)];

            // Start battle after both player and AI have selected cards
            gm.StartBattle(this, aiCard);

            // Replace the played card with the next card from the player's deck
            gm.playerDeck[handIndex] = gm.DrawRandomCardFromDeck(gm.playerDeck);
        }
    }
}