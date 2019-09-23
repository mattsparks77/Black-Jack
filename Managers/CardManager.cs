using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;



public class CardManager : MonoBehaviour
{
    public List<CardObject> cards;
    public List<GameObject> CardPool;

    string json;

    public HashSet<CardObject> deck;
    public Queue<CardObject> deckQ;

    public Dictionary<int, List<CardObject>> tableHands;
    public TableSeat[] tableSeats;

    public AudioSource source;

    public GameObject DeckSpawn;
    public bool AutoDealing;
    public bool isPaused = false;

    public int numPlayers = 5;
    public int playerTurn = 1;

    public GameManager game;
    public float clearSeconds = 5f;

    public void initCardManager(GameManager g)
    {
        game = g;
        System.Random rng = new System.Random();

        tableSeats = new TableSeat[6];
        tableSeats[0] = GameObject.FindGameObjectWithTag("DealerSpawn").AddComponent<TableSeat>();
        tableSeats[1] = GameObject.FindGameObjectWithTag("Player1Spawn").AddComponent<TableSeat>();
        tableSeats[2] = GameObject.FindGameObjectWithTag("Player2Spawn").AddComponent<TableSeat>();
        tableSeats[3] = GameObject.FindGameObjectWithTag("Player3Spawn").AddComponent<TableSeat>();
        tableSeats[4] = GameObject.FindGameObjectWithTag("Player4Spawn").AddComponent<TableSeat>();
        tableSeats[5] = GameObject.FindGameObjectWithTag("Player5Spawn").AddComponent<TableSeat>();

        DeckSpawn = GameObject.FindGameObjectWithTag("DeckSpawn");


        //passes in the player controller into each table seat class
        tableSeats[0].InitTableSeat(game.playerController);
        tableSeats[1].InitTableSeat(game.playerController);
        tableSeats[2].InitTableSeat(game.playerController);
        tableSeats[3].InitTableSeat(game.playerController);
        tableSeats[4].InitTableSeat(game.playerController);
        tableSeats[5].InitTableSeat(game.playerController);

        source = this.gameObject.AddComponent<AudioSource>();

        deckQ = new Queue<CardObject>(); // queue of card objects for in game

        cards = new List<CardObject>(); // list of possible cards in game
        CardPool = new List<GameObject>(); // pool used for card objects

        deck = new HashSet<CardObject>();

        SpawnDeck();
        CreateDeckQueue();
    }


    public void SpawnDeck()
    {
        //reads in scriptable objects of all cards and instantiates a deck of them
        TextAsset ca = Resources.Load<TextAsset>("CardObjects/loadCardObjects");
        string[] cardList = Regex.Split(ca.text, "\n");

        foreach (string c in cardList)
        {
            CardObject co = Resources.Load<CardObject>("CardObjects/" + c);
            cards.Add(co);
        }
      
    }


    public void FlipCard(GameObject g) // rotates an object 180 dealer first card
    {
        g.gameObject.transform.Rotate (new Vector3(180f, 0f, 180f), Space.World);
    }


    public void incrementTurnCounter() // increments player turn counter in game and this module
    {
        tableSeats[playerTurn].cardValue = LogicModel.Check21(tableSeats[playerTurn]);
        if (tableSeats[playerTurn].cardValue == 21)
        {
            Debug.Log("BlackJack!!");
            playerTurn += 1;
            if (playerTurn >= numPlayers)
            {
                playerTurn = 0;
            }
            return;
        }
        if ( playerTurn >= numPlayers )
        {
            playerTurn = 0;
        }
        else
        {
            playerTurn += 1;

        }
    }

    public Transform GetCardHolder(int turn)
    {
        int numC = tableSeats[turn].GetNumCards();

        return tableSeats[turn].gameObject.transform.GetChild(numC);
    }


    public void CalculateBets()
    {
        int toBeat = tableSeats[0].cardValue;
        Debug.Log("DealerTotal: " + toBeat);
        for (int i = 1; i < tableSeats.Length; i++)
        {
            if (toBeat > 21 && tableSeats[i].cardValue < 22) // dealer busts you didnt so you iwn 
            {
                game.betManager.myWallet.AddMoney(tableSeats[i].tableBet * 2);
                game.uiManager.setWinLoseUI(i, true, (int)tableSeats[i].tableBet*2);

            }
            else if (tableSeats[i].cardValue > toBeat && tableSeats[i].cardValue < 22) // you beat dealers hand
            {
                game.betManager.myWallet.AddMoney(tableSeats[i].tableBet * 2);
                game.uiManager.setWinLoseUI(i, true, (int)tableSeats[i].tableBet*2);

            }
            else if (tableSeats[i].cardValue == toBeat) // tie the dealer
            {
                game.betManager.myWallet.AddMoney(tableSeats[i].tableBet);
                game.uiManager.setWinLoseUI(i, false, 0);

            }
            else
            {
                game.betManager.myWallet.AddToBetHistory(-(int)tableSeats[i].tableBet);

                game.uiManager.setWinLoseUI(i, false, (int)tableSeats[i].tableBet);
            }
        }
    }


