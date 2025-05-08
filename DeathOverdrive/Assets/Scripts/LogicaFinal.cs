using UnityEngine;
using System.Collections;


public class LogicaFinal : MonoBehaviour
{
    public GameObject panelCristal;
    public GameObject panelContinuara;
    public GameObject tilemapObject;

    public CameraShake cameraShake; // Asigna aquí el script de la cámara

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.3f;

    public void CambiarDestino()
    {
        if (panelCristal != null) panelCristal.SetActive(false);
        if (panelContinuara != null) panelContinuara.SetActive(true);
        Debug.Log("El jugador decidió cambiar su destino. Mostrando panel 'Continuará...'");
    }

 public void DestruirCristal()
{
    // Primero asegurarse de ejecutar la corutina antes de desactivar el panel
    if (cameraShake != null)
    {
        if (cameraShake.gameObject.activeInHierarchy)
        {
            // Ejecutar la corutina de shake
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
        }
        else
        {
            Debug.LogWarning("El objeto que contiene CameraShake no está activo. No se puede ejecutar la corutina.");
        }
    }
    else
    {
        Debug.LogWarning("CameraShake no asignado.");
    }

    // Desactivar el panel de cristal después de un pequeño retraso (si se necesita)
    if (panelCristal != null)
    {
        StartCoroutine(DesactivarPanelConRetraso());
    }

    // Destruir el Tilemap
    if (tilemapObject != null)
    {
        Destroy(tilemapObject);
        Debug.Log("Tilemap destruido junto con el cristal.");
    }
}

// Corutina para desactivar el panel con un pequeño retraso
private IEnumerator DesactivarPanelConRetraso()
{
    yield return new WaitForSeconds(0.1f);  // Pequeño retraso para asegurar que la corutina de shake termine
    panelCristal.SetActive(false);
    Debug.Log("Panel de cristal desactivado.");
}

}
