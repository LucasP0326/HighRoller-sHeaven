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

    // Start is called before the first frame update
    void Start()
    {
        SpawnAvailableCards();
    }

    void SpawnAvailableCards()
    {
        // Spawn available cards in the available deck slots
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
}