    public void ClearTable()
    {
        foreach (TableSeat t in tableSeats)
        {
            t.Reset();
        }
        foreach (GameObject g in CardPool)
        {
            Destroy(g);
        }
        playerTurn = 1;
        isPaused = false;
        game.gameStarted = false;
        game.uiManager.enableBetSliderUI();
        game.uiManager.disableAllBetUIAmounts();
        game.betManager.ResetBetTurn();
    }


    public void StartDealerTurn()
    {
        isPaused = true;
        FlipCard(tableSeats[0].CardGameObjects[0]);
        while (LogicModel.DealerDecision(tableSeats[0]) == "HIT")
        {
            if (deckQ.Count <= 0)
            {
                CreateDeckQueue();
            }
            CardObject c = deckQ.Dequeue();

            //creates a card object from the prefab
            GameObject g = Instantiate(c.prefab, GetCardHolder(0));
            //adds the cards into the list of card objects and game objects at each seat
            tableSeats[0].seatCards.Add(c);
            tableSeats[0].CardGameObjects.Add(g);

            tableSeats[0].IncrementCardsInPlay();
            CardPool.Add(g);

        }

        isPaused = false;
        incrementTurnCounter();
        tableSeats[0].cardValue = LogicModel.Check21(tableSeats[0]);
        // EndGame();
        CalculateBets();
        game.uiManager.enableNewGame();
        Debug.Log(LogicModel.DealerDecision(tableSeats[playerTurn]));

    }


    private void EndGame()
    {
        CalculateBets();
        StartCoroutine(ClearTableCoroutine()); // clears table after round is over

    }


    IEnumerator ClearTableCoroutine()
    {
        yield return new WaitForSeconds(clearSeconds);    

        ClearTable();
    }


    public bool CheckAllBetsMade()
    {
        for (int i = 1; i < tableSeats.Length; i++)
        {
            if (tableSeats[i].lockedBet == false)
            {
                return false;
            }
        }
        return true;
    }


    public void Hit()
    {
        if (isPaused)
        {
            return;
        }
        if (deckQ.Count <= 0)
        {
            CreateDeckQueue();
        }


        CardObject c = deckQ.Dequeue();

        //creates a card object from the prefab
        GameObject g = Instantiate(c.prefab, GetCardHolder(playerTurn));
        //adds the cards into the list of card objects and game objects at each seat
        tableSeats[playerTurn].seatCards.Add(c);
        tableSeats[playerTurn].CardGameObjects.Add(g);

        tableSeats[playerTurn].IncrementCardsInPlay();
        CardPool.Add(g);

        tableSeats[playerTurn].cardValue = LogicModel.Check21(tableSeats[playerTurn]);
        if (tableSeats[playerTurn].cardValue >= 21 || tableSeats[playerTurn].isFrozen)
        {
            Debug.Log("Turn OVER!");
            incrementTurnCounter();
            if (playerTurn == 0)
            {
                isPaused = true;
                StartDealerTurn();
                return;
            }
        }
    }


    public void StartQuadDownSequence()
    {
        if (tableSeats[playerTurn].quadDown)
        {
            incrementTurnCounter();
            return;
        }
        if (!(game.betManager.myWallet.money >= tableSeats[playerTurn].tableBet))
        {
            Debug.Log("Not enough funds to quad down..");
            return;
        }
       
        game.betManager.QuadBet();
        for (int i = 0; i < 2; i++)
        {
            QuadDown();
        }

        incrementTurnCounter();
        if (playerTurn == 0)
        {
            isPaused = true;
            StartDealerTurn();
            return;
        }
    }

