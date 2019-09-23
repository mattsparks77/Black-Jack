using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public CardManager cardManager;
    public BetManager betManager;
    public UIManager uiManager;
    public SoundManager soundManager;

    public AudioSource asource;

    public int BetTurn = 1;
    public int playerTurn;
    public int numPlayers = 5;

    public List<CardObject> cards;
    public PlayerController playerController;
    public List<PlayerObject> players;

    public bool gameStarted;
    public bool lockedBetsIn;
    public Text alerter;

    // Start is called before the first frame update
    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").AddComponent<PlayerController>();

        alerter = GameObject.FindGameObjectWithTag("Alerter").GetComponent<Text>();
        playerController.InitPlayerController("Player", GetComponent<GameManager>());

        cardManager = this.gameObject.GetComponent<CardManager>();
        betManager = this.gameObject.GetComponent<BetManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        asource = gameObject.AddComponent<AudioSource>();

        soundManager = new SoundManager();
        soundManager.InitSoundManager();
        soundManager.StartBGMusic(asource);
        cardManager.initCardManager(this);
        betManager.InitBetManager(this);

        gameStarted = false;
        lockedBetsIn = false;

        BetTurn = 1;
        playerTurn = 1;
        numPlayers = 5;

        uiManager.enableBetSliderUI();

        //CreateNewGame();


    }


    public void StartGameAfterBets()
    {
        CreateNewGame();
        soundManager.PlaySoundOneShotName(cardManager.source, "cardShuffle");

        //Debug.Log("game starting..");
        gameStarted = true;
        uiManager.enableGameUI();
        alerter.text = "";
    }


    void Update()
    {
        if (!lockedBetsIn && !gameStarted)
        {
            alerter.text = "SEAT (" + BetTurn + ")" + " BET.";
        }
        else
        {
            alerter.text = "";
        }
    }


    private void StartAutoDeal()
    {
        cardManager.AutoDealing = true;
        //        int max = 0;
        for (int i = 0; i < (numPlayers + 1) * 2; i++)
        {
            cardManager.AutoDealCards();
        }
    }


    public void CreateNewGame()
    {
        lockedBetsIn = false;
        cardManager.game.playerTurn = 1;
        StartAutoDeal();
    }


    public void AddPlayer()
    {
        numPlayers++;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RemovePlayer()
    {
        numPlayers--;
        if (numPlayers < 1)
        {
            //EndGame();
        }
    }

}
