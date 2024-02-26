using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    public Transform[] availableDeckSlots; // Array of available deck slots transformation points
    public Transform[] currentDeckSlots;   // Array of current deck slots transformation points

    public GameObject[] holyCardPrefabs;   // Array of prefabs for Holy cards
    public GameObject[] terrestrialCardPrefabs; // Array of prefabs for Terrestrial cards
    public GameObject[] demonicCardPrefabs; // Array of prefabs for Demonic cards

    void Start()
    {
        SpawnAvailableCards();
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
}
