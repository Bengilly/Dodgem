using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    private readonly float bounceForce = 5.0f;
    private readonly float speed = 4;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GameScreen)
        {
            MoveEnemy();
        }

    }

    private void MoveEnemy()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //bounce off edges//
        if (collision.gameObject.CompareTag("TopWall"))
        {
            enemyRb.AddForce(-Vector3.forward * bounceForce, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("BottomWall"))
        {
            enemyRb.AddForce(Vector3.forward * bounceForce, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("LeftWall"))
        {
            enemyRb.AddForce(Vector3.right * bounceForce, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("RightWall"))
        {
            enemyRb.AddForce(-Vector3.right * bounceForce, ForceMode.Impulse);
        }

    }

}
