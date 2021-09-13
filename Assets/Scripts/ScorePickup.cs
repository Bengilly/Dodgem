using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NEED TO MOVE SCORE IMPLEMENTATION FROM GAMEMANAGER

public class ScorePickup : MonoBehaviour
{
    private SpawnManager spawnManager;
    private GameManager gameManager;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
