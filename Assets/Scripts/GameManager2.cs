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
    public TextMeshProUGUI playerLifeText;
    public TextMeshProUGUI opponentLifeText;

    public GameObject winscreen;
    public TextMeshProUGUI fateText;

    public AudioSource cardDrawAudioSource; // Reference to the AudioSource component
    public AudioSource cardPlayAudioSource; // Reference to the AudioSource component

    // Define card types
    public enum CardType { Holy, Terrestrial, Demonic }

    // Player's deck and related variables
    public List<Card2> playerDeck;
    public Transform[] playerCardSlots;
    public Transform playerBattlePosition;
    public bool[] playerAvailableCardSlots;

    // Opponent's deck and related variables
    public List<Card2> opponentDeck;
    public Transform[] opponentCardSlots;
    public Transform opponentBattlePosition;
    public bool[] opponentAvailableCardSlots;

    // Opponent's hand
    public List<Card2> opponentHand = new List<Card2>(); // New list to store opponent's hand

    // Original decks to replenish at game reset
    public List<Card2> startingPlayerDeck = new List<Card2>(); //original starting decks that can be changed in deck editor
    public List<Card2> startingOpponentDeck = new List<Card2>(); //original starting decks that can be changed in deck editor

    public int playerLives = 5;
    public int opponentLives = 5;

    public int playerWins; //0 is nothing, 1 is win, 2 is lose

    Card2 card;
    public DeckData deckData; //Reference to Deck Data

    // Start is called before the first frame update
    void Start()
    {
        // Set player deck to be whatever is marked as starting deck
        playerDeck.Clear();
        if (deckData != null && DeckData.playerDeck.Count > 0)
        {
            LoadSavedDeck();
        }
        foreach (Card2 card in DeckData.playerDeck)
        {
            Debug.Log(card.name); // Assuming card has a "name" field for identification
        }
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
        //UpdateLifeUI();

        // Shuffle player and opponent's deck and play the top 3 cards

        StartCoroutine(DrawPlayerCards());
        StartCoroutine(DrawOpponentCards());
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input to reset the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        playerLifeText.text = "Player Lives: " + playerLives.ToString();
        opponentLifeText.text = "Opponent Lives: " + opponentLives.ToString();
    }

    void LoadSavedDeck()
    {
        if (deckData != null)
        {
            Debug.Log("Loading saved player deck from DeckData.");

            // Load the saved player deck from the DeckData instance
            startingPlayerDeck = new List<Card2>(DeckData.playerDeck);

            // Log the number of cards loaded for debugging
            Debug.Log("Loaded " + startingPlayerDeck.Count + " cards from DeckData.");

            // Optionally, log the details of each card loaded
            foreach (Card2 card in startingPlayerDeck)
            {
                Debug.Log("Loaded card: " + card.name);
            }
        }
        else
        {
            Debug.LogError("DeckData reference is null. Make sure it is assigned in the inspector.");
        }
    }

    public void SavePlayerDeck()
    {
        // Save the player's current deck state
        // You can implement saving functionality here, such as saving the card identifiers or other relevant data
        Debug.Log("Player's current deck has been saved.");
    }
    public void ResetGame()
    {
        // Reset game state
        playerLives = 5;
        opponentLives = 5;
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

    public IEnumerator DrawPlayerCards()
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
                    cardDrawAudioSource.Play();
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                }
            }
        }
    }

    public IEnumerator DrawOpponentCards()
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
                    cardDrawAudioSource.Play();
                    yield return new WaitForSeconds(0.5f);
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
        //Flavor
        cardPlayAudioSource.Play();

        // Find opponent's card
        Card2 opponentCard = FindOpponentCard();
        cardPlayAudioSource.Play();

        // Start the battle
        StartCoroutine(StartBattle(selectedCard, opponentCard));
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

    public IEnumerator StartBattle(Card2 playerCard, Card2 opponentCard)
    {
        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";

        // Move Cards to battle spot
        playerCard.transform.position = playerBattlePosition.position;
        opponentCard.transform.position = opponentBattlePosition.position;
        yield return new WaitForSeconds(1f);

        // Determine card types and values
        CardType playerCardType = playerCard.GetComponent<Card2>().cardType;
        int playerCardValue = playerCard.cardValue;
        CardType opponentCardType = opponentCard.GetComponent<Card2>().cardType;
        int opponentCardValue = opponentCard.cardValue;

        // Display who played what
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
        if (playerCardType == opponentCardType)
        {
            if (playerCardValue > opponentCardValue)
            {
                Debug.Log("Player Won");
                textNotifications.text = "Player wins a point!";
                // No changes to life count if player wins
            }
            else if (playerCardValue < opponentCardValue)
            {
                Debug.Log("Player lose");
                textNotifications.text = "Opponent wins a point!";
                playerLives--; // Decrease player's life count
            }
            else
            {
                textNotifications.text = "It's a tie!";
            }
        }
        else
        {
            if ((playerCardType == CardType.Holy && opponentCardType == CardType.Demonic) ||
                (playerCardType == CardType.Demonic && opponentCardType == CardType.Terrestrial) ||
                (playerCardType == CardType.Terrestrial && opponentCardType == CardType.Holy))
            {
                Debug.Log("Player Won");
                textNotifications.text = "Opponent loses a life!";
                opponentLives--; // Decrease opponent's life count
            }
            else
            {
                Debug.Log("Player lose");
                textNotifications.text = "Player loses a life!";
                playerLives--; // Decrease player's life count
            }
        }

        // Mark the cards as played
        playerCard.hasBeenPlayed = true;
        opponentCard.hasBeenPlayed = true;

        // Discard the cards
        PlayerDiscard(playerCard);
        OpponentDiscard(opponentCard);

        // Remove the played cards from the opponent's hand
        opponentHand.Remove(opponentCard);

        // Check if either player has won the game
        CheckGameEnd();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DrawPlayerCards());
        StartCoroutine(DrawOpponentCards());
    }

    public void CheckGameEnd()
    {
        if (playerLives <= 0)
        {
            GameOver(false); // Opponent wins
        }
        else if (opponentLives <= 0)
        {
            GameOver(true); // Player wins
        }
    }

    void GameOver(bool playerWins)
    {
        // Display appropriate win message based on the result
        if (playerWins)
        {
            winscreen.SetActive(true);
            Debug.Log("Player Wins the game!");
            fateText.text = "Player wins the game!";
        }
        else
        {
            winscreen.SetActive(true);
            Debug.Log("Opponent Wins the game!");
            fateText.text = "Opponent wins the game!";
        }
    }

    public IEnumerator SimpleDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        Debug.Log("Time delay should be there");
    }

    /*void UpdateLifeUI()
    {
        // Update UI to display remaining life count for both players
        playerLifeText.text = "Player Lives: " + playerLives.ToString();
        opponentLifeText.text = "Opponent Lives: " + opponentLives.ToString();
    }*/
}

