using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightMover : MonoBehaviour
{
    GameManager game;
    int turn;
    Transform trans;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trans = game.cardManager.tableSeats[turn].transform;
        turn = game.cardManager.playerTurn;
        this.transform.position = new Vector3 (trans.position.x, trans.position.y, trans.position.z - 50f);
    }
}
