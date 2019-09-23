using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Button QuitButton;
    private Button BetButton;
    private Button HitButton;
    private Button StayButton;
    private Button DoubleDownButton;
    private Button DealButton;
    private Button NewButton;
    private Button QuadDownButton;

    private Slider BetSlider;
    private GameObject BettingTool;
    private GameObject SliderValueText;

    private List<Text> betUIs;

    private bool toggleQuitUI = false;

    // Start is called before the first frame update
    void Start()
    {
        betUIs = new List<Text>();

        QuitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
        BetButton = GameObject.FindGameObjectWithTag("BetButton").GetComponent<Button>();
        StayButton = GameObject.FindGameObjectWithTag("StayButton").GetComponent<Button>();
        DealButton = GameObject.FindGameObjectWithTag("DealButton").GetComponent<Button>();
        HitButton = GameObject.FindGameObjectWithTag("HitButton").GetComponent<Button>();
        DoubleDownButton = GameObject.FindGameObjectWithTag("DoubleDownButton").GetComponent<Button>();
        BetSlider = GameObject.FindGameObjectWithTag("BetSlider").GetComponent<Slider>();
        SliderValueText = GameObject.FindGameObjectWithTag("SliderValueText");
        NewButton = GameObject.FindGameObjectWithTag("NewGameButton").GetComponent<Button>();
        QuadDownButton = GameObject.FindGameObjectWithTag("QuadDownButton").GetComponent<Button>();


        betUIs.Add(GameObject.FindGameObjectWithTag("BetUI1").GetComponent<Text>());
        betUIs.Add(GameObject.FindGameObjectWithTag("BetUI2").GetComponent<Text>());
        betUIs.Add(GameObject.FindGameObjectWithTag("BetUI3").GetComponent<Text>());
        betUIs.Add(GameObject.FindGameObjectWithTag("BetUI4").GetComponent<Text>());
        betUIs.Add(GameObject.FindGameObjectWithTag("BetUI5").GetComponent<Text>());
    }

    void Update()
    {
        // Reverse the active state every time escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleQuitUI = !toggleQuitUI;
        }
        if (toggleQuitUI)
        {
            QuitButton.gameObject.SetActive(true);
        }
        else
        {
            QuitButton.gameObject.SetActive(false);
        }

    }
        public void enableBetUI(int turn)
    {
        betUIs[turn].gameObject.SetActive(true);
        betUIs[turn].color = Color.yellow;
    }


    public void disableAllBetUIAmounts()
    {
        foreach (Text t in betUIs)
        {
            t.gameObject.SetActive(false);
        }
    }


    public void setBetUI(int turn, float amt)
    {
        enableBetUI(turn - 1);
        betUIs[turn - 1].text = "$" + amt;
    }


    public void setWinLoseUI(int turn, bool isWinner, int amt)
    {
        turn -= 1;
        betUIs[turn].gameObject.SetActive(true);
        if (isWinner)
        {
            betUIs[turn].color = Color.green;
            betUIs[turn].text = "WIN! \n +$"+ amt ;
            return;
        }
        else if (amt == 0)
        {
            betUIs[turn].color = Color.white;
            betUIs[turn].text = "TIE! + - $0" ;
        }
        betUIs[turn].color = Color.red;
        betUIs[turn].text = "LOSE! \n -$"+ amt;
    }


    public void enableBetSliderUI() // sets bet slider and buttons to param
    {
        StayButton.gameObject.SetActive(false);
        HitButton.gameObject.SetActive(false);
        DoubleDownButton.gameObject.SetActive(false);
        QuadDownButton.gameObject.SetActive(false);
        NewButton.gameObject.SetActive(false);

        BetSlider.gameObject.SetActive(true);
        BetButton.gameObject.SetActive(true);
        DealButton.gameObject.SetActive(true);
        SliderValueText.SetActive(true);
    }

    public void enableNewGame()
    {
        NewButton.gameObject.SetActive(true);
        StayButton.gameObject.SetActive(false);
        HitButton.gameObject.SetActive(false);
        DoubleDownButton.gameObject.SetActive(false);
        QuadDownButton.gameObject.SetActive(false);

    }
    public void enableGameUI() // sets stay and hit button to param
    {
       // BettingTool.SetActive(false);
        DealButton.gameObject.SetActive(false);
        BetButton.gameObject.SetActive(false);
        SliderValueText.SetActive(false);
        BetSlider.gameObject.SetActive(false);

        StayButton.gameObject.SetActive(true);
        HitButton.gameObject.SetActive(true);
        DoubleDownButton.gameObject.SetActive(true);
        QuadDownButton.gameObject.SetActive(true);
    }

}
