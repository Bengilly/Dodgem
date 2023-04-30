using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public ParticleSystem deathParticle;
    public ParticleSystem scoreParticle;
    public GameObject powerupIndicator;
    public float turnSmoothTime = 0.1f;
    public bool hasPowerup = false;
    public float playerSpeed = 10.0f;
    public bool slowMoActive = false;

    private GameManager gameManager;
    private Pickup_Nuke nukePickup;
    private Pickup_Shield shieldPickup;
    private Pickup_SlowMo slowMoPickup;
    private Vector3 lookDirection;
    private float gameBoundaries = 9.3f;
    private float horizontalInput;
    private float verticalInput;
    private float turnSmoothVelocity;
    private float gravityForce = -9.3f;
    private int score = 1;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        powerupIndicator.transform.position = transform.position + new Vector3(0, 0, 0);
        powerupIndicator.transform.Rotate(0, 100 * Time.deltaTime, 0 );
    }

    //input controls for moving player
    private void MovePlayer()
    {
        if (gameManager.isGameActive == true)
        {
            PlayerBoundaries();

            PlayerInput();
        }
    }

    private void PlayerInput()
    {
        //player input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        lookDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if (lookDirection.magnitude >= 0.1f)
        {
            //calculate angle to face
            float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;

            //smooth rotation of player
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            anim.SetInteger("Walk", 1);

            //move player and assign gravity
            Vector3 gravity = new Vector3(0.0f, gravityForce, 0.0f);
            controller.Move(lookDirection * playerSpeed * Time.deltaTime + gravity);
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }
    }

    //Trigger events for pickups
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("ScorePickup"))
        {

            Instantiate(scoreParticle, transform.position, scoreParticle.transform.rotation);

            StartCoroutine(ScorePickupCountdownRoutine());
            Destroy(collider.gameObject);
            gameManager.UpdateScore(score);
        }

        if (collider.gameObject.CompareTag("Powerup"))
        {
            shieldPickup = GameObject.FindWithTag("Powerup").GetComponent<Pickup_Shield>();
            shieldPickup.CollectPickup();

            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountDownRoutine());
        }

        if (collider.gameObject.CompareTag("Nuke"))
        {
            nukePickup = GameObject.FindWithTag("Nuke").GetComponent<Pickup_Nuke>();
            nukePickup.CollectPickup();
        }

        if (collider.gameObject.CompareTag("SlowMo"))
        {
            slowMoPickup = GameObject.FindWithTag("SlowMo").GetComponent<Pickup_SlowMo>();

            StartCoroutine(SlowMoCountDownRoutine());
            slowMoPickup.CollectPickup();
        }
    }

    //Score coroutine to give player a speed boost when they collect score pickup
    IEnumerator ScorePickupCountdownRoutine()
    {
        if (slowMoActive == true)
        {
            playerSpeed = 20.0f;
        }
        else
        {
            playerSpeed = 15.0f;
        }

        yield return new WaitForSeconds(1);

        if (slowMoActive == true)
        {
            playerSpeed = 15.0f;
        }
        else
        {
            playerSpeed = 10.0f;
        }

    }

    //powerup coroutine to disable powerup after 3 seconds
    IEnumerator PowerupCountDownRoutine()
    {
        yield return new WaitForSeconds(3);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator SlowMoCountDownRoutine()
    {
        slowMoActive = true;
        slowMoPickup.SlowMotion();
        playerSpeed = 20.0f;

        yield return new WaitForSeconds(2);

        slowMoActive = false;
        playerSpeed = 10.0f;
        slowMoPickup.NormalSpeed();
    }

    //checks collisions between player and other objects
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasPowerup)
        {
            Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);

            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    //defining the playzone -- setting wall boundaries//
    private void PlayerBoundaries()
    {
        //left wall
        if (transform.position.x < -gameBoundaries)
        {
            transform.position = new Vector3(-gameBoundaries, transform.position.y, transform.position.z);
        }
        //rightwall
        if (transform.position.x > gameBoundaries)
        {
            transform.position = new Vector3(gameBoundaries, transform.position.y, transform.position.z);
        }
        //bottom wall
        if (transform.position.z < -gameBoundaries)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -gameBoundaries);
        }
        //top wall
        if (transform.position.z > gameBoundaries)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, gameBoundaries);
        }
    }
}
