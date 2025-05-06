using UnityEngine;

public class Vida : MonoBehaviour
{
    public int vidaMaxima = 3;
    private int vidaActual;

    public bool esJugador = false; // ← Añadido

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;

        Debug.Log(gameObject.name + " recibió daño. Vida restante: " + vidaActual);

        if (esJugador)
        {
            VidaJugadorManager.Instance?.RecibirDanio(cantidad);
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
<<<<<<< Updated upstream
        Debug.Log(gameObject.name + " ha muerto.");

        Seguir_Jugador_Area enemigo = GetComponent<Seguir_Jugador_Area>();
        if (enemigo != null)
        {
            enemigo.Morir(); // Usa la animación y luego destruye
        }
        else
        {
            Destroy(gameObject); // Para otros objetos sin animaciones especiales
=======
        if (!esJugador)
        {
            Destroy(gameObject);
        }
        else
        {
            // El jugador no se destruye aquí
            Debug.Log("Jugador muerto (controlado por VidaJugadorManager)");
>>>>>>> Stashed changes
        }
    }
}
