using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilder3 : MonoBehaviour
{
    public Transform[] availableDeckSlots; // Array of available deck slots transformation points
    public Transform[] currentDeckSlots;   // Array of current deck slots transformation points
    public GameObject[] specificAvailableCards; // Specific card game objects to assign to available deck slots
    public string SceneToLoad; // Name of the scene to load
    public List<Card2> customPlayerDeck;
    public DeckData deckData; //Reference to Deck Data

    void Start()
    {
        AssignSpecificAvailableCards();
    }

    public void TransitionToScene()
    {
        // Check if the sceneToLoad reference is not null
        if (!string.IsNullOrEmpty(SceneToLoad))
        {
            // Load the scene by its name
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not assigned.");
        }
    }

    void AssignSpecificAvailableCards()
    {
        // Check if the number of specific available cards matches the number of available deck slots
        if (specificAvailableCards.Length != availableDeckSlots.Length)
        {
            Debug.LogError("Number of specific available cards does not match the number of available deck slots.");
            return;
        }

        // Assign specific cards to available deck slots
        for (int i = 0; i < availableDeckSlots.Length; i++)
        {
            if (specificAvailableCards[i] != null)
            {
                specificAvailableCards[i].transform.position = availableDeckSlots[i].position;
                specificAvailableCards[i].transform.SetParent(availableDeckSlots[i]);
            }
            else
            {
                Debug.LogError("Specific card game object is null for available deck slot " + i);
            }
        }
    }

    public void ResetScene()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Clear the player's deck in DeckData
        DeckData.playerDeck.Clear();
    }

    public void AddToCurrentDeck(Transform cardTransform)
    {
        // Find an empty slot in the current deck
        for (int i = 0; i < currentDeckSlots.Length; i++)
        {
            if (currentDeckSlots[i].childCount == 0)
            {
                // Move the card to the current deck slot
                cardTransform.SetParent(currentDeckSlots[i]);
                cardTransform.localPosition = Vector3.zero;
                // Disable the ClickHandler component to prevent further clicks
                Destroy(cardTransform.GetComponent<ClickHandler>());
                break;
            }
        }
    }

    // Method to remove a card from the current deck slots
    public void RemoveFromCurrentDeck(Transform cardTransform)
    {
        // Find an empty slot in the available deck
        for (int i = 0; i < availableDeckSlots.Length; i++)
        {
            if (availableDeckSlots[i].childCount == 0)
            {
                // Move the card back to the available deck slot
                cardTransform.SetParent(availableDeckSlots[i]);
                cardTransform.localPosition = Vector3.zero;
                // Re-enable the ClickHandler component
                cardTransform.gameObject.AddComponent<ClickHandler>();
                break;
            }
        }
    }

    public void SaveCurrentDeck()
    {
        // Log the contents of playerDeck before saving
        Debug.Log("Before saving: Player Deck Count: " + DeckData.playerDeck.Count);
        foreach (Card2 card in DeckData.playerDeck)
        {
            Debug.Log(card.name); // Assuming card has a "name" field for identification
        }

        // Save the current deck to DeckData
        DeckData.playerDeck = new List<Card2>(customPlayerDeck);

        // Log the contents of playerDeck after saving
        Debug.Log("After saving: Player Deck Count: " + DeckData.playerDeck.Count);
        foreach (Card2 card in DeckData.playerDeck)
        {
            Debug.Log(card.name); // Assuming card has a "name" field for identification
        }
    }
}
