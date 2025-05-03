using UnityEngine;

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
    public float distanciaAtaque = 1.0f; // O prueba con 1.5f o más
    public float tiempoEntreAtaques = 3f; // Tiempo de espera entre ataques
    private bool puedeAtacar = true; // Controla el tiempo de ataque

    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Atacando,
        Volviendo
    }

    public void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator del enemigo

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
            case EstadosMovimiento.Atacando: // ¡Asegúrate de incluir esto!
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
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
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


        // Movimiento y animación de caminar
        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);
        animator.SetBool("isWalkingG", true);
        animator.SetBool("isAttackingG", false);

        GirarAObjetivo(transformJugador.position);

        if (distancia > distanciaMaxima)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoAtacando()
    {
        animator.SetBool("isWalkingG", false);
        animator.SetBool("isAttackingG", true);

        if (puedeAtacar)
        {
            puedeAtacar = false;

            // Verifica si el jugador tiene un script de vida y le aplica daño
            Vida vidaJugador = transformJugador.GetComponent<Vida>();
            if (vidaJugador != null)
            {
                vidaJugador.RecibirDanio(1);
            }

            Invoke("ReiniciarAtaque", tiempoEntreAtaques);
        }

        float distancia = Vector2.Distance(transform.position, transformJugador.position);

        if (distancia > distanciaAtaque)
        {
            estadoActual = EstadosMovimiento.Siguiendo;
            animator.SetBool("isAttackingG", false);
        }
    }


    private void ReiniciarAtaque()
    {
        puedeAtacar = true;
    }



    private void EstadoVolviendo()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
