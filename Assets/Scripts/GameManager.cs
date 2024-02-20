using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Define card types
    public enum CardType { Holy, Terrestrial, Demonic }

    // Player's deck and related variables
    public List<Card> playerDeck;
    public Transform[] playerCardSlots;
    public bool[] playerAvailableCardSlots;
    public Transform playerBattleSlot; // Player's battle slot

    // Opponent's deck and related variables
    public List<Card> opponentDeck;
    public Transform[] opponentCardSlots;
    public bool[] opponentAvailableCardSlots;
    public Transform opponentBattleSlot; // Opponent's battle slot

    private void Start()
    {
        // Shuffle player's deck and play the top 3 cards
        ShuffleDeck(playerDeck);
        for (int i = 0; i < Mathf.Min(playerDeck.Count, 3); i++)
        {
            PlayCard(playerDeck[i], i, true);
            OpponentDrawCard();
        }
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
                    playerDeck.Remove(randomCard);
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
            Card randomCard = opponentDeck[Random.Range(0, opponentDeck.Count)];
            for (int i = 0; i < opponentAvailableCardSlots.Length; i++)
            {
                if (opponentAvailableCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    // For opponent, you might handle the handIndex differently
                    // For example, you might set it to -1 or some other value
                    randomCard.handIndex = -1; // Placeholder value for opponent
                    randomCard.transform.position = opponentCardSlots[i].position;
                    randomCard.hasBeenPlayed = false;
                    opponentDeck.Remove(randomCard);
                    opponentAvailableCardSlots[i] = false;
                    return;
                }
            }
        }
    }

    public void PlayerShuffle()
    {
        // Shuffling is not needed since cards go back to the bottom of the deck when used
    }

    public void OpponentShuffle()
    {
        // Shuffling is not needed since cards go back to the bottom of the deck when used
    }

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

    private void ShuffleDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    private void PlayCard(Card card, int index, bool isPlayer)
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
                // For opponent, you might handle the handIndex differently
                // For example, you might set it to -1 or some other value
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
        CardType playerCardType = playerCard.GetComponent<Card>().cardType;
        CardType opponentCardType = opponentCard.GetComponent<Card>().cardType;

        // Determine the winner based on card types
        if ((playerCardType == CardType.Holy && opponentCardType == CardType.Demonic) ||
            (playerCardType == CardType.Demonic && opponentCardType == CardType.Terrestrial) ||
            (playerCardType == CardType.Terrestrial && opponentCardType == CardType.Holy))
        {
            Debug.Log("Player Wins!");
        }
        else if (playerCardType == opponentCardType)
        {
            Debug.Log("It's a Tie!");
        }
        else
        {
            Debug.Log("Opponent Wins!");
        }

        StartCoroutine(ReturnCards(playerCard, opponentCard));
    }

    IEnumerator ReturnCards(Card playerCard, Card opponentCard)
    {
        yield return new WaitForSeconds(10f);

        PlayerDiscard(playerCard);
        OpponentDiscard(opponentCard);

        // Draw new cards for player and opponent
        PlayerDrawCard();
        OpponentDrawCard();
    }

    public void OpponentPlayCard()
    {
        if (opponentDeck.Count > 0)
        {
            // Choose a random available card slot for the opponent
            int randomIndex = Random.Range(0, opponentAvailableCardSlots.Length);
            while (!opponentAvailableCardSlots[randomIndex])
            {
                randomIndex = Random.Range(0, opponentAvailableCardSlots.Length);
            }

            // Play the card in the chosen slot
            Card opponentCard = opponentDeck[Random.Range(0, opponentDeck.Count)];
            PlayCard(opponentCard, randomIndex, false);

            // Start the battle between player's card and opponent's card
            StartBattle(playerBattleSlot.GetComponentInChildren<Card>(), opponentCard);
        }
        else
        {
            Debug.LogWarning("Opponent's deck is empty!");
        }
    }
}
