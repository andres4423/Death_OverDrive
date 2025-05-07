using UnityEngine;

public class LogicaFinal : MonoBehaviour
{
    public GameObject panelCristal;
    public GameObject panelContinuara;

    // Este método lo asignarás al botón de "Cambiar el destino"
    public void CambiarDestino()
    {
        if (panelCristal != null) panelCristal.SetActive(false);
        if (panelContinuara != null) panelContinuara.SetActive(true);
        Debug.Log("El jugador decidió cambiar su destino. Mostrando panel 'Continuará...'");
    }
}
