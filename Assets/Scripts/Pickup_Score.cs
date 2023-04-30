using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NEED TO MOVE SCORE IMPLEMENTATION FROM GAMEMANAGER

public class Pickup_Score : PickupObject
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

    public override void CollectPickup()
    {
    //    Instantiate(scoreParticle, transform.position, scoreParticle.transform.rotation);

    //    StartCoroutine(ScorePickupCountdownRoutine());
    //    Destroy(gameObject);
    //    gameManager.UpdateScore(score);
    }

    public override void DeletePickup()
    {
    //    throw new System.NotImplementedException();
    }

    //IEnumerator ScorePickupCountdownRoutine()
    //{
    //    if (slowmoScript.slowMoActive == true)
    //    {
    //        player.playerSpeed = 20.0f;
    //    }
    //    else
    //    {
    //        player.playerSpeed = 15.0f;
    //    }

    //    yield return new WaitForSeconds(1);

    //    if (slowmoScript.slowMoActive == true)
    //    {
    //        player.playerSpeed = 15.0f;
    //    }
    //    else
    //    {
    //        player.playerSpeed = 10.0f;
    //    }

    //}
}
