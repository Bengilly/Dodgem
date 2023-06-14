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

    private GameManager gameManager;
    private Pickup_Nuke nukePickup;
    private Pickup_Shield shieldPickup;
    private Pickup_SlowMo slowMoPickup;
    private Animator anim;

    private MeshFilter meshCharacterBody;
    private MeshFilter meshCharacterHead;
    private MeshFilter meshCharacterLegFL;
    private MeshFilter meshCharacterLegFR;
    private MeshFilter meshCharacterLegBL;
    private MeshFilter meshCharacterLegBR;
    private MeshFilter meshCharacterTail;
    private MeshFilter meshCharacterWingL;
    private MeshFilter meshCharacterWingR;

    //cat mesh
    private MeshFilter meshCatBody;
    private MeshFilter meshCatHead;
    private MeshFilter meshCatLegBL;
    private MeshFilter meshCatLegBR;
    private MeshFilter meshCatLegFL;
    private MeshFilter meshCatLegFR;
    private MeshFilter meshCatTail;

    //chicken mesh
    private MeshFilter meshChickenBody;
    private MeshFilter meshChickenHead;
    private MeshFilter meshChickenLegL;
    private MeshFilter meshChickenLegR;

    //dog mesh
    private MeshFilter meshDogBody;
    private MeshFilter meshDogHead;
    private MeshFilter meshDogLegBL;
    private MeshFilter meshDogLegBR;
    private MeshFilter meshDogLegFL;
    private MeshFilter meshDogLegFR;
    private MeshFilter meshDogTail;

    //lion
    private MeshFilter meshLionBody;
    private MeshFilter meshLionHead;
    private MeshFilter meshLionLegBL;
    private MeshFilter meshLionLegBR;
    private MeshFilter meshLionLegFL;
    private MeshFilter meshLionLegFR;
    private MeshFilter meshLionTail;

    //penguin
    private MeshFilter meshPenguinBody;
    private MeshFilter meshPenguinWingL;
    private MeshFilter meshPenguinWingR;


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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();

        string characterModel = PlayerPrefs.GetString("CharacterModel", "Cat");

        LoadCharacterModels();
        LoadSavedCharacterModel(characterModel);
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

    public void BoughtItem(UI_ShopItem.Item item)
    {
        Debug.Log("Bought character: " + item);
        switch (item)
        {
            default:
            case UI_ShopItem.Item.Cat:
                SetCharacterCat(); break;
            case UI_ShopItem.Item.Chicken:
                SetCharacterChicken(); break;
            case UI_ShopItem.Item.Dog:
                SetCharacterDog(); break;
            case UI_ShopItem.Item.Lion:
                SetCharacterLion(); break;
            case UI_ShopItem.Item.Penguin:
                SetCharacterPenguin(); break;
        }
        
    }

    public bool CanBuyItem(int pointsRequired)
    {
        int shopPoints = gameManager.GetShopPoints();

        if (shopPoints >= pointsRequired)
        {
            shopPoints -= pointsRequired;
            gameManager.SetShopPoints(shopPoints);
            
            OnPointsAmountChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Can buy item");
            return true;
        }
        else
        {
            Debug.Log("Unable to buy item");
            return false;
        }
        
    }
    private void LoadSavedCharacterModel(string characterModel)
    {
        if (characterModel == "Cat") { SetCharacterCat(); }
        if (characterModel == "Chicken") { SetCharacterChicken(); }
        if (characterModel == "Dog") { SetCharacterDog(); }
        if (characterModel == "Lion") { SetCharacterLion(); }
        if (characterModel == "Penguin") { SetCharacterPenguin(); }
    }

    private void LoadCharacterModels()
    {
        //character meshes
        meshCharacterBody = GameObject.Find("Body").GetComponent<MeshFilter>();
        meshCharacterHead = GameObject.Find("Head").GetComponent<MeshFilter>();
        meshCharacterLegFL = GameObject.Find("LegL").GetComponent<MeshFilter>();
        meshCharacterLegFR = GameObject.Find("LegR").GetComponent<MeshFilter>();
        meshCharacterLegBL = GameObject.Find("LegBL").GetComponent<MeshFilter>();
        meshCharacterLegBR = GameObject.Find("LegBR").GetComponent<MeshFilter>();
        meshCharacterTail = GameObject.Find("Tail").GetComponent<MeshFilter>();
        meshCharacterWingL = GameObject.Find("WingL").GetComponent<MeshFilter>();
        meshCharacterWingR = GameObject.Find("WingR").GetComponent<MeshFilter>();

        //cat meshes
        meshCatBody = GameAssets.i.catBody;
        meshCatHead = GameAssets.i.catHead;
        meshCatLegFL = GameAssets.i.catLegFL;
        meshCatLegFR = GameAssets.i.catLegFR;
        meshCatLegBL = GameAssets.i.catLegBL;
        meshCatLegBR = GameAssets.i.catLegBR;

        //chicken meshes
        meshChickenBody = GameAssets.i.chickenBody;
        meshChickenHead = GameAssets.i.chickenHead;
        meshChickenLegL = GameAssets.i.chickenLegL;
        meshChickenLegR = GameAssets.i.chickenLegR;

        //dog meshes
        meshDogBody = GameAssets.i.dogBody;
        meshDogHead = GameAssets.i.dogHead;
        meshDogLegFL = GameAssets.i.dogLegFL;
        meshDogLegFR = GameAssets.i.dogLegFR;
        meshDogLegBL = GameAssets.i.dogLegBL;
        meshDogLegBR = GameAssets.i.dogLegBR;
        meshDogTail = GameAssets.i.dogTail;

        //lion meshes
        meshLionBody = GameAssets.i.lionBody;
        meshLionHead = GameAssets.i.lionHead;
        meshLionLegFL = GameAssets.i.lionLegFL;
        meshLionLegFR = GameAssets.i.lionLegFR;
        meshLionLegBL = GameAssets.i.lionLegBL;
        meshLionLegBR = GameAssets.i.lionLegBR;
        meshLionTail = GameAssets.i.lionTail;

        //penguin meshes
        meshPenguinBody = GameAssets.i.penguinBody;
        meshPenguinWingL = GameAssets.i.penguinWingL;
        meshPenguinWingR = GameAssets.i.penguinWingR;
    }

    private void SetCharacterCat()
    {
        PlayerPrefs.SetString("CharacterModel", "Cat");

        DisableWing();

        EnableBackLegs();
        EnableFrontLegs();
        EnableHead();

        //load animations
        anim.runtimeAnimatorController = null;

        //load mesh onto player character
        meshCharacterBody.mesh = meshCatBody.sharedMesh;
        //meshCharacterBody.transform.localPosition = new Vector3(-0.2522086f, 0.4361439f, 0.442086f);

        meshCharacterHead.mesh = meshCatHead.sharedMesh;
        //meshCharacterHead.transform.localPosition = new Vector3(-0.3441231f, 0.9279511f, 0.7460315f);

        meshCharacterLegFL.mesh = meshCatLegFL.sharedMesh;
        meshCharacterLegFL.transform.localPosition = new Vector3(-0.2511246f, 0.4101753f, 0.3453283f);

        meshCharacterLegFR.mesh = meshCatLegFR.sharedMesh;
        meshCharacterLegFR.transform.localPosition = new Vector3(0.2500359f, 0.4062789f, 0.3417089f);

        meshCharacterLegBL.mesh = meshCatLegBL.sharedMesh;
        meshCharacterLegBL.transform.localPosition = new Vector3(-0.2511246f, 0.4102064f, -0.2544346f);

        meshCharacterLegBR.mesh = meshCatLegBR.sharedMesh;
        meshCharacterLegBR.transform.localPosition = new Vector3(0.2500359f, 0.4062789f, -0.2567908f);

        anim.runtimeAnimatorController = GameAssets.i.animatorCat;
    }

    private void SetCharacterChicken()
    {
        PlayerPrefs.SetString("CharacterModel", "Chicken");

        DisableTail();
        DisableWing();
        DisableBackLegs();

        EnableFrontLegs();
        EnableHead();

        anim.runtimeAnimatorController = null;

        //load mesh onto player character
        meshCharacterBody.mesh = meshChickenBody.sharedMesh;
        //meshCharacterBody.transform.localPosition = new Vector3(-0.2897395f, 0.4951811f, 0.3094809f);

        meshCharacterHead.mesh = meshChickenHead.sharedMesh;
        //meshCharacterHead.transform.localPosition = new Vector3(-0.2966708f, 0.8899453f, 0.3055043f);

        meshCharacterLegFL.mesh = meshChickenLegL.sharedMesh;
        meshCharacterLegFL.transform.localPosition = new Vector3(-0.2963808f, 0.499345f, 0.004138455f);

        meshCharacterLegFR.mesh = meshChickenLegR.sharedMesh;
        meshCharacterLegFR.transform.localPosition = new Vector3(0.3036214f, 0.5010772f, 0.00832437f);

        //load animations
        anim.runtimeAnimatorController = GameAssets.i.animatorChicken;
    }
    private void SetCharacterDog()
    {
        PlayerPrefs.SetString("CharacterModel", "Dog");

        DisableWing();

        EnableTail();
        EnableBackLegs();
        EnableFrontLegs();
        EnableHead();

        anim.runtimeAnimatorController = null;

        //load mesh onto player character
        meshCharacterBody.mesh = meshDogBody.sharedMesh;
        meshCharacterHead.mesh = meshDogHead.sharedMesh;
        meshCharacterLegFL.mesh = meshDogLegFL.sharedMesh;
        meshCharacterLegFL.transform.localPosition = new Vector3(-0.2411f, 0.3063f, 0.196f);
        meshCharacterLegFR.mesh = meshDogLegFR.sharedMesh;
        meshCharacterLegFR.transform.localPosition = new Vector3(0.154f, 0.3063f, 0.196f);
        meshCharacterLegBL.mesh = meshDogLegBL.sharedMesh;
        meshCharacterLegBL.transform.localPosition = new Vector3(-0.3437675f, 0.3890306f, -0.366496f);
        meshCharacterLegBR.mesh = meshDogLegBR.sharedMesh;
        meshCharacterLegBR.transform.localPosition = new Vector3(0.3562458f, 0.4031368f, -0.3828406f);
        meshCharacterTail.mesh = meshDogTail.sharedMesh;

        //load animations
        anim.runtimeAnimatorController = GameAssets.i.animatorDog;
    }
    private void SetCharacterLion()
    {
        PlayerPrefs.SetString("CharacterModel", "Lion");

        DisableWing();

        EnableTail();
        EnableBackLegs();
        EnableFrontLegs();
        EnableHead();

        anim.runtimeAnimatorController = null;

        //load mesh onto player character
        meshCharacterBody.mesh = meshLionBody.sharedMesh;
        meshCharacterHead.mesh = meshLionHead.sharedMesh;
        meshCharacterLegFL.mesh = meshLionLegFL.sharedMesh;
        meshCharacterLegFL.transform.localPosition = new Vector3(-0.3288f, 1.12f, 0.351f);
        meshCharacterLegFR.mesh = meshLionLegFR.sharedMesh;
        meshCharacterLegFR.transform.localPosition = new Vector3(-0.3319f, 1.12f, 0.351f);
        meshCharacterLegBL.mesh = meshLionLegBL.sharedMesh;
        meshCharacterLegBL.transform.localPosition = new Vector3(-0.3288f, 1.12f, 0.514f);
        meshCharacterLegBR.mesh = meshLionLegBR.sharedMesh;
        meshCharacterLegBR.transform.localPosition = new Vector3(-0.3319f, 1.12f, 0.514f);
        meshCharacterTail.mesh = meshLionTail.sharedMesh;

        //load animations
        anim.runtimeAnimatorController = GameAssets.i.animatorLion;
    }
    private void SetCharacterPenguin()
    {
        PlayerPrefs.SetString("CharacterModel", "Penguin");

        DisableTail();
        DisableFrontLegs();
        DisableBackLegs();
        DisableHead();

        EnableWings();

        anim.runtimeAnimatorController = null;

        //load mesh onto player character
        meshCharacterBody.mesh = meshPenguinBody.sharedMesh;
        meshCharacterWingL.mesh = meshPenguinWingL.sharedMesh;
        meshCharacterWingL.transform.localPosition = new Vector3(-0.5541989f, 0.8080121f, 0.07016902f);
        meshCharacterWingR.mesh = meshPenguinWingR.sharedMesh;
        meshCharacterWingR.transform.localPosition = new Vector3(0.5488839f, 0.8143129f, 0.07041698f);

        //load animations
        anim.runtimeAnimatorController = GameAssets.i.animatorPenguin;
    }

    private void DisableHead()
    {
        if (meshCharacterHead.gameObject.activeInHierarchy == true)
        {
            meshCharacterHead.gameObject.SetActive(false);
        }
    }

    private void DisableWing()
    {
        if (meshCharacterWingL.gameObject.activeInHierarchy == true || meshCharacterWingR.gameObject.activeInHierarchy == true)
        {
            meshCharacterWingL.gameObject.SetActive(false);
            meshCharacterWingR.gameObject.SetActive(false);
        }
    }

    private void DisableTail()
    {
        if (meshCharacterTail.gameObject.activeInHierarchy == true)
        {
            meshCharacterTail.gameObject.SetActive(false);
        }
    }

    private void DisableFrontLegs()
    {
        if (meshCharacterLegFL.gameObject.activeInHierarchy == true || meshCharacterLegFR.gameObject.activeInHierarchy == true)
        {
            meshCharacterLegFL.gameObject.SetActive(false);
            meshCharacterLegFR.gameObject.SetActive(false);
        }
    }

    private void DisableBackLegs()
    {
        if (meshCharacterLegBL.gameObject.activeInHierarchy == true || meshCharacterLegBR.gameObject.activeInHierarchy == true)
        {
            meshCharacterLegBL.gameObject.SetActive(false);
            meshCharacterLegBR.gameObject.SetActive(false);
        }
    }

    private void EnableHead()
    {
        if (meshCharacterHead.gameObject.activeInHierarchy == false)
        {
            meshCharacterHead.gameObject.SetActive(true);
        }
    }

    private void EnableWings()
    {
        if (meshCharacterWingL.gameObject.activeInHierarchy == false || meshCharacterWingR.gameObject.activeInHierarchy == false)
        {
            meshCharacterWingL.gameObject.SetActive(true);
            meshCharacterWingR.gameObject.SetActive(true);
        }
    }

    private void EnableTail()
    {
        if (meshCharacterTail.gameObject.activeInHierarchy == false)
        {
            meshCharacterTail.gameObject.SetActive(true);
        }
    }

    private void EnableBackLegs()
    {
        if (meshCharacterLegBL.gameObject.activeInHierarchy == false || meshCharacterLegBR.gameObject.activeInHierarchy == false)
        {
            meshCharacterLegBL.gameObject.SetActive(true);
            meshCharacterLegBR.gameObject.SetActive(true);
        }
    }

    private void EnableFrontLegs()
    {
        if (meshCharacterLegFL.gameObject.activeInHierarchy == false || meshCharacterLegFR.gameObject.activeInHierarchy == false)
        {
            meshCharacterLegFL.gameObject.SetActive(true);
            meshCharacterLegFR.gameObject.SetActive(true);
        }
    }


}
