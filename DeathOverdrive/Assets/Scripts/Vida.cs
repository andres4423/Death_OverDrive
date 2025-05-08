using UnityEngine;

public class Vida : MonoBehaviour
{
    public bool esJugador = false;

    [Header("Configuración de Vida")]
    public int vidaMaxima = 10;
    private int vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(int cantidad)
    {
        Debug.Log(gameObject.name + " recibió daño: " + cantidad);

        if (esJugador)
        {
            VidaJugadorManager.Instance?.RecibirDanio(cantidad);

            if (VidaJugadorManager.Instance.vidaActual <= 0)
            {
                Morir();
            }
        }
        else
        {
            vidaActual -= cantidad;
            Debug.Log(gameObject.name + " vida restante: " + vidaActual);

            if (vidaActual <= 0)
            {
                Morir();
            }
        }
    }

    private void Morir()
    {
        if (TryGetComponent<Seguir_Jugador_Area>(out var enemigo))
        {
            enemigo.transformJugador = null;
            enemigo.Morir();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
