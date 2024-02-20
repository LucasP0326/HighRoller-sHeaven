using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;
    public GameManager.CardType cardType; // Added variable for card type

    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        if (gm == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnMouseDown()
    {
        if (!hasBeenPlayed && gm != null)
        {
            Debug.Log("Left mouse button clicked!");
            hasBeenPlayed = true;
            gm.PlayerDiscard(this); // Notify the GameManager that this card has been played
            MoveToBattleSlot();

            // Trigger opponent's card play
            gm.OpponentPlayCard();
        }
    }

    private void MoveToBattleSlot()
    {
        if (!hasBeenPlayed && gm != null) // Check if the card has not been played yet
        {
            Debug.Log("Card not played yet. Checking slot availability...");

            if (handIndex < gm.playerAvailableCardSlots.Length && gm.playerAvailableCardSlots[handIndex]) // Check if the slot is available
            {
                Debug.Log("Moving to player battle slot");
                // Move the card to the player's battle slot
                transform.position = gm.playerBattleSlot.position;
                gm.playerAvailableCardSlots[handIndex] = false;
            }
            else if (handIndex < gm.opponentAvailableCardSlots.Length && gm.opponentAvailableCardSlots[handIndex]) // Check if the slot is available
            {
                Debug.Log("Moving to opponent battle slot");
                // Move the card to the opponent's battle slot
                transform.position = gm.opponentBattleSlot.position;
                gm.opponentAvailableCardSlots[handIndex] = false;
            }
            else
            {
                Debug.LogWarning("Cannot move card to battle slot: Slot not available.");
                Debug.Log("handIndex: " + handIndex);
                Debug.Log("PlayerAvailableCardSlots: " + string.Join(", ", gm.playerAvailableCardSlots));
            }

            hasBeenPlayed = true; // Mark the card as played after moving to the battle slot
            Debug.Log("Card marked as played.");
        }
        else
        {
            Debug.LogWarning("Card has already been played.");
        }
    }
}
