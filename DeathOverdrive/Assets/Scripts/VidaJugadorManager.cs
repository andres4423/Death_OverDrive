using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaJugadorManager : MonoBehaviour
{
    public static VidaJugadorManager Instance { get; private set; }
    public PlayerMovement playerMovement;
    public int vidaMaxima = 3;
    public int vidaActual;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre escenas
            vidaActual = vidaMaxima;
            SceneManager.sceneLoaded += HandleSceneLoaded; // 🔄 Escuchar cuando se carga una escena
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded; // 🔄 Eliminar suscripción
    }

    // 🔄 Método que se llama cada vez que una escena se carga
    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Restaurar vida al máximo
        vidaActual = vidaMaxima;

        // Buscar y asignar el PlayerMovement
        GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
        if (jugadorGO != null)
        {
            playerMovement = jugadorGO.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto con tag 'Player' en la nueva escena.");
        }
    }


    public void RecibirDanio(int cantidad)
    {

        if (playerMovement != null && playerMovement.IsInvincible)
        {
            Debug.Log("El jugador es invencible y no recibe daño.");
            return;
        }

        vidaActual -= cantidad;
        
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
    }

    public void MejorarVida(int cantidad)
    {
        vidaMaxima += cantidad;
        vidaActual = vidaMaxima; // Restaurar vida al máximo
        Debug.Log("Vida mejorada. Nueva vida máxima: " + vidaMaxima);
    }

}
