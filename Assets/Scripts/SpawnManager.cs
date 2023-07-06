using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject scorePickupPrefab;
    public GameObject powerupPrefab;
    public GameObject nukePrefab;
    public GameObject slowMoPrefab;

    private int ScorePickupCount = 0;
    private int BonusPickupCount = 0;
    private readonly float spawnRange = 6.5f;


    // Update is called once per frame
    void Update()
    {
        SpawnPickup();
    }

    //randomise spawn positions in game zone
    private Vector3 GeneratePickupSpawnLocation()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 4.0f, spawnPosZ);

        return randomPos;
     }

    //randomise spawn positions between 4 arena corners
    private Vector3 GenerateEnemySpawnLocation()
    {
        Vector3 posTopLeft = new Vector3(-8.0f, 4.0f, 8.0f);
        Vector3 posTopRight = new Vector3(8.0f, 4.0f, 8.0f);
        Vector3 posBottomLeft = new Vector3(-8.0f, 4.0f, -8.0f);
        Vector3 posBottomRight = new Vector3(8.0f, 4.0f, -8.0f);

        Vector3[] posArray = { posTopLeft, posTopRight, posBottomLeft, posBottomRight };

        int index = Random.Range(0, posArray.Length);

        return posArray[index];
    }

    //spawn pickups
    private void SpawnPickup()
    {
        ScorePickupCount = FindObjectsOfType<Pickup_Score>().Length;
        BonusPickupCount = FindObjectsOfType<Pickup_Shield>().Length + FindObjectsOfType<Pickup_Nuke>().Length + FindObjectsOfType<Pickup_SlowMo>().Length;

        if (GameManager.Instance.state == GameManager.GameState.GameScreen && ScorePickupCount == 0)
        {
            Instantiate(scorePickupPrefab, GeneratePickupSpawnLocation(), scorePickupPrefab.transform.rotation);

            //spawn enemy each time score pickup is collected
            SpawnEnemyWave();
        }

        if (GameManager.Instance.state == GameManager.GameState.GameScreen && BonusPickupCount == 0)
        {
            int rand = Random.Range(1, 4);
            if (rand == 1)
            {
                Instantiate(powerupPrefab, GeneratePickupSpawnLocation(), powerupPrefab.transform.rotation);
            }

            else if (rand == 2)
            {
                Instantiate(nukePrefab, GeneratePickupSpawnLocation(), nukePrefab.transform.rotation);
            }

            else if (rand == 3)
            {
                Instantiate(slowMoPrefab, GeneratePickupSpawnLocation(), slowMoPrefab.transform.rotation);
            }

        }
    }

    //spawn enemies
    private void SpawnEnemyWave()
    {
        Instantiate(enemyPrefab, GenerateEnemySpawnLocation(), enemyPrefab.transform.rotation);
    }

}