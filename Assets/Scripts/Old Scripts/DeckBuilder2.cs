using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder2 : MonoBehaviour
{
    public GameObject deckBuildingCanvas;
    public GameObject battleCanvas;
    public Button toBattleButton;
    public Button saveButton;
    public Button resetButton;

    public Transform[] availableDeckSlots; // Array of available deck slots transformation points
    public Transform[] currentDeckSlots;   // Array of current deck slots transformation points

    public GameObject[] specificAvailableCards; // Specific card game objects to assign to available deck slots

    void Start()
    {
        // Ensure that the deck-building canvas is active when the scene starts
        deckBuildingCanvas.SetActive(true);
        battleCanvas.SetActive(false);

        // Add a listener to the toBattleButton to switch to the battle phase
        toBattleButton.onClick.AddListener(SwitchToBattle);

        AssignSpecificAvailableCards();
    }

    void SwitchToBattle()
    {
        // Deactivate the deck-building canvas and activate the battle canvas
        deckBuildingCanvas.SetActive(false);
        battleCanvas.SetActive(true);

        // Activate the GameManager3 script to start the battle
        GameManager3.Instance.StartBattle();
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

    // Method to add a card to the current deck slots
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

    // Method to reset the scene
    void ResetScene()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Method to save the current deck data
    public void SaveCurrentDeck()
    {
        // Create a string to store the card names in the deck
        List<string> cardNames = new List<string>();

        // Iterate through the current deck slots and add card names to the list
        for (int i = 0; i < currentDeckSlots.Length; i++)
        {
            if (currentDeckSlots[i].childCount > 0)
            {
                // Get the card's name from its child game object
                string cardName = currentDeckSlots[i].GetChild(0).name;
                cardNames.Add(cardName);
            }
            else
            {
                // If the slot is empty, add "Empty" to signify no card
                cardNames.Add("Empty");
            }
        }

        // Convert the list of card names to a comma-separated string
        string deckData = string.Join(",", cardNames);

        // Save the deck data to PlayerPrefs
        PlayerPrefs.SetString("CurrentDeck", deckData);
        PlayerPrefs.Save(); // Save changes immediately

        // Debug log to verify if the deck data is saved
        Debug.Log("Current deck data saved: " + deckData);
    }

    public void PrintPlayerPrefsData()
    {
        Debug.Log("CurrentDeck PlayerPrefs Data: " + PlayerPrefs.GetString("CurrentDeck", "No data found"));
    }
}
