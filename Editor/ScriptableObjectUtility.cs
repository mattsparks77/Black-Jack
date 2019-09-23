using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    // allows easy creation of cards and custom cards through an editor script

    static readonly string path = "Assets/CardObjects/";

    static CardObject CreateCard(bool isFaceUp, string csuit, string name, int value, int count, GameObject prefab)
    {
        CardObject o = ScriptableObject.CreateInstance<CardObject>();
        o.InitCardObject(isFaceUp, csuit, name, value, count, prefab);
        o.path = AssetDatabase.GenerateUniqueAssetPath(path + "/Card_ " + name + ".asset");

        return o;
    }


    public static void CreateAsset<CardObject>(bool isFaceUp, string csuit, string name, int value, int count, GameObject prefab) where CardObject : ScriptableObject
    {
        Debug.Log(Selection.activeObject);
        var asset = CreateCard( isFaceUp,  csuit,  name, value, count, prefab);

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Card_" + name + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        asset.name = name;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}

