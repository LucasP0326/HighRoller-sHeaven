using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public DeckBuilder deckBuilder; // Reference to the DeckBuilder script

    public void OnMouseDown()
    {
        // Check if the card's parent is an available deck slot
        for (int i = 0; i < deckBuilder.availableDeckSlots.Length; i++)
        {
            if (transform.parent == deckBuilder.availableDeckSlots[i])
            {
                // Add the card to the current deck
                deckBuilder.AddToCurrentDeck(transform);
                return;
            }
        }

        // Check if the card's parent is a current deck slot
        for (int i = 0; i < deckBuilder.currentDeckSlots.Length; i++)
        {
            if (transform.parent == deckBuilder.currentDeckSlots[i])
            {
                // Remove the card from the current deck
                deckBuilder.RemoveFromCurrentDeck(transform);
                return;
            }
        }
    }
}
