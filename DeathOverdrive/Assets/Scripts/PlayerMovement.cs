using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float speed = 3f;
    private float jumpForce = 6.4f;

    // GroundCheck variables
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
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

    private ParticleSystem particulas;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particulas = GetComponentInChildren<ParticleSystem>(); 
    }

    void Update()
    {
        // Revisión del suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }


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
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }

        if (rigidBody.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }

        // DASH
        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastDashTime + dashCooldown)
        {
            float direction = spriteRenderer.flipX ? -1f : 1f;

            Vector3 particlePosition = particulas.transform.localPosition;
            particlePosition.x = direction < 0 ? Mathf.Abs(particlePosition.x) : -Mathf.Abs(particlePosition.x);
            particulas.transform.localPosition = particlePosition;

            particulas.Play();

            StartCoroutine(DashCoroutine());
        }

        // ATAQUE
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        animator.SetTrigger("dash");

        float direction = spriteRenderer.flipX ? -1f : 1f;
        rigidBody.linearVelocity = new Vector2(direction * dashSpeed, 0);

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
    }

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
                vidaEnemigo.RecibirDanio(3);
            }
        }

        yield return new WaitForSeconds(attackDuration);

        animator.SetBool("attack", false);
        isAttacking = false;
    }

    // Dibuja el círculo de detección en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}