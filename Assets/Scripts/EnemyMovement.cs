using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float moveSpeed = 1.0f;

    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyDirection();
    }

    void FlipEnemyDirection()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1.0f);
    }

    public bool GetAlive()
    {
        return isAlive;
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }
}
