using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{
    public Transform[] availableDeckSlots; // Array of available deck slots transformation points
    public Transform[] currentDeckSlots;   // Array of current deck slots transformation points

    public Button saveButton; // Reference to the save button
    public Button transitionButton; // Reference to the button that triggers the scene transition
    public string MechanicsTesting2; // Name of the scene to load
    public Button resetButton; // Reference to the reset button

    public GameObject[] specificAvailableCards; // Specific card game objects to assign to available deck slots

    void Start()
    {
        // Assuming you have already set up the save button in the Unity Editor
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveCurrentDeck); // Attach the SaveCurrentDeck method to the button click event
        }
        
        // Attach the button click listener
        if (transitionButton != null)
        {
            transitionButton.onClick.AddListener(TransitionToScene);
        }

        // Attach the ResetScene method to the reset button click event
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetScene);
        }

        AssignSpecificAvailableCards();

        PrintPlayerPrefsData(); // Call the method to print PlayerPrefs data
    }

    void TransitionToScene()
    {
        // Check if the sceneToLoad reference is not null
        if (!string.IsNullOrEmpty(MechanicsTesting2))
        {
            // Load the scene by its name
            UnityEngine.SceneManagement.SceneManager.LoadScene(MechanicsTesting2);
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

    // Method to reset the scene
    void ResetScene()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
