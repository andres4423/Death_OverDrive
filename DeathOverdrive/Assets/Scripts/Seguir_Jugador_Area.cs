using UnityEngine;
using System.Collections;

public class Seguir_Jugador_Area : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    public float distanciaMaxima;
    public Vector3 puntoInicial;
    public bool mirandoDerecha;
    private Animator animator;
    public EstadosMovimiento estadoActual;
    public float distanciaAtaque = 1.0f;
    public float tiempoEntreAtaques = 3f;
    private bool puedeAtacar = true;

    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Atacando,
        Volviendo
    }

    public void Start()
    {
        animator = GetComponent<Animator>();

        if (transformJugador == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                transformJugador = jugador.transform;
            }
            else
            {
                Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
            }
        }

        puntoInicial = transform.position;
    }

    void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Atacando:
                EstadoAtacando();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }
    }

    private void EstadoEsperando()
    {
        animator.SetBool("isWalkingG", false);

        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);

        if (jugadorCollider)
        {
            float diferenciaAltura = Mathf.Abs(jugadorCollider.transform.position.y - transform.position.y);

            if (diferenciaAltura < 0.5f) // Ajusta este valor según lo que consideres "misma altura"
            {
                transformJugador = jugadorCollider.transform;
                estadoActual = EstadosMovimiento.Siguiendo;
            }
        }
    }


    private void EstadoSiguiendo()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }

        float distancia = Vector2.Distance(transform.position, transformJugador.position);

        if (distancia <= distanciaAtaque)
        {
            estadoActual = EstadosMovimiento.Atacando;
            return;
        }

        // **Movemos al enemigo ajustando la velocidad si hay slow motion**
        float velocidadAjustada = velocidadMovimiento;
        if (SlowMotionManager.Instance != null && SlowMotionManager.Instance.IsInSlowMotion)
        {
            velocidadAjustada *= 0.5f; // Ajusta este valor para sincronizarlo
        }

        Vector3 objetivoPosicion = new Vector3(transformJugador.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, objetivoPosicion, velocidadAjustada * Time.deltaTime);
        animator.SetBool("isWalkingG", true);
        animator.SetBool("isAttackingG", false);

        float diferenciaAltura = Mathf.Abs(transformJugador.position.y - transform.position.y);

        GirarAObjetivo(transformJugador.position);

        if (diferenciaAltura > 0.5f)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
            return;
        }

        if (distancia > distanciaMaxima)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoAtacando()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            animator.SetBool("isAttackingG", false);
            return;
        }

        animator.SetBool("isWalkingG", false);
        animator.SetBool("isAttackingG", true);

        if (puedeAtacar)
        {
            puedeAtacar = false;
            Vida vidaJugador = transformJugador.GetComponent<Vida>();
            if (vidaJugador != null)
            {
                vidaJugador.RecibirDanio(1);
            }

            // Cambiamos el Invoke por una corrutina que respete el slow motion
            StartCoroutine(ReiniciarAtaqueCoroutine());
        }

        float distancia = Vector2.Distance(transform.position, transformJugador.position);

        if (distancia > distanciaAtaque)
        {
            estadoActual = EstadosMovimiento.Siguiendo;
            animator.SetBool("isAttackingG", false);
        }
    }

    // Usamos una corrutina en vez de Invoke para respetar el slow motion
    IEnumerator ReiniciarAtaqueCoroutine()
    {
        float tiempoEspera = SlowMotionManager.Instance.IsInSlowMotion ? tiempoEntreAtaques * 2f : tiempoEntreAtaques;
        yield return new WaitForSecondsRealtime(tiempoEspera);
        puedeAtacar = true;
    }

    private void EstadoVolviendo()
    {
        Vector3 volverPosicion = new Vector3(puntoInicial.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, volverPosicion, velocidadMovimiento * Time.deltaTime);
        animator.SetBool("isWalkingG", true);

        GirarAObjetivo(puntoInicial);

        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            estadoActual = EstadosMovimiento.Esperando;
        }
    }

    private void GirarAObjetivo(Vector3 objetivo)
    {
        if (objetivo.x < transform.position.x && mirandoDerecha)
        {
            Girar();
        }
        else if (objetivo.x > transform.position.x && !mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void Morir()
    {
        animator.SetTrigger("die");  // Activar el Trigger para la animación de muerte
        GetComponent<Collider2D>().enabled = false; // Evitar colisiones durante animación
        this.enabled = false; // Desactiva el script para que deje de moverse

        StartCoroutine(DestruirDespuesDeAnimacion());
    }

    IEnumerator DestruirDespuesDeAnimacion()
    {
        yield return new WaitForSeconds(1.25f); // Espera la animación de muerte
        Destroy(gameObject); // Destruye al enemigo
    }


}