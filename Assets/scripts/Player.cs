using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float movementX;
    float movementY;
    [SerializeField] float speed = 100;
    [SerializeField] Rigidbody2D rb; //SerializeField allows us to make changes quickly by showing up in inspector
    [SerializeField] float jumpPower = 1f; //unity editor is final, overrides Player.cs script 
    bool jumping = false;
    bool touchingGround;

    private Animator animator;
    bool facingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Look for RigidBody2D and set it to rb
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("walking", movementX != 0f);
        animator.SetBool("jumping", !touchingGround);
    }
    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();

        movementX = v.x;
        movementY = v.y;

        Debug.Log(v);
    }

    void OnJump()
    {
        if (touchingGround)
        {
            jumping = true;
        }
    }
    
    void FixedUpdate()
    {
        float XmoveDistance = movementX * speed;
        //float YmoveDistance = movementY * speed * Time.fixedDeltaTime;
        //transform.position = new Vector2(transform.position.x + XmoveDistance, transform.position.y + YmoveDistance);

        rb.linearVelocityX = XmoveDistance; //can be cancelled out, not as good as AddForce if you want to add knockback etc.

        {
        //handle jumping
        if (touchingGround && jumping)
            rb.AddForce(jumpPower * Vector2.up, ForceMode2D.Impulse);
            jumping = false;
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("ground")) {
        //     rb.AddForce(new Vector2(0, 500));
        // }
        if (collision.gameObject.CompareTag("ground"))
        {
            touchingGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            touchingGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
        }
        
    }
}
