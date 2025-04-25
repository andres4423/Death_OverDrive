using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ¡No olvides este using!
using System.Collections;

public class CambioConHistoria : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject panelHistoria; 
    public TextMeshProUGUI textoTMP; // Referencia al TextMeshProUGUI
    public int idEscena = 1;
    public float velocidadTexto = 0.05f;

    private bool mostrarHistoria = false;
    private bool _textoCompletado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !mostrarHistoria)
        {
            panelHistoria.SetActive(true);
            mostrarHistoria = true;
            Time.timeScale = 0f;
            StartCoroutine(MostrarTextoProgresivamente());
        }
    }

    private IEnumerator MostrarTextoProgresivamente()
    {
        string textoCompleto = textoTMP.text; // Usa textoTMP en lugar de textoHistoria
        textoTMP.text = "";

        for (int i = 0; i <= textoCompleto.Length; i++)
        {
            textoTMP.text = textoCompleto.Substring(0, i);
            yield return new WaitForSecondsRealtime(velocidadTexto);
        }

        _textoCompletado = true;
    }

    private void Update()
    {
        // Opción para saltar el texto con Space
        if (mostrarHistoria && !_textoCompletado && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            textoTMP.text = textoTMP.text; // Fuerza mostrar todo el texto
            _textoCompletado = true;
        }

        // Confirmar y cambiar escena con Enter
        if (mostrarHistoria && _textoCompletado && Input.GetKeyDown(KeyCode.Return))
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