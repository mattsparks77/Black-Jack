using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardObject : ScriptableObject
{
    private static CardObject card;

    public bool isFaceUp;
    public string csuit;
    public new string name;
    public int value;
    public int count;
    public GameObject prefab;
    public string path;


    public CardObject()
    {
        card = this;
    }

    public void InitCardObject( bool FaceUp, string suit, string n, int value, int ccount, GameObject pref)
    {
        card.name = n;
        card.csuit = suit;
        card.isFaceUp = FaceUp;
        card.prefab = pref;
        card.count = ccount;
        card.value = value;
    }

    public void Print()
    {
        Debug.Log(name + ":" + csuit);
    }


    public void Start()
    {
 

    }
}
