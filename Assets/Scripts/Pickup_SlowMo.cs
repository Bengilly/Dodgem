using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_SlowMo : PickupObject
{
    private AudioSource audioSource;

    public AudioClip slowmoAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void CollectPickup()
    {
        //play bomb audio
        audioSource.PlayOneShot(slowmoAudio, 0.3f);

        //disable mesh of pickup
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        //hide all child components of clock prefab with renderer component
        Renderer[] mr = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in mr)
        {
            r.enabled = false;
        }

        gameObject.GetComponent<Light>().enabled = false;

        DeletePickup();
    }

    public override void DeletePickup()
    {
        Destroy(gameObject, 2f);
    }

    public void SlowMotion()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

}
