using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour
{
    public Wallet myWallet;
    public GameManager game;

    public void InitPlayerController(string name, GameManager g)
    {
        game = g;
    }


    public GameObject OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 500))
            {
                Debug.Log(hit.transform.gameObject.name);
                return hit.transform.gameObject;
            }
        }
        return null;
    }
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ClickedOn = OnClick();
        }
    }


}