    public void QuadDown() // like double down except its 4x2 so you bet quads and have to get dealt two cards in a row
    {
        if (tableSeats[playerTurn].cardValue >= 21)
        {
            Debug.Log("Busted before second card could come out :(");
            
            return;
        }
        if (deckQ.Count <= 0)
        {
            CreateDeckQueue();
        }

        tableSeats[playerTurn].quadDown = true;

        CardObject c = deckQ.Dequeue();

        //creates a card object from the prefab
        GameObject g = Instantiate(c.prefab, GetCardHolder(playerTurn));
        //adds the cards into the list of card objects and game objects at each seat
        tableSeats[playerTurn].seatCards.Add(c);
        tableSeats[playerTurn].CardGameObjects.Add(g);

        tableSeats[playerTurn].IncrementCardsInPlay();
        CardPool.Add(g);

        tableSeats[playerTurn].cardValue = LogicModel.Check21(tableSeats[playerTurn]);
       
     
       
        


    }

    public void DoubleDown()
    {
        if (tableSeats[playerTurn].doubleDown)
        {
            incrementTurnCounter();
            return;
        }
        if (!(game.betManager.myWallet.money >= tableSeats[playerTurn].tableBet))
        {
            Debug.Log("Not enough funds to double down..");
            return;
        }
        tableSeats[playerTurn].doubleDown = true;
        if (deckQ.Count <= 0)
        {
            CreateDeckQueue();
        }


        CardObject c = deckQ.Dequeue();

        //creates a card object from the prefab
        GameObject g = Instantiate(c.prefab, GetCardHolder(playerTurn));
        //adds the cards into the list of card objects and game objects at each seat
        tableSeats[playerTurn].seatCards.Add(c);
        tableSeats[playerTurn].CardGameObjects.Add(g);

        tableSeats[playerTurn].IncrementCardsInPlay();
        CardPool.Add(g);

        tableSeats[playerTurn].cardValue = LogicModel.Check21(tableSeats[playerTurn]);
        Debug.Log("doubled down!!");
        game.betManager.DoubleBet();
        incrementTurnCounter();
        if (playerTurn == 0)
        {
            isPaused = true;
            StartDealerTurn();
            return;
        }
        
    }


    public int TestDealCard(int turnCounter = 0) // used for testing purposes
    {
        if (tableSeats[turnCounter].cardValue >= 21 || tableSeats[turnCounter].isFrozen)
        {
            return -1;
        }
        if (deckQ.Count <= 0)
        {
            CreateDeckQueue();
        }
     
        CardObject c = deckQ.Dequeue();

        //adds the cards into the list of cards at each seat
        tableSeats[turnCounter].seatCards.Add(c);

        //creates a card object from the prefab
        GameObject g = Instantiate(c.prefab,GetCardHolder(turnCounter));

        tableSeats[turnCounter].IncrementCardsInPlay();
        CardPool.Add(g);

        tableSeats[turnCounter].cardValue = LogicModel.Check21(tableSeats[turnCounter]);
        //checks and returns players total 
        return tableSeats[turnCounter].cardValue;
    }


    private void Update()
    {
        if (playerTurn == 0 && !isPaused)
        {
            isPaused = true;
            StartDealerTurn();
            return;
        }
    }


    public void Stay()
    {
        if (isPaused)
        {
            return;
        }
        tableSeats[playerTurn].cardValue = LogicModel.Check21(tableSeats[playerTurn]);

        tableSeats[playerTurn].isFrozen = true;
        incrementTurnCounter();
    }


    public void AutoDealCards() // deals the initial first two cards to all five spots at the table finishing with the dealer
    {
        if (deckQ.Count <= 0)
        {
            CreateDeckQueue();
        }

        CardObject c = deckQ.Dequeue();
        GameObject Card;

        // checks if its dealers first card if so then deals it face down
        if (playerTurn == 0 && tableSeats[playerTurn].GetNumCards() == 0) 
        {
            tableSeats[playerTurn].seatCards.Add(c);
            Card = Instantiate(c.prefab, GetCardHolder(playerTurn));
            FlipCard(Card);
        }
        else
        {
            tableSeats[playerTurn].seatCards.Add(c);
            Card = Instantiate(c.prefab, GetCardHolder(playerTurn));
        }
        tableSeats[playerTurn].CardGameObjects.Add(Card);
        // increases the count of total cards at a given table seat 
        tableSeats[playerTurn].IncrementCardsInPlay();
        //Adds cards into a pool for deletion.. thought about possibly making an object pool so wouldnt have to instantiate/destroy so much
        CardPool.Add(Card);

        incrementTurnCounter();

        if (playerTurn == 1 && tableSeats[playerTurn].GetNumCards() == 2)
        {
            AutoDealing = false; // deals last card to dealer thus ending the auto dealing
        }
    }


    public void CreateDeckQueue() // creates a deck queue for the game
    {
        cards.ShuffleList<CardObject>();

        deck.UnionWith(cards);

        foreach (CardObject c in deck)
        {
            deckQ.Enqueue(c);
        }
    }

}
