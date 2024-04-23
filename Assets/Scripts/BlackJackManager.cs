using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlackJackManager : MonoBehaviour
{
    [Header("UI and Effects")]
    public TextMeshProUGUI textNotifications;
    public TextMeshProUGUI opponentCardText;
    public TextMeshProUGUI playerCardText;
    public TextMeshProUGUI playerLifeText;
    public TextMeshProUGUI opponentLifeText;

    public GameObject winscreen;
    public GameObject hitOrStay;
    public TextMeshProUGUI fateText;

    public AudioSource cardDrawAudioSource; // Reference to the AudioSource component
    public AudioSource cardPlayAudioSource; // Reference to the AudioSource component

    [Header("Card and Hand Information")]
    public List<Card2> availableCards;
    public Transform[] playerCardSlots;
    public Transform[] opponentCardSlots;
    public bool[] playerAvailableCardSlots;
    public bool[] opponentAvailableCardSlots;
    public List<Card2> playerHand = new List<Card2>();
    public List<Card2> opponentHand = new List<Card2>();

    [Header("Battle Information")]
    private bool gameHasBegun = false;
    private bool playerCardsDrawn = false;
    private bool opponentCardsDrawn = false;
    public int playerTotal = 0;
    public int opponentTotal = 0;
    public bool playerTurn;
    public bool opponentTurn;
    public bool playerDone;
    public bool opponentDone;

    [Header("Other Settings")]
    public int playerLives = 5;
    public int opponentLives = 5;
    public int playerWins; //0 is nothing, 1 is win, 2 is lose
    public bool gameEnded = false;

    Card2 card;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //Clear Text Notifications
        textNotifications.text = "";
        opponentCardText.text = "";
        playerCardText.text = "";
        winscreen.SetActive(false);
        hitOrStay.SetActive(false);
        fateText.text = "";

        // Initialize player's hand
        foreach (Card2 card in availableCards)
        {
            card.faceDown = false;
            card.gameObject.SetActive(false);
        }
        playerHand.Clear();
        opponentHand.Clear();

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

        if (gameHasBegun == false)
        {
            if (playerCardsDrawn == true && opponentCardsDrawn == true)
            {
                StartCoroutine(StartBattle());
                gameHasBegun = true;
            }
        }
        if (gameEnded == false)
        {
            foreach (Card2 card in opponentHand)
            {
                card.faceDown = true;
            }
        }
        if (gameEnded == true)
        {
            foreach (Card2 card in opponentHand)
            {
                card.faceDown = true;
            }
        }
    }

    public void ResetGame()
    {
        gameEnded = false;
        gameHasBegun = false;
        Debug.Log("Resetting");
        // Reset game state
        playerLives = 5;
        opponentLives = 5;
        playerWins = 0;

        // Clear any existing notifications
        textNotifications.text = "";

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


        // Clear opponent's hand
        foreach (Card2 card in opponentHand)
        {
            card.gameObject.SetActive(false);
        }
        opponentHand.Clear();

        // Disable all cards in player deck
        foreach (Card2 card in playerHand)
        {
            card.gameObject.SetActive(false);
        }

        playerHand.Clear();
        opponentHand.Clear();

        for (int i = 0; i < playerAvailableCardSlots.Length; i++)
        {
            playerAvailableCardSlots[i] = true;
        }
        for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
        {
            opponentAvailableCardSlots[i] = true;
        }

        Start();
    }

    public IEnumerator DrawPlayerCards()
    {
        // Check if the player has already drawn cards
        if (playerHand.Count >= 2)
        {
            // Player has already drawn cards, draw only one card
            DrawSingleCardForPlayer();
        }
        else
        {
            // Player has not drawn cards yet, draw two cards
            StartCoroutine(DrawTwoCardsForPlayer());
        }

        yield return null;
    }

    public IEnumerator DrawTwoCardsForPlayer()
    {
        List<int> availableSlots = new List<int>();

        // Find all available card slots
        for (int i = 0; i < 2; i++)
        {
            if (playerAvailableCardSlots[i])
            {
                availableSlots.Add(i);
            }
        }

        // Draw a random card for each of the first 2 slots
        foreach (int slotIndex in availableSlots)
        {
            if (availableCards.Count > 0)
            {
                int randomIndex = Random.Range(0, availableCards.Count);
                Card2 randomCard = availableCards[randomIndex];

                // Ensure the card is not already drawn
                if (!randomCard.gameObject.activeSelf)
                {
                    DrawCard(randomCard, slotIndex);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                    /*for (int i = 0; i < playerHand.Count; i++)
                    {
                        playerHand[i].SetActive(false);
                    }*/
                    playerHand.Clear();
                    StartCoroutine(DrawTwoCardsForPlayer());
                }
            }
        }
        playerCardsDrawn = true;
        //StartCoroutine(StartBattle());
    }

    public void DrawSingleCardForPlayer()
    {
        int availableSlot = -1;

        // Find an available card slot
        for (int i = 0; i < playerAvailableCardSlots.Length; i++)
        {
            if (playerAvailableCardSlots[i])
            {
                availableSlot = i;
                break;
            }
        }

        // Draw a random card for the available slot
        if (availableSlot != -1 && availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            Card2 randomCard = availableCards[randomIndex];

            // Ensure the card is not already drawn
            if (!randomCard.gameObject.activeSelf)
            {
                DrawCard(randomCard, availableSlot);
            }
            else
            {
                Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                DrawSingleCardForPlayer();
            }
        }
        //StartCoroutine(StartBattle());
    }

    private void DrawCard(Card2 card, int slotIndex)
    {
        card.gameObject.SetActive(true);
        card.handIndex = slotIndex;
        card.transform.position = playerCardSlots[slotIndex].position;
        playerAvailableCardSlots[slotIndex] = false;

        // Add the drawn card to the player's hand
        playerHand.Add(card);
        playerTurn = false;
    }

    public IEnumerator DrawOpponentCards()
    {
        // Check if the opponent has already drawn cards
        if (opponentHand.Count >= 2)
        {
            // Opponent has already drawn cards, draw only one card
            DrawSingleCardForOpponent();
        }
        else
        {
            // Opponent has not drawn cards yet, draw two cards
            StartCoroutine(DrawTwoCardsForOpponent());
        }

        yield return null;
    }

    public IEnumerator DrawTwoCardsForOpponent()
    {
        List<int> availableSlots = new List<int>();

        // Find all available card slots
        for (int i = 0; i < 2; i++)
        {
            if (opponentAvailableCardSlots[i])
            {
                availableSlots.Add(i);
            }
        }

        // Draw a random card for each of the first 2 slots
        foreach (int slotIndex in availableSlots)
        {
            if (availableCards.Count > 0)
            {
                int randomIndex = Random.Range(0, availableCards.Count);
                Card2 randomCard = availableCards[randomIndex];

                // Ensure the card is not already drawn
                if (!randomCard.gameObject.activeSelf)
                {
                    DrawOpponentCard(randomCard, slotIndex);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                    opponentHand.Clear();
                    StartCoroutine(DrawTwoCardsForOpponent());
                }
            }
        }
        opponentCardsDrawn = true;
        //StartCoroutine(StartBattle());
    }

    public void DrawSingleCardForOpponent()
    {
        int availableSlot = -1;

        // Find an available card slot
        for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
        {
            if (opponentAvailableCardSlots[i])
            {
                availableSlot = i;
                break;
            }
        }

        // Draw a random card for the available slot
        if (availableSlot != -1 && availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            Card2 randomCard = availableCards[randomIndex];

            // Ensure the card is not already drawn
            if (!randomCard.gameObject.activeSelf)
            {
                DrawOpponentCard(randomCard, availableSlot);
            }
            else
            {
                Debug.LogWarning("Trying to draw the same card again: " + randomCard.name);
                DrawSingleCardForOpponent();
            }
        }
        //StartCoroutine(StartBattle());
    }

    private void DrawOpponentCard(Card2 card, int slotIndex)
    {
        card.gameObject.SetActive(true);
        card.handIndex = slotIndex;
        card.transform.position = opponentCardSlots[slotIndex].position;
        opponentAvailableCardSlots[slotIndex] = false;

        // Add the drawn card to the opponent's hand
        opponentHand.Add(card);
        opponentTurn = false;
    }

    public IEnumerator StartBattle()
    {
        if (playerDone == true && opponentDone == true && gameEnded == false)
        {
            gameEnded = true;
            opponentCardText.text = opponentTotal.ToString();
            yield return new WaitForSeconds(1f);
            CheckGameEnd();
            yield return null;
        }
        if (gameEnded == false)
        {
            textNotifications.text = "";
            opponentCardText.text = "";
            playerCardText.text = "";

            playerTotal = 0;
            opponentTotal = 0;

            // Sum up the card values in the player's hand
            foreach (Card2 card in playerHand)
            {
                playerTotal += card.cardValue;
            }
            playerCardText.text = playerTotal.ToString();

            // Sum up the card values in the opponent's hand
            foreach (Card2 card in opponentHand)
            {
                opponentTotal += card.cardValue;
            }
            //opponentCardText.text = opponentTotal.ToString();
        }
        if (playerTurn == false && playerDone == true && opponentDone == true && gameEnded == false)
        {
            gameEnded = true;
            // Set all cards face up
            foreach (Card2 card in availableCards)
            {
                card.faceDown = false;
            }

            opponentCardText.text = opponentTotal.ToString();
            yield return new WaitForSeconds(1f);
            CheckGameEnd();
        }
        yield return new WaitForSeconds(1f);
        if (gameEnded == false)
        {
            //Opponent Turn
            opponentTurn = true;
            if (opponentTurn = true)
            {
                int randomNumber = Random.Range(1, 10);
                //Enemy Logic for Hitting or Staying
                if (opponentDone == false)
                    {
                    if (opponentTotal <= 10 && randomNumber < 9)
                        DrawSingleCardForOpponent();
                    else if (opponentTotal <= 14 && randomNumber < 7)
                        DrawSingleCardForOpponent();
                    else if (opponentTotal <= 15 && randomNumber < 6)
                        DrawSingleCardForOpponent();
                    else if (opponentTotal <= 16 && randomNumber < 5)
                        DrawSingleCardForOpponent();
                    else if (opponentTotal <= 17 && randomNumber < 4)
                        DrawSingleCardForOpponent();
                    else if (opponentTotal <= 18 && randomNumber < 3)
                        DrawSingleCardForOpponent();
                    else if (opponentTotal <= 19 && randomNumber < 2)
                        DrawSingleCardForOpponent();
                    else
                    {
                        Debug.Log("Do I get triggered?");
                        opponentDone = true;
                    }
                }
                if (playerDone == true && opponentDone == true)
                    StartCoroutine(StartBattle());
            }
            
            yield return new WaitForSeconds(1f);
            
            if (playerDone == false)
            {
                //Player Turn
                playerTurn = true;
                if (playerTurn == true)
                {
                    hitOrStay.SetActive(true);
                }
                else
                {
                    hitOrStay.SetActive(false);
                }
            }
            else if (playerDone == true)
                StartCoroutine(StartBattle());
        }
        yield return null;   
    }

    public void CheckGameEnd()
    {
        if (playerTotal < opponentTotal && opponentTotal <= 21)
        {
            textNotifications.text = "Player loses a life!";
            playerLives--;
        }
        else if (opponentTotal < playerTotal && playerTotal <= 21)
        {
            textNotifications.text = "Opponent loses a life!";
            opponentLives--;
        }
        else if (playerTotal == opponentTotal)
        {
            textNotifications.text = "Tie!";
        }
        else if (playerTotal > 21 && opponentTotal <= 21)
        {
            textNotifications.text = "Player loses a life!";
            playerLives--;
        }
        else if (opponentTotal > 21 && playerTotal <= 21)
        {
            textNotifications.text = "Opponent loses a life!";
            opponentLives--;
        }
        else if (playerTotal > 21 && opponentTotal > 21)
        {
            textNotifications.text = "Tie!";
        }
        if (playerLives <= 0)
        {
            GameOver(false); // Opponent wins
        }
        else if (opponentLives <= 0)
        {
            GameOver(true); // Player wins
        }
        else if (playerLives >= 0 && opponentLives >= 0)
        { 
            // Clear opponent's hand
            foreach (Card2 card in opponentHand)
            {
                card.gameObject.SetActive(false);
            }
            opponentHand.Clear();

            // Disable all cards in player deck
            foreach (Card2 card in playerHand)
            {
                card.gameObject.SetActive(false);
            }

            playerHand.Clear();
            opponentHand.Clear();

            playerTurn = false;
            opponentTurn = false;
            playerDone = false;
            opponentDone = false;

            for (int i = 0; i < playerAvailableCardSlots.Length; i++)
            {
                playerAvailableCardSlots[i] = true;
            }
            for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
            {
                opponentAvailableCardSlots[i] = true;
            }

            gameEnded = false;
            Start();
        }
        //StartCoroutine(DrawPlayerCards());
        //StartCoroutine(DrawOpponentCards());
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

    public void Hit()
    {
        hitOrStay.SetActive(false);
        StartCoroutine(DrawPlayerCards());
        StartCoroutine(StartBattle());

    }

    public void Stay()
    {
        hitOrStay.SetActive(false);
        playerDone = true;
        playerTurn = false;
        StartCoroutine(StartBattle());
    }
}
