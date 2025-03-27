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
        Destroy(gameObject); // Destruye el objeto al morir
    }
}
