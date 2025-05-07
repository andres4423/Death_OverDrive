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
        Debug.Log(gameObject.name + " ha muerto.");

        // Instanciar el cristal en la posición del objeto

        // Destruir este objeto
        Destroy(gameObject);
    }
}
