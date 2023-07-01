using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour, IShopPurchaser
{
    public CharacterController controller;
    public ParticleSystem deathParticle;
    public ParticleSystem scoreParticle;
    public GameObject powerupIndicator;
    public float turnSmoothTime = 0.1f;
    public bool hasPowerup = false;
    public float playerSpeed = 10.0f;
    public bool slowMoActive = false;

    public event EventHandler OnPointsAmountChanged;

    private Player player;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        string characterModel = PlayerPrefs.GetString("CharacterModel", "Cat");

        player.LoadSavedCharacterModel(characterModel);

        SetPlayerBoundaries();
    }


    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    //input controls for moving player
    private void MovePlayer()
    {
        if (GameManager.Instance.IsGameOver() == true)
        {
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

            player.EnableMoveAnimation();

            //move player and assign gravity
            Vector3 gravity = new Vector3(0.0f, gravityForce, 0.0f);
            controller.Move(playerSpeed * Time.deltaTime * lookDirection + gravity);
        }
        else
        {
            player.DisableMoveAnimation();
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
            GameManager.Instance.UpdateScore(score);
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
        powerupIndicator.transform.position = transform.position + new Vector3(0, 0, 0);
        powerupIndicator.transform.Rotate(0, 100 * Time.deltaTime, 0);

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
            GameManager.Instance.GameOver();
        }
    }

    //defining the playzone -- setting wall boundaries//
    private void SetPlayerBoundaries()
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

    public void BoughtItem(UI_ShopItem.Item item)
    {
        Debug.Log("Bought character: " + item);
        switch (item)
        {
            default:
            case UI_ShopItem.Item.Cat:
                player.SetCharacterCat(); break;
            case UI_ShopItem.Item.Chicken:
                player.SetCharacterChicken(); break;
            case UI_ShopItem.Item.Dog:
                player.SetCharacterDog(); break;
            case UI_ShopItem.Item.Lion:
                player.SetCharacterLion(); break;
            case UI_ShopItem.Item.Penguin:
                player.SetCharacterPenguin(); break;
        }
    }

    public bool CanBuyItem(int pointsRequired)
    {
        int shopPoints = GameManager.Instance.GetShopPoints();

        if (shopPoints >= pointsRequired)
        {
            shopPoints -= pointsRequired;
            GameManager.Instance.SetShopPoints(shopPoints);

            //OnPointsAmountChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Can buy item");
            return true;
        }
        else
        {
            Debug.Log("Unable to buy item");
            return false;
        }
    }
}
