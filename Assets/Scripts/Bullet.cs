using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20.0f;
    PlayerMovement player;
    Rigidbody2D myRigidbody;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;   
    }


    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0.0f);
        transform.localScale = new Vector2(Mathf.Sign(xSpeed), 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.gameObject.GetComponent<EnemyMovement>().GetAlive())
        {
            collision.gameObject.GetComponent<EnemyMovement>().SetAlive(false);
            Destroy(collision.gameObject);
            FindObjectOfType<GameSession>().AddScore(50);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
