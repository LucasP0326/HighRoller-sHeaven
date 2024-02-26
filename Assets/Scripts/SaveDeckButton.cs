using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveDeckButton : MonoBehaviour
{
    public GameManager2 gameManager; // Reference to the GameManager script

    private void Start()
    {
        // Find the GameManager script in the scene
        gameManager = FindObjectOfType<GameManager2>();

        // Add an event listener to the button's onClick event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SavePlayerDeck);
    }

    // Method to save the player's current deck
    public void SavePlayerDeck()
    {
        // Call a method in the GameManager to save the player's deck
        gameManager.SavePlayerDeck();
    }
}
