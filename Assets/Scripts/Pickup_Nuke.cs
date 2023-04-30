using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Nuke : PickupObject
{
    public ParticleSystem deathParticle;
    public AudioClip nukeAudio;

    private GameObject[] enemyObj;
    private AudioSource Audiosource;
    private Light nukeLight;

    // Start is called before the first frame update
    void Start()
    {
        Audiosource = GetComponent<AudioSource>();

        nukeLight = GetComponent<Light>();
    }

    public override void CollectPickup()
    {
        //play bomb audio
        Audiosource.PlayOneShot(nukeAudio, 0.3f);

        //disable mesh and lighting of pickup
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Light>().enabled = false;

        KillEnemies();
        DeletePickup();
    }

    public override void DeletePickup()
    {
        Destroy(gameObject, 2f);
    }

    //destroys enemy objects
    private void KillEnemies()
    {
        enemyObj = GameObject.FindGameObjectsWithTag("Enemy");
        //destroy half the enemies
        for (int i = 0; i <= enemyObj.Length /2; i++)
            {
                Destroy(enemyObj[i]);
                Instantiate(deathParticle, enemyObj[i].transform.position, deathParticle.transform.rotation);
            }
    }
}
