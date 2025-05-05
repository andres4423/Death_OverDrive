using UnityEngine;

public class Vida : MonoBehaviour
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
        Debug.Log(gameObject.name + " ha muerto.");

        Seguir_Jugador_Area enemigo = GetComponent<Seguir_Jugador_Area>();
        if (enemigo != null)
        {
            enemigo.Morir(); // Usa la animación y luego destruye
        }
        else
        {
            Destroy(gameObject); // Para otros objetos sin animaciones especiales
        }
    }
}
