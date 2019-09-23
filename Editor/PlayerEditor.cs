using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController myScript = (PlayerController)target;
        if (GUILayout.Button("Add 50"))
        {
            myScript.myWallet.AddMoney(50);
        }
    }
}
