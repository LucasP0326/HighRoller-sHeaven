using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    // Reference to the TextMeshPro text objects for displaying messages
    public TextMeshProUGUI textNotifications; 
    public TextMeshProUGUI opponentCardText;
    public TextMeshProUGUI playerCardText;

    // Define card types
    public enum CardType { Holy, Terrestrial, Demonic }

    // Player's deck and related variables
    public List<Card2> playerDeck;
    public Transform[] playerCardSlots;
    public bool[] playerAvailableCardSlots;

    // Opponent's deck and related variables
    public List<Card2> opponentDeck;
    public Transform[] opponentCardSlots;
    public bool[] opponentAvailableCardSlots;

    // Original decks to replenish at game reset
    public List<Card2> startingPlayerDeck = new List<Card2>();
    public List<Card2> startingOpponentDeck = new List<Card2>();

    public int playerWinsCount = 0;
    public int opponentWinsCount = 0;
    public int gamesToWin = 3;

    public int playerWins; //0 is nothing, 1 is win, 2 is lose

    Card2 card;

    // Start is called before the first frame update
    void Start()
    {
        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";

        // Shuffle player and opponent's deck and play the top 3 cards

        DrawPlayerCards();
        DrawOpponentCards();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input to reset the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    void ResetGame()
    {
        // Reset game state
        playerWinsCount = 0;
        opponentWinsCount = 0;
        playerWins = 0;

        // Reset player card slots availability
        for (int i = 0; i < playerAvailableCardSlots.Length; i++)
        {
            playerAvailableCardSlots[i] = true;
        }

        // Reset opponent card slots availability
        for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
        {
            opponentAvailableCardSlots[i] = true;
        }

        // Clear any existing notifications
        textNotifications.text = "";

        // Replenish player's deck to its original state
        playerDeck.Clear();
        foreach (Card2 card in startingPlayerDeck)
        {
            playerDeck.Add(card);
        }

        // Replenish opponent's deck to its original state
        opponentDeck.Clear();
        foreach (Card2 card in startingOpponentDeck)
        {
            opponentDeck.Add(card);
        }

        // Disable all cards in player deck
        foreach (Card2 card in playerDeck)
        {
            card.gameObject.SetActive(false);
        }

        // Disable all cards in opponent deck
        foreach (Card2 card in opponentDeck)
        {
            card.gameObject.SetActive(false);
        }

        Start();
    }
    
    public void DrawPlayerCards()
    {
        List<int> availableSlots = new List<int>();

        // Find all available card slots
        for (int i = 0; i < playerAvailableCardSlots.Length; i++)
        {
            if (playerAvailableCardSlots[i])
            {
                availableSlots.Add(i);
            }
        }

        // Debug log to check available slots
        Debug.Log("Available Player card slots: " + availableSlots.Count);

        // Draw a random card for each available slot
        foreach (int slotIndex in availableSlots)
        {
            if (playerDeck.Count > 0)
            {
                int randomIndex = Random.Range(0, playerDeck.Count);
                Card2 randomCard = playerDeck[randomIndex];
                Debug.Log("Drawing card for player: " + randomCard.name);

                // Ensure the card is not already drawn
                if (!randomCard.gameObject.activeSelf)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = slotIndex;
                    randomCard.transform.position = playerCardSlots[slotIndex].position;
                    randomCard.hasBeenPlayed = false;
                    playerDeck.Remove(randomCard);
                    playerAvailableCardSlots[slotIndex] = false;
                }
                else
                {
                    Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                }
            }
        }
    }

    public void DrawOpponentCards()
    {
        List<int> availableSlots = new List<int>();

        // Find all available card slots
        for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
        {
            if (opponentAvailableCardSlots[i])
            {
                availableSlots.Add(i);
            }
        }

        // Debug log to check available slots
        Debug.Log("Available Opponent card slots: " + availableSlots.Count);

        // Draw a random card for each available slot
        foreach (int slotIndex in availableSlots)
        {
            if (opponentDeck.Count > 0)
            {
                int randomIndex = Random.Range(0, opponentDeck.Count);
                Card2 randomCard = opponentDeck[randomIndex];
                Debug.Log("Drawing card for opponent: " + randomCard.name);

                // Ensure the card is not already drawn
                if (!randomCard.gameObject.activeSelf)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = slotIndex;
                    randomCard.transform.position = opponentCardSlots[slotIndex].position;
                    randomCard.hasBeenPlayed = false;
                    opponentDeck.Remove(randomCard);
                    opponentAvailableCardSlots[slotIndex] = false;
                }
                else
                {
                    Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                }
            }
        }
    }

    public IEnumerator SimpleDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
    }

}

