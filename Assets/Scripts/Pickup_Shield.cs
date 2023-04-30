using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Shield : PickupObject
{
    private SpawnManager spawnManager;
    private PlayerController player;
    private AudioSource audioSource;
    private float powerupStrength = 5.0f;

    public AudioClip powerupAudio;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        audioSource = GetComponent<AudioSource>();
    }

    public override void CollectPickup()
    {
        //play shield audio
        audioSource.PlayOneShot(powerupAudio, 0.3f);

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Light>().enabled = false;

        DeletePickup();
    }

    public override void DeletePickup()
    {
        Destroy(gameObject, 2f);
    }

    //spawn barrier around player and knock away enemies
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
        enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
    }
}
