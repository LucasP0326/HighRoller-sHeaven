using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Reference to the TextMeshPro text objects for displaying messages
    public TextMeshProUGUI playerWinsMessage;
    public TextMeshProUGUI opponentWinsMessage;
    public TextMeshProUGUI playerPointMessage;
    public TextMeshProUGUI opponentPointMessage;

    // Define card types
    public enum CardType { Holy, Terrestrial, Demonic }

    // Player's deck and related variables
    public List<Card> playerDeck;
    public Transform[] playerCardSlots;
    public bool[] playerAvailableCardSlots;

    // Opponent's deck and related variables
    public List<Card> opponentDeck;
    public Transform[] opponentCardSlots;
    public bool[] opponentAvailableCardSlots;

    public int playerWinsCount = 0;
    public int opponentWinsCount = 0;
    public int gamesToWin = 3;

    public void Start()
    {
        // Shuffle player's deck and play the top 3 cards
        ShuffleDeck(playerDeck);
        for (int i = 0; i < Mathf.Min(playerDeck.Count, 3); i++)
        {
            PlayCard(playerDeck[i], i, true);
            OpponentDrawCard();
        }
    }

    public int GetHandIndex(Card card)
    {
        // Find the index of the card in the player's deck
        return playerDeck.IndexOf(card);
    }

    public Card DrawRandomCardFromDeck(List<Card> deck)
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("Attempted to draw a card from an empty deck.");
            return null;
        }

        int randomIndex = Random.Range(0, deck.Count);
        Card drawnCard = deck[randomIndex];
        deck.RemoveAt(randomIndex);
        return drawnCard;
    }

    public void PlayerDrawCard()
    {
        if (playerDeck.Count >= 1)
        {
            Card randomCard = playerDeck[Random.Range(0, playerDeck.Count)];
            for (int i = 0; i < playerAvailableCardSlots.Length; i++)
            {
                if (playerAvailableCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = i;
                    randomCard.transform.position = playerCardSlots[i].position;
                    randomCard.hasBeenPlayed = false;
                    playerDeck.Remove(randomCard); // Remove the drawn card from the deck
                    playerAvailableCardSlots[i] = false;

                    // Log the current deck count to the console
                    Debug.Log("Player's Deck Count: " + playerDeck.Count);

                    return;
                }
            }
        }
    }

    public void OpponentDrawCard()
    {
        if (opponentDeck.Count >= 1)
        {
            int randomIndex = Random.Range(0, opponentDeck.Count);
            Card randomCard = opponentDeck[randomIndex];
            for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
            {
                if (opponentAvailableCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = -1; // Placeholder value for opponent
                    randomCard.transform.position = opponentCardSlots[i].position;
                    randomCard.hasBeenPlayed = false;
                    opponentDeck.RemoveAt(randomIndex); // Remove the drawn card from the deck
                    opponentAvailableCardSlots[i] = false;
                    return;
                }
            }
        }
    }

    //public void PlayerShuffle()
    //{
        // Shuffling is not needed since cards go back to the bottom of the deck when used
   // }

   // public void OpponentShuffle()
   // {
        // Shuffling is not needed since cards go back to the bottom of the deck when used
   // }

    public void PlayerDiscard(Card card)
    {
        // Add the card to the bottom of the player's deck (no discard pile)
        playerDeck.Add(card);
    }

    public void OpponentDiscard(Card card)
    {
        // Add the card to the bottom of the opponent's deck (no discard pile)
        opponentDeck.Add(card);
    }

    public void ShuffleDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void PlayCard(Card card, int index, bool isPlayer)
    {
        if (isPlayer)
        {
            if (index < playerCardSlots.Length)
            {
                card.gameObject.SetActive(true);
                card.handIndex = index;
                card.transform.position = playerCardSlots[index].position;
                card.hasBeenPlayed = false;
                playerAvailableCardSlots[index] = false;
            }
        }
        else
        {
            if (index < opponentCardSlots.Length)
            {
                card.gameObject.SetActive(true);
                card.handIndex = -1; // Placeholder value for opponent
                card.transform.position = opponentCardSlots[index].position;
                card.hasBeenPlayed = false;
                opponentAvailableCardSlots[index] = false;
            }
        }
    }

    public void StartBattle(Card playerCard, Card opponentCard)
    {
        // Determine card types
        Debug.Log("Getting Component for player");
        CardType playerCardType = playerCard.GetComponent<Card>().cardType;
        Debug.Log("Getting Component for Ai");
        CardType opponentCardType = opponentCard.GetComponent<Card>().cardType;

        // Determine the winner based on card types
        if ((playerCardType == CardType.Holy && opponentCardType == CardType.Demonic) ||
            (playerCardType == CardType.Demonic && opponentCardType == CardType.Terrestrial) ||
            (playerCardType == CardType.Terrestrial && opponentCardType == CardType.Holy))
        {
            Debug.Log("Player Won");
            playerPointMessage.text = "Player wins a point!";
        }
        else if (playerCardType == opponentCardType)
        {
            Debug.Log("Tie");

            playerPointMessage.text = "It's a tie!";
        }
        else
        {
            Debug.Log("Player lose");
            opponentPointMessage.text = "Opponent wins a point!";
        }

        // Check if either player has won the game
        CheckGameEnd();
    }

    // CheckGameEnd method modified to display final win/lose messages permanently
    public void CheckGameEnd()
    {
        // Check game end conditions
        bool playerWins = true; // For example, set to true if player wins
        if (playerWins)
        {
            Debug.Log("Player Won game end");
            playerWinsMessage.text = "Player wins the game!";
        }
        else
        {
            Debug.Log("Player Lose game end");
            opponentWinsMessage.text = "Opponent wins the game!";
        }
    }
}