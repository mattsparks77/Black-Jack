﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (CardGenerator))]
public class CardScriptorEditor : Editor
{

    public GameObject pprefab;

    public int val;

    public string suit;
    public string cname;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        pprefab = (GameObject)EditorGUILayout.ObjectField(pprefab, typeof(GameObject), true);

        val = EditorGUILayout.IntField("Card Value:", val);

        cname = EditorGUILayout.TextField("Card Name:", cname);
        suit = EditorGUILayout.TextField("Card Suit:", suit);

        if (GUILayout.Button("Generate Card"))
        {
            CardGenerator.CreateCardSciptObject(false, suit, cname, val, 0, pprefab);
        }
    }
}

