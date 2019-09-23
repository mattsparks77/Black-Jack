using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    public static void CreateCardSciptObject(bool isFaceUp, string csuit, string name, int value, int count, GameObject prefab)
    {
        Debug.Log("Card is being created!! WOO!!");
        ScriptableObjectUtility.CreateAsset<CardObject>(isFaceUp, csuit, name, value, count, prefab);
    }
}
