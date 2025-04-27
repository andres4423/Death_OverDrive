using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CambioConHistoria : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject panelHistoria; 
    public TextMeshProUGUI textoTMP; // Referencia al TextMeshProUGUI
    public int idEscena = 1;

    private bool mostrarHistoria = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !mostrarHistoria)
        {
            panelHistoria.SetActive(true);
            mostrarHistoria = true;
            Time.timeScale = 0f; // Pausa el juego
            
            // El texto ya aparece completo (sin typewriter)
            // No hay necesidad de corrutinas aquí
        }
    }

    private void Update()
    {
        // Confirmar y cambiar escena con Enter
        if (mostrarHistoria && Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            CargarEscenaPorID();
        }
    }

    private void CargarEscenaPorID()
    {
        if (idEscena >= 0 && idEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(idEscena);
        }
        else
        {
            Debug.LogError("ID de escena inválido. Revisa Build Settings.");
        }
    }
}