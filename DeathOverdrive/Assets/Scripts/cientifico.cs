using UnityEngine;
using System.Collections;


public class cientifico : MonoBehaviour
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
    private bool hasDied = false;
    public GameObject cristal; // Aquí usas el nombre "cristal"
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

        if (Input.GetKeyDown(KeyCode.K) && !hasDied)
        {
            animator.SetTrigger("die");
            animator.SetBool("isWalkingG", false); // <--- Esta línea es la que necesitas
            animator.SetBool("isAttackingG", false); // <--- Esta línea es la que necesitas
            hasDied = true;
            StartCoroutine(DestruirDespuesDeAnimacion());
            Instantiate(cristal, transform.position, Quaternion.identity);
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
        Vector3 objetivoPosicion = new Vector3(transformJugador.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, objetivoPosicion, velocidadMovimiento * Time.deltaTime);
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
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Esperando;
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }

    public void Morir()
    {
        Debug.Log("Morí");
        animator.SetTrigger("die");  
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestruirDespuesDeAnimacion());
        Instantiate(cristal, transform.position, Quaternion.identity);

    }

    IEnumerator DestruirDespuesDeAnimacion()
    {
        yield return new WaitForSeconds(1.25f);
        this.enabled = false;
        Destroy(gameObject);
    }

    


}