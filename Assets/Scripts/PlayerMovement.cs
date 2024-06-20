using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] float swimSpeed = 4;
    [SerializeField] Vector2 deathJump = new Vector2(10.0f, 10.0f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    float gravityAtStart;
    bool isAlive = true;
    bool isSwimming = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        SetSwimming();
        Run();
        Swim();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void SetSwimming()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Waters")))
        {
            isSwimming = true;
        }
        else
        {
            isSwimming = false;
        }
    }

    void Run()
    {
        if (!isSwimming)
        {
            Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;

            bool playerIsMovingHorizontally = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

            myAnimator.SetBool("isRunning", playerIsMovingHorizontally);
        }
    }

    void Swim()
    {
        if (isSwimming)
        {
            Debug.Log("Swimming");
            Vector2 swimVelocity = new Vector2(moveInput.x * swimSpeed, moveInput.y * swimSpeed);
            myRigidbody.velocity = swimVelocity;
        }
    }    
    void FlipSprite()
    {
        bool playerIsMovingHorizontally = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerIsMovingHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        bool playerIsClimbing = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerIsClimbing);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("isDead");
            myRigidbody.velocity = deathJump;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
