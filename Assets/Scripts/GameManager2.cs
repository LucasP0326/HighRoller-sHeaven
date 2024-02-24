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

    public GameObject winscreen;
    public TextMeshProUGUI fateText;

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

    // Opponent's hand
    public List<Card2> opponentHand = new List<Card2>(); // New list to store opponent's hand

    // Original decks to replenish at game reset
    public List<Card2> startingPlayerDeck = new List<Card2>(); //original starting decks that can be changed in deck editor
    public List<Card2> startingOpponentDeck = new List<Card2>(); //original starting decks that can be changed in deck editor

    public int playerWinsCount = 0;
    public int opponentWinsCount = 0;
    public int gamesToWin = 3;

    public int playerWins; //0 is nothing, 1 is win, 2 is lose

    Card2 card;

    // Start is called before the first frame update
    void Start()
    {
        // Set player deck to be whatever is marked as starting deck
        playerDeck.Clear();
        foreach (Card2 card in startingPlayerDeck)
        {
            playerDeck.Add(card);
        }

        // Set opponent deck to be whatever is marked as starting deck
        opponentDeck.Clear();
        foreach (Card2 card in startingOpponentDeck)
        {
            opponentDeck.Add(card);
        }

        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";
        winscreen.SetActive(false);
        fateText.text = "";


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

    public void ResetGame()
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

        // Clear opponent's hand
        opponentHand.Clear();

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

                    // Add the drawn card to the opponent's hand
                    opponentHand.Add(randomCard);
                }
                else
                {
                    Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                }
            }
        }
    }

    public void PlayerDiscard(Card2 card)
    {
        // Add the card to the bottom of the player's deck (no discard pile)
        playerDeck.Add(card);

        // Deactivate the card
        card.gameObject.SetActive(false);

        // Make the card slot available again
        playerAvailableCardSlots[card.handIndex] = true;
    }

    public void OpponentDiscard(Card2 card)
    {
        // Add the card to the bottom of the opponent's deck (no discard pile)
        opponentDeck.Add(card);

        // Deactivate the card
        card.gameObject.SetActive(false);

        // Make the card slot available again
        opponentAvailableCardSlots[card.handIndex] = true;
    }

    public void PlayerPlayCard(Card2 selectedCard)
    {
        // Find opponent's card
        Card2 opponentCard = FindOpponentCard();

        // Start the battle
        StartBattle(selectedCard, opponentCard);
    }

    private Card2 FindOpponentCard()
    {
        // If opponent has no cards in hand, return null
        if (opponentHand.Count == 0)
        {
            return null;
        }

        // Select a random card from the opponent's hand
        int randomIndex = UnityEngine.Random.Range(0, opponentHand.Count);
        Card2 opponentCard = opponentHand[randomIndex];

        return opponentCard;
    }

    public void StartBattle(Card2 playerCard, Card2 opponentCard)
    {
        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";

        // Determine card types
        Debug.Log("Getting Component for player");
        CardType playerCardType = playerCard.GetComponent<Card2>().cardType;
        Debug.Log("Getting Component for Ai");
        CardType opponentCardType = opponentCard.GetComponent<Card2>().cardType;

        //display who played what
        if (playerCardType == CardType.Holy)
            playerCardText.text = "Player played Holy";
        if (playerCardType == CardType.Demonic)
            playerCardText.text = "Player played Demonic";
        if (playerCardType == CardType.Terrestrial)
            playerCardText.text = "Player played Terrestrial";
        if (opponentCardType == CardType.Holy)
            opponentCardText.text = "Opponent played Holy";
        if (opponentCardType == CardType.Demonic)
            opponentCardText.text = "Opponent played Demonic";
        if (opponentCardType == CardType.Terrestrial)
            opponentCardText.text = "Opponent played Terrestrial";

        // Determine the winner based on card types
        if ((playerCardType == CardType.Holy && opponentCardType == CardType.Demonic) ||
            (playerCardType == CardType.Demonic && opponentCardType == CardType.Terrestrial) ||
            (playerCardType == CardType.Terrestrial && opponentCardType == CardType.Holy))
        {
            Debug.Log("Player Won");
            textNotifications.text = "Player wins a point!";
            playerWinsCount++;
        }
        else if (playerCardType == opponentCardType)
        {
            Debug.Log("Tie");

            textNotifications.text = "It's a tie!";
        }
        else
        {
            Debug.Log("Player lose");
            textNotifications.text = "Opponent wins a point!";
            opponentWinsCount++;
        }

        // Mark the cards as played
        playerCard.hasBeenPlayed = true;
        opponentCard.hasBeenPlayed = true;

        //Discard the cards
        PlayerDiscard(playerCard);
        OpponentDiscard(opponentCard);

        // Remove the played cards from the opponent's hand
        opponentHand.Remove(opponentCard);

        // Check if either player has won the game
        CheckGameEnd();

        DrawPlayerCards();
        DrawOpponentCards();
    }

    public void CheckGameEnd()
    {
        if (playerWinsCount >= gamesToWin)
        {
            playerWins = 1;
        }

        if (opponentWinsCount >= gamesToWin)
        {
            playerWins = 2;
        }
        // Check game end conditions
        // For example, set to true if player wins
        if (playerWins == 1)
        {
            winscreen.SetActive(true);
            Debug.Log("Player Won game end");
            fateText.text = "Player wins the game!";
        }
        else if (playerWins == 2)
        {
            winscreen.SetActive(true);
            Debug.Log("Player Lose game end");
            fateText.text = "Opponent wins the game!";
        }
    }

    public IEnumerator SimpleDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
    }

}

