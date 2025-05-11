using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Componentes
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private ParticleSystem particulas;

    // Movimiento básico
    private float speed = 3f;
    private float jumpForce = 6.4f;

    // GroundCheck variables
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool wasGrounded;

    // Dash
    public float dashSpeed = 8f;
    public float dashTime = 0.2f;
    private bool isDashing = false;
    private float lastDashTime;
    public float dashCooldown = 1f;

    // Ataque
    private bool isAttacking = false;
    public float attackDuration = 0.15f;

    // Sonidos
    public AudioClip dashSound;
    [Range(0, 1)] public float dashVolume = 0.5f;
    public AudioClip landingSound;
    [Range(0, 1)] public float landingVolume = 0.4f;

    // Paneles
    public GameObject panelCristal;
    public GameObject panelFin;

    // Slow Motion
    public float slowMotionFactor = 0.3f;
    private bool isInSlowMotion = false;
    public bool IsInvincible { get; private set; } = false;

    void Start()
    {
        Time.timeScale = 1f;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particulas = GetComponentInChildren<ParticleSystem>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        wasGrounded = true;
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

        // === MOVIMIENTO ===
        float move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            rigidBody.linearVelocity = new Vector2(speed * move, rigidBody.linearVelocity.y);
            animator.SetFloat("speed", Mathf.Abs(move));
            animator.SetBool("IsRunning", true);
            spriteRenderer.flipX = move < 0;

            if (isInSlowMotion)
            {
                Vector3 movimiento = new Vector3(move * speed, 0, 0) * Time.unscaledDeltaTime;
                transform.Translate(movimiento);
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(move * speed, rigidBody.linearVelocity.y);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetFloat("speed", 0);

            if (!isInSlowMotion)
                rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
        }

        // === SALTO ===
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            wasGrounded = false;
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }

        if (rigidBody.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }

        // === DASH ===
        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastDashTime + dashCooldown)
        {
            float direction = spriteRenderer.flipX ? -1f : 1f;

            Vector3 particlePosition = particulas.transform.localPosition;
            particlePosition.x = direction < 0 ? Mathf.Abs(particlePosition.x) : -Mathf.Abs(particlePosition.x);
            particulas.transform.localPosition = particlePosition;

            particulas.Play();

            if (dashSound != null)
            {
                audioSource.PlayOneShot(dashSound, dashVolume);
            }
            StartCoroutine(DashCoroutine());
        }

        // === ATAQUE ===
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackCoroutine());
        }

        // === SLOW MOTION ===
        if (VidaJugadorManager.Instance != null && VidaJugadorManager.Instance.vidaActual == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isInSlowMotion)
            {
                ActivateSlowMotion();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && isInSlowMotion)
            {
                DeactivateSlowMotion();
            }
        }

        // === CAÍDA DEL MAPA ===
        if (transform.position.y < -10f)
        {
            MostrarPanelFin();
        }
    }

    // === DASH ===
    IEnumerator DashCoroutine()
    {
        isDashing = true;
        IsInvincible = true;
        lastDashTime = Time.time;
        animator.SetTrigger("dash");

        float direction = spriteRenderer.flipX ? -1f : 1f;
        rigidBody.linearVelocity = new Vector2(direction * dashSpeed, 0);

        float dashDuration = isInSlowMotion ? dashTime / slowMotionFactor : dashTime;
        yield return new WaitForSecondsRealtime(dashDuration);

        isDashing = false;
        IsInvincible = false;
    }

    // === ATAQUE ===
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetBool("attack", true);

        Collider2D enemigo = Physics2D.OverlapCircle(transform.position, 1.0f, LayerMask.GetMask("Enemigo", "Cientifico"));

        if (enemigo != null)
        {
            Vida vidaEnemigo = enemigo.GetComponent<Vida>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDanio(3);
            }
            else
            {
                VidaCientifico vidaCientifico = enemigo.GetComponent<VidaCientifico>();
                if (vidaCientifico != null)
                {
                    vidaCientifico.RecibirDanio(3);
                }
            }
        }

        float attackWait = isInSlowMotion ? attackDuration * slowMotionFactor : attackDuration;
        yield return new WaitForSeconds(attackWait);

        animator.SetBool("attack", false);
        isAttacking = false;
    }

    // === SLOW MOTION CONTROL ===
    void ActivateSlowMotion()
    {
        SlowMotionManager.Instance.ActivateSlowMotion();
        isInSlowMotion = true;
    }

    void DeactivateSlowMotion()
    {
        SlowMotionManager.Instance.DeactivateSlowMotion();
        isInSlowMotion = false;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        isGrounded = false;
        wasGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        foreach (ContactPoint2D contact in coll.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                if (!wasGrounded && landingSound != null)
                {
                    audioSource.PlayOneShot(landingSound, landingVolume);
                }

                isGrounded = true;
                wasGrounded = true;
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cristal"))
        {
            panelCristal.SetActive(true);
            Debug.Log("Panel activado por contacto con cristal.");
        }
    }

    // === MOSTRAR PANEL FIN ===
    void MostrarPanelFin()
    {
        if (panelFin != null && !panelFin.activeSelf)
        {
            panelFin.SetActive(true);
            Debug.Log("El jugador cayó del mapa. Mostrando panel de fin.");

            rigidBody.linearVelocity = Vector2.zero;
            this.enabled = false; // Desactiva el control del jugador

            // Opcional: detener el tiempo
            // Time.timeScale = 0f;
        }
    }
}
