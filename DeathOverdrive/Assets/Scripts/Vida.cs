using UnityEngine;

public class Vida : MonoBehaviour
{
    public bool esJugador = false;

    public void RecibirDanio(int cantidad)
    {
        Debug.Log(gameObject.name + " recibió daño.");

        if (esJugador)
        {
            // Delegar el daño al VidaJugadorManager
            VidaJugadorManager.Instance?.RecibirDanio(cantidad);

            // Verificar si ha muerto
            if (VidaJugadorManager.Instance.vidaActual <= 0)
            {
                Morir();
            }
        }
        else
        {
            // Aquí manejas el daño de enemigos u otros objetos
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        Seguir_Jugador_Area enemigo = GetComponent<Seguir_Jugador_Area>();
        if (enemigo != null)
        {
            enemigo.transformJugador = null; // Notificas al enemigo que el jugador ya no está
        }

        Destroy(gameObject);
    }

}