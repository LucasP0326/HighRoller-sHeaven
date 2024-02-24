using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    // Method to handle card click event
    public void OnMouseDown()
    {
        // Get reference to the DeckBuilder script
        DeckBuilder deckBuilder = FindObjectOfType<DeckBuilder>();

        // Check if the parent of the clicked card is in the available deck slots
        for (int i = 0; i < deckBuilder.availableDeckSlots.Length; i++)
        {
            if (transform.parent == deckBuilder.availableDeckSlots[i])
            {
                // Check if the current deck is full
                if (deckBuilder.currentDeckSlots[i].childCount >= 10)
                {
                    Debug.Log("Deck is full. Cannot add more cards.");
                    return;
                }

                // Move the clicked card to the current deck slot
                transform.SetParent(deckBuilder.currentDeckSlots[i]);
                transform.localPosition = Vector3.zero;
                // Disable the click handler script to prevent further clicks
                Destroy(GetComponent<ClickHandler>());
                return;
            }
        }

        // Check if the parent of the clicked card is in the current deck slots
        for (int i = 0; i < deckBuilder.currentDeckSlots.Length; i++)
        {
            if (transform.parent == deckBuilder.currentDeckSlots[i])
            {
                // Move the clicked card back to the available deck slot
                transform.SetParent(deckBuilder.availableDeckSlots[i]);
                transform.localPosition = Vector3.zero;
                // Enable the click handler script again
                gameObject.AddComponent<ClickHandler>();
                return;
            }
        }
    }
}
