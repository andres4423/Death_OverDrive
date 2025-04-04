using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float speed = 3f;
    private float jumpForce = 6.4f;
    private bool isGrounded;

    // Dash variables
    public float dashSpeed = 8f;
    public float dashTime = 0.2f;
    private bool isDashing = false;
    private float lastDashTime;
    public float dashCooldown = 1f;

    // Attack variables
    private bool isAttacking = false;
    public float attackDuration = 0.15f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDashing || isAttacking) return;

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
            animator.SetBool("isFalling", false);
        }

        if (rigidBody.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(DashCoroutine());
        }

        //ataque input
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    //animacion dash
    IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        // Activar el dash
        animator.SetTrigger("dash");

        float direction = spriteRenderer.flipX ? -1f : 1f;
        rigidBody.linearVelocity = new Vector2(direction * dashSpeed, 0);

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
    }

    //animacion ataque
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetBool("attack", true);

        Collider2D enemigo = Physics2D.OverlapCircle(transform.position, 1.0f, LayerMask.GetMask("Enemigo"));

        if (enemigo != null)
        {
            Vida vidaEnemigo = enemigo.GetComponent<Vida>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDanio(3); // Mata al enemigo con un solo golpe
            }
        }

        yield return new WaitForSeconds(attackDuration);

        animator.SetBool("attack", false);
        isAttacking = false;
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
