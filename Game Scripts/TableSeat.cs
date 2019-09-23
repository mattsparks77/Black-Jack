using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSeat : MonoBehaviour
{
    public List<CardObject> seatCards;
    public List<GameObject> CardGameObjects;
    public Wallet playerWallet;

    private int numCardsInPlay;
    public int cardValue;
    public float tableBet;

    public PlayerController playerSeated;

    public bool isComputer;
    public bool isFrozen;
    public bool lockedBet;
    public bool quadDown = false;
    public bool doubleDown = false;

    public void InitTableSeat(PlayerController pc)
    {
        playerSeated = pc;
        if (isComputer)
        {
            playerWallet = new Wallet("Computer");
        }
        else
        {
            playerWallet = playerSeated.myWallet;
        }
        seatCards = new List<CardObject>();
        CardGameObjects = new List<GameObject>();
        numCardsInPlay = 0;
    }


    private void ClearSeatCards()
    {
        seatCards.Clear();
        CardGameObjects.Clear();
    }


    public int GetNumCards()
    {
        return numCardsInPlay;
    }


    public void IncrementCardsInPlay()
    {
        numCardsInPlay += 1;
    }


    public void Reset()
    {
        tableBet = 0;
        ClearSeatCards();
        numCardsInPlay = 0;
        isFrozen = false;
        lockedBet = false;
        doubleDown = false;
        quadDown = false;
    }


    public void Stay()
    {
        isFrozen = true;
    }


    public void AddPlayer(PlayerController pc)
    {
        playerSeated = pc;
        isComputer = false;
    }


    public void RemovePlayer()
    {
        playerSeated = null;
    }


    public void AddComputer()
    {
        isComputer = true;
    }
}
