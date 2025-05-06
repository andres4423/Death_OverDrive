using UnityEngine;

public class VidaJugadorManager : MonoBehaviour
{
    public static VidaJugadorManager Instance { get; private set; }

    public int vidaMaxima = 3;
    public int vidaActual;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre escenas
            vidaActual = vidaMaxima;
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados
        }
    }

    public void RecibirDanio(int cantidad)
    {
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
        // Aquí puedes activar una pantalla de Game Over, reiniciar nivel, etc.
    }
}
