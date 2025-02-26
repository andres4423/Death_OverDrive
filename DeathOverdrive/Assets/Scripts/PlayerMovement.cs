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

        // Movimiento horizontal
        if (move != 0)
        {
            rigidBody.linearVelocity = new Vector2(speed * move, rigidBody.linearVelocity.y);
            animator.SetFloat("speed", Mathf.Abs(move));
            animator.SetBool("IsRunning", true);
            spriteRenderer.flipX = move < 0;
        }
        else
        {
            // Detener el personaje cuando no se presiona dirección
          
            animator.SetBool("IsRunning", false);
            animator.SetFloat("speed", 0);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }

        // Animación de caída
        if (rigidBody.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }
}
