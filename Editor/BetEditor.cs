using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BetManager))]
public class BetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BetManager myScript = (BetManager)target;
        if (GUILayout.Button("Add 50"))
        {
            myScript.myWallet.AddMoney(50);
        }
    }
}
