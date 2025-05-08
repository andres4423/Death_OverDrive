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
        if (TryGetComponent<Seguir_Jugador_Area>(out var enemigo))
        {
            enemigo.transformJugador = null;
            enemigo.Morir(); // ✅ Llama al método Morir() del enemigo para animación + delay
        }
        else
        {
            Destroy(gameObject); // Solo destruye directamente si NO es enemigo (por ejemplo, otros objetos)
        }
    }


}