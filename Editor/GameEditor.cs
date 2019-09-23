using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameEditor : Editor
{
    public int turnCount;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        turnCount = EditorGUILayout.IntField("Test Player Turn: ", turnCount);
        GameManager myScript = (GameManager)target;

        if (GUILayout.Button("Test Deal Card"))
        {
            myScript.cardManager.TestDealCard(turnCount);
        }
        if (GUILayout.Button("Create New Game"))
        {
            myScript.CreateNewGame();
        }
        if (GUILayout.Button("Clear Table"))
        {
            myScript.cardManager.ClearTable();
        }


    }
}
