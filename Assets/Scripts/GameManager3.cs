using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager3 : MonoBehaviour
{
    // Singleton instance
    public static GameManager3 Instance;

    // Reference to the TextMeshPro text objects for displaying messages
    public TextMeshProUGUI textNotifications;
    public TextMeshProUGUI opponentCardText;
    public TextMeshProUGUI playerCardText;

    public GameObject winscreen;
    public TextMeshProUGUI fateText;

    public AudioSource cardDrawAudioSource; // Reference to the AudioSource component
    public AudioSource cardPlayAudioSource; // Reference to the AudioSource component

    // Define card types (ensure it's consistent with the definition in GameManager2)
    public enum CardType { Holy, Terrestrial, Demonic }

    // Player's deck and related variables
    public List<Card3> playerDeck;
    public Transform[] playerCardSlots;
    public Transform playerBattlePosition;
    public bool[] playerAvailableCardSlots;

    // Opponent's deck and related variables
    public List<Card3> opponentDeck;
    public Transform[] opponentCardSlots;
    public Transform opponentBattlePosition;
    public bool[] opponentAvailableCardSlots;

    // Opponent's hand
    public List<Card3> opponentHand = new List<Card3>(); // New list to store opponent's hand

    // Original decks to replenish at game reset
    public List<Card3> startingPlayerDeck = new List<Card3>(); //original starting decks that can be changed in deck editor
    public List<Card3> startingOpponentDeck = new List<Card3>(); //original starting decks that can be changed in deck editor

    public int playerWinsCount = 0;
    public int opponentWinsCount = 0;
    public int gamesToWin = 3;

    public int playerWins; //0 is nothing, 1 is win, 2 is lose

    Card3 card;


    void Awake()
    {
        // Ensure there's only one instance of GameManager3
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set player deck to be whatever is marked as starting deck
        playerDeck.Clear();
        foreach (Card3 card in startingPlayerDeck)
        {
            playerDeck.Add(card);
        }

        // Set opponent deck to be whatever is marked as starting deck
        opponentDeck.Clear();
        foreach (Card3 card in startingOpponentDeck)
        {
            opponentDeck.Add(card);
        }

        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";
        winscreen.SetActive(false);
        fateText.text = "";

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
        foreach (Card3 card in startingPlayerDeck)
        {
            playerDeck.Add(card);
        }

        // Replenish opponent's deck to its original state
        opponentDeck.Clear();
        foreach (Card3 card in startingOpponentDeck)
        {
            opponentDeck.Add(card);
        }

        // Clear opponent's hand
        opponentHand.Clear();

        // Disable all cards in player deck
        foreach (Card3 card in playerDeck)
        {
            card.gameObject.SetActive(false);
        }

        // Disable all cards in opponent deck
        foreach (Card3 card in opponentDeck)
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
                Card3 randomCard = playerDeck[randomIndex];
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
                Card3 randomCard = opponentDeck[randomIndex];
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

    public void PlayerDiscard(Card3 card)
    {
        // Add the card to the bottom of the player's deck (no discard pile)
        playerDeck.Add(card);

        // Deactivate the card
        card.gameObject.SetActive(false);

        // Make the card slot available again
        playerAvailableCardSlots[card.handIndex] = true;
    }

    public void OpponentDiscard(Card3 card)
    {
        // Add the card to the bottom of the opponent's deck (no discard pile)
        opponentDeck.Add(card);

        // Deactivate the card
        card.gameObject.SetActive(false);

        // Make the card slot available again
        opponentAvailableCardSlots[card.handIndex] = true;
    }

    public void PlayerPlayCard(Card3 selectedCard)
    {
        //Flavor
        cardPlayAudioSource.Play();

        // Find opponent's card
        Card3 opponentCard = FindOpponentCard();
        cardPlayAudioSource.Play();

        // Start the battle
        StartCoroutine(StartBattle(selectedCard, opponentCard));
    }

    private Card3 FindOpponentCard()
    {
        // If opponent has no cards in hand, return null
        if (opponentHand.Count == 0)
        {
            return null;
        }

        // Select a random card from the opponent's hand
        int randomIndex = UnityEngine.Random.Range(0, opponentHand.Count);
        Card3 opponentCard = opponentHand[randomIndex];

        return opponentCard;
    }

    public IEnumerator StartBattle(Card3 playerCard, Card3 opponentCard)
    {
        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";

        //Move Cards to battle spot
        playerCard.transform.position = playerBattlePosition.position;
        opponentCard.transform.position = opponentBattlePosition.position;
        yield return new WaitForSeconds(1f);

        // Determine card types
        Debug.Log("Getting Component for player");
        CardType playerCardType = playerCard.GetComponent<Card3>().cardType;
        Debug.Log("Getting Component for Ai");
        CardType opponentCardType = opponentCard.GetComponent<Card3>().cardType;

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

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DrawPlayerCards());
        StartCoroutine(DrawOpponentCards());
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
        Debug.Log("Time delay should be there");
    }

    public void StartBattle()
    {
        // Add logic here to start the battle
        Debug.Log("Battle started!");
    }
}
