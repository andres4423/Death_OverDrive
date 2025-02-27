using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float speed = 2f;
    private float jumpForce = 5f;
    private bool isGrounded;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            rigidBody.linearVelocity = new Vector2(speed * move, rigidBody.linearVelocity.y);
            animator.SetFloat("speed", Mathf.Abs(move));
            animator.SetBool("IsRunning", true);
            spriteRenderer.flipX = move < 0;
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetFloat("speed", 0);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            animator.SetBool("IsFalling", false);
        }

        if (rigidBody.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        foreach (ContactPoint2D contact in coll.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
                return;
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        isGrounded = false;
    }
}
