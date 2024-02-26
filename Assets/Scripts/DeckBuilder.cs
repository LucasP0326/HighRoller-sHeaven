using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
    public Transform[] availableDeckSlots; // Array of available deck slots transformation points
    public Transform[] currentDeckSlots;   // Array of current deck slots transformation points

    public GameObject[] holyCardPrefabs;   // Array of prefabs for Holy cards
    public GameObject[] terrestrialCardPrefabs; // Array of prefabs for Terrestrial cards
    public GameObject[] demonicCardPrefabs; // Array of prefabs for Demonic cards

    public Button saveButton; // Reference to the save button
    public Button transitionButton; // Reference to the button that triggers the scene transition
    public SceneAsset sceneToLoad; // Reference to the scene to load

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

        SpawnAvailableCards();
    }

    void TransitionToScene()
    {
        // Check if the sceneToLoad reference is not null
        if (sceneToLoad != null)
        {
            // Load the scene by its name
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad.name);
        }
        else
        {
            Debug.LogWarning("Scene to load is not assigned.");
        }
    }


    void SpawnAvailableCards()
    {
        for (int i = 0; i < availableDeckSlots.Length; i++)
        {
            // Determine which type of card to spawn
            int cardTypeIndex = Random.Range(0, 3); // Assuming 3 types of cards (Holy, Terrestrial, Demonic)
            GameObject[] cardPrefabs = null;

            // Select the appropriate array of prefabs based on the card type
            switch (cardTypeIndex)
            {
                case 0:
                    cardPrefabs = holyCardPrefabs;
                    break;
                case 1:
                    cardPrefabs = terrestrialCardPrefabs;
                    break;
                case 2:
                    cardPrefabs = demonicCardPrefabs;
                    break;
            }

            // Select a random prefab from the array
            int prefabIndex = Random.Range(0, cardPrefabs.Length);
            GameObject cardPrefab = cardPrefabs[prefabIndex];

            // Instantiate the selected prefab at the current available deck slot
            Instantiate(cardPrefab, availableDeckSlots[i].position, Quaternion.identity, availableDeckSlots[i]);
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

    // Method to save the current deck data
    public void SaveCurrentDeck()
    {
        // Check if the current deck has at least 10 cards
        if (CountCardsInCurrentDeck() >= 10)
        {
            string deckData = "";
            for (int i = 0; i < currentDeckSlots.Length; i++)
            {
                if (currentDeckSlots[i].childCount > 0)
                {
                    GameObject cardObject = currentDeckSlots[i].GetChild(0).gameObject;
                    string cardName = cardObject.name; // Assuming card name represents its data
                    deckData += cardName + ",";
                }
                else
                {
                    deckData += "Empty,";
                }
            }
            PlayerPrefs.SetString("CurrentDeck", deckData);
            PlayerPrefs.Save();
            Debug.Log("Current deck saved successfully.");
        }
        else
        {
            Debug.Log("Cannot save deck. Current deck must contain at least 10 cards.");
        }
    }

    // Method to count the number of cards in the current deck
    private int CountCardsInCurrentDeck()
    {
        int count = 0;
        foreach (Transform slot in currentDeckSlots)
        {
            if (slot.childCount > 0)
            {
                count++;
            }
        }
        return count;
    }
}
