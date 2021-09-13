//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Target : MonoBehaviour
//{
//    private float minSpeed = 12;
//    private float maxSpeed = 16;
//    private float maxTorque = 10;
//    private float xRange = 4;
//    private float ySpawnPos = -2;

//    private GameManager gameManager;
//    private Rigidbody targetRb;
//    public ParticleSystem explosionParticle;

//    public int pointValue;

//    // Start is called before the first frame update
//    void Start()
//    {
//        targetRb = GetComponent<Rigidbody>();
//        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
//        gameManager.GetComponent<GameManager>();

//        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
//        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

//        transform.position = RandomSpawnPosition();

//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    private void OnMouseDown()
//    {
//        if (gameManager.isGameActive)
//        {
//            Destroy(gameObject);
//            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
//            gameManager.UpdateScore(pointValue);
//        }

//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Destroy(gameObject);
//        if (!gameObject.CompareTag("BadObject"))
//        {
//            gameManager.GameOver();
//        }
        
//    }

//    //calculate random force to objects
//    Vector3 RandomForce()
//    {
//        return Vector3.up * Random.Range(minSpeed, maxSpeed);
//    }

//    //calculate random forque to objects
//    float RandomTorque()
//    {
//        return Random.Range(-maxTorque, maxTorque);
//    }

//    //calculate random spawn position for objects
//    Vector3 RandomSpawnPosition()
//    {
//        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
//    }
//}
