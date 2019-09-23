using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Wallet
{
    [SerializeField] public int money;
    public List<int> betHistory;
    public string Owner;
    [SerializeField] public int startCash = 1000;
    

    public Wallet(string Name)
    {
        Owner = Name;
        betHistory.Clear();
        betHistory = new List<int>();
       
    }

    public void ClearBetHistory()
    {
        if (money <= 0)
        {
            money = startCash;
            Debug.Log("Bankrupt! Here's one on the house!");
            return;
        }
        betHistory.Clear();
    }

    public List<int> GetBetHistory()
    {
        return betHistory;
    }

    public void AddToBetHistory(int bet)
    {
        betHistory.Add(bet);
    }

    public string GetOwner()
    {
        return Owner;
    }

    public int GetBalance()
    {
        return money;
    }

    public void AddMoney(float m)
    {
        money += (int)m;
        AddToBetHistory((int)m);
    }
    public void RemoveMoney(float m)
    {
        money -= (int)m;
        AddToBetHistory((int)m);
    }
}


public class BetManager : MonoBehaviour
{

    public Wallet myWallet;
    string json = "";
    Slider betSlider;
    public GameManager game;

    public Text walletUI;

    private string path;

    public void InitBetManager(GameManager g)
    {
        path = Path.Combine(Application.streamingAssetsPath, "myWallet.txt");
        LoadJsonWalletData();
        game = g;
        betSlider = GameObject.FindGameObjectWithTag("BetSlider").GetComponent<Slider>();
        betSlider.wholeNumbers = true;

        walletUI = GameObject.FindGameObjectWithTag("WalletText").GetComponent<Text>();


        if (myWallet == null)
        {
            myWallet = new Wallet(name);
            Debug.Log("New Wallet Created..");
            Debug.Log("MyNewBalance = " + myWallet.money);
        }

        myWallet.betHistory.Clear();
    }


    private void Start()
    {
        myWallet.betHistory.Clear();
        if (myWallet.money <= 0)
        {
            myWallet.money = myWallet.startCash;
            Debug.Log("Bankrupt! Here's one on the house! + $100");
        }
    }

    // Update is called once per frame
    void Update()
    {
        walletUI.text = "Wallet: $" + myWallet.money.ToString();
        betSlider.maxValue = myWallet.money;
    }


    public void LoadJsonWalletData()
    {
        //TextAsset t = AssetBundle.LoadFromFile("Assets/Resources/PlayerWallet.txt");
        json = Resources.Load<TextAsset>("PlayerWallet").ToString();
        Debug.Log(json);
        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        myWallet = JsonUtility.FromJson<Wallet>(json);
    }


    public void ResetBetTurn()
    {
        game.BetTurn = 1; // starts from first seat skips dealer
    }


    public void DoubleBet() // removes the initial bet from wallet again and then doubles the bet value stored in tableBet
    {
        int turn = game.cardManager.playerTurn;
        float bet = game.cardManager.tableSeats[turn].tableBet;
        myWallet.RemoveMoney(bet);
        game.uiManager.setBetUI(turn, bet * 2);

        game.cardManager.tableSeats[turn].tableBet *= 2;
    }

    public void QuadBet() // removes the initial bet from wallet again and then quads the bet value stored in tableBet
    {
        int turn = game.cardManager.playerTurn;
        float bet = game.cardManager.tableSeats[turn].tableBet;
        myWallet.RemoveMoney(bet*3);
        game.uiManager.setBetUI(turn, bet * 4);

        game.cardManager.tableSeats[turn].tableBet *= 4;
    }


    public void MakeBet()
    {
        if (game.lockedBetsIn)
        {
            return;
        }

        if (game.BetTurn > game.numPlayers - 1)
        {
            game.lockedBetsIn = true;
            game.gameStarted = true;
            game.StartGameAfterBets();
        }
        game.cardManager.tableSeats[game.BetTurn].tableBet = betSlider.value;
        game.cardManager.tableSeats[game.BetTurn].lockedBet = true;
        game.uiManager.setBetUI(game.BetTurn, betSlider.value);
        game.BetTurn += 1;
        myWallet.RemoveMoney(betSlider.value);
    }


    private void OnApplicationQuit()
    {
        SerializeWallet();
        Debug.Log("Quitting... Wallet = $ " + myWallet.money);
    }


    public void SerializeWallet()
    {
        json = JsonUtility.ToJson(myWallet);
        string path = "Assets/Resources/" + myWallet.Owner + "Wallet.txt";
        File.WriteAllText(path, json);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }


    ~BetManager()
    {
        SerializeWallet();
    }
}
