using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeckBuilder3 : MonoBehaviour
{
    public Transform[] availableDeckSlots; // Array of available deck slots transformation points
    public Transform[] currentDeckSlots;   // Array of current deck slots transformation points
    public GameObject[] specificAvailableCards; // Specific card game objects to assign to available deck slots
    public TextMeshProUGUI cardDescription; //Add space for card description;
    public string SceneToLoad; // Name of the scene to load
    public List<Card2> customPlayerDeck;
    public List<Card2> unchosenCards;
    public DeckData deckData; //Reference to Deck Data
    public AudioSource cardChoose;

    void Start()
    {
        AssignSpecificAvailableCards();
        cardDescription.text = "";
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
        if (unchosenCards.Count != availableDeckSlots.Length)
        {
            Debug.LogError("Number of specific available cards does not match the number of available deck slots.");
            return;
        }

        // Assign specific cards to available deck slots
        for (int i = 0; i < availableDeckSlots.Length; i++)
        {
            if (unchosenCards[i] != null)
            {
                unchosenCards[i].transform.position = availableDeckSlots[i].position;
                //specificAvailableCards[i].transform.SetParent(availableDeckSlots[i]);
            }
            else
            {
                Debug.LogError("Specific card game object is null for available deck slot " + i);
            }
        }
    }

    public void ResetScene()
    {
        //Disable Redundancies
        foreach (Card2 card in customPlayerDeck)
        {
            card.gameObject.SetActive(false);
        }
        foreach (Card2 card in unchosenCards)
        {
            card.gameObject.SetActive(false);
        }
        
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Clear the player's deck in DeckData
        DeckData.playerDeck.Clear();
        customPlayerDeck.Clear();
    }

    public void AddToCurrentDeck(Transform cardTransform)
    {
        cardChoose.Play();
        // Find an empty slot in the current deck
        for (int i = 0; i < currentDeckSlots.Length; i++)
        {
            if (currentDeckSlots[i].transform.position != customPlayerDeck[i].transform.position)
            {
                // Move the card to the current deck slot
                customPlayerDeck[i].transform.position = currentDeckSlots[i].transform.position;
                // Disable the ClickHandler component to prevent further clicks
                //break;
            }
        }
    }

    // Method to remove a card from the current deck slots
    public void RemoveFromCurrentDeck(Transform cardTransform)
    {
        cardChoose.Play();
        // Find an empty slot in the available deck
        for (int i = 0; i < availableDeckSlots.Length; i++)
        {
            if (availableDeckSlots[i].transform.position != unchosenCards[i].transform.position)
            {
                // Move the card back to the available deck slot
                //cardTransform.SetParent(availableDeckSlots[i]);
                unchosenCards[i].transform.position = availableDeckSlots[i].transform.position;
                // Re-enable the ClickHandler component
                //break;
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
