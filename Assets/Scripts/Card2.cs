using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card2 : MonoBehaviour
{
    [TextArea(3, 10)]
    public string cardDescription;
    public bool hasBeenPlayed;
    public bool revealed = true;
    public bool playerCard;
    public int handIndex;
    public GameManager2.CardType cardType;
    public int cardValue;

    GameManager2 gm;
    public DeckBuilder3 deckBuilder;
    public bool inCustomDeck;

    private bool updatingCardValue = false;

    private bool isLocked = false;

    public enum CardType { Holy, Terrestrial, Demonic }

    public SpriteRenderer spriteRenderer;

    public Sprite[] upgradedSprites;
    public Sprite[] changedSprites;
    public bool faceDown = false;
    public Sprite faceDownSprite;
    public Sprite faceUpSprite;
    public bool blackJackGame = false;

    private void Start()
    {
        gm = FindObjectOfType<GameManager2>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        faceUpSprite = spriteRenderer.sprite;
        DontDestroyOnLoad(this);
        //Debug.Log("Card Created: " + name);
    }

    void Update()
    {
        gm = FindObjectOfType<GameManager2>();
        if (faceDown == true && blackJackGame == true)
        {
            spriteRenderer.sprite = faceDownSprite;
        }
        else if(faceDown == false && blackJackGame == true)
        {
            spriteRenderer.sprite = faceUpSprite;
        }
    }

    public void OnMouseDown()
    {
        if (updatingCardValue)
        {
            return;
        }

        if (deckBuilder != null)
        {
            if (inCustomDeck == false)
            {
                if (deckBuilder.customPlayerDeck.Count < 10)
                {
                    for (int i = 0; i < deckBuilder.availableDeckSlots.Length; i++)
                    {
                        deckBuilder.customPlayerDeck.Add(this);
                        deckBuilder.unchosenCards.Remove(this);
                        inCustomDeck = true;
                        playerCard = true;
                        deckBuilder.cardDescription.text = cardDescription;
                        deckBuilder.AddToCurrentDeck(transform);
                        return;
                    }
                }
                else
                {
                    Debug.Log("Player Deck cannot exceed 10 cards!");
                }
            }
            else
            {
                for (int i = 0; i < deckBuilder.currentDeckSlots.Length; i++)
                {
                    deckBuilder.customPlayerDeck.Remove(this);
                    deckBuilder.unchosenCards.Add(this);
                    inCustomDeck = false;
                    playerCard = false;
                    deckBuilder.RemoveFromCurrentDeck(transform);
                    deckBuilder.cardDescription.text = "";
                    return;
                }
            }
        }
        else if (deckBuilder == null)
        {
            if (!hasBeenPlayed && playerCard)
            {
                Debug.Log("Player card selected");
                hasBeenPlayed = true;
                gm.PlayerPlayCard(this);
            }
        }
    }

    public void UpgradeCardSprite(int index)
    {
        if (index >= 0 && index < upgradedSprites.Length)
        {
            spriteRenderer.sprite = upgradedSprites[index];
            Debug.Log("Upgrade 2");
        }
    }

    public void ChangeCardSprite(int index)
    {
        if (index >= 0 && index < changedSprites.Length)
        {
            spriteRenderer.sprite = changedSprites[index];
            Debug.Log("Change Type 2");
        }
    }

    public void UpgradeCardTypeAndSprite(int index)
    {
        UpgradeCardSprite(index);
        Debug.Log("Upgrade 1");
    }

    public void ChangeCardTypeAndSprite(int index)
    {
        ChangeCardSprite(index);
        Debug.Log("Change Type 1");
    }

    public void LockCard()
    {
        isLocked = true;
        Debug.Log("Card is now locked and cannot be updated or changed further.");
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}
