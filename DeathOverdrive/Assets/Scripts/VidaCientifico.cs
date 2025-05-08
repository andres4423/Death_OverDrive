using UnityEngine;

public class VidaCientifico : MonoBehaviour
{
    public int vidaMaxima = 3;
    private int vidaActual;

    

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log(gameObject.name + " recibió daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (TryGetComponent<Seguir_Jugador_Area>(out var cientifico))
        {
            cientifico.transformJugador = null;
            cientifico.Morir(); // ✅ Llama al método Morir() del enemigo para animación + delay
        }
        else
        {
            Destroy(gameObject); // Solo destruye directamente si NO es enemigo (por ejemplo, otros objetos)
        }
    }
}
