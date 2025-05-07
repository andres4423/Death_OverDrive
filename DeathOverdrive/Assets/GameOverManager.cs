using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Necesario para Event Trigger

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject player;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null || !player.activeInHierarchy)
        {
            ShowGameOver();
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    // Llamado desde el Event Trigger (Click en "Reiniciar")
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }

    // Llamado desde el Event Trigger (Click en "Salir")
    public void QuitGame()
    {
        Time.timeScale = 1f;

        // Si tienes un menú principal, carga su escena:
        // SceneManager.LoadScene("MenuPrincipal");

        // Si quieres cerrar la aplicación (en build):
        Application.Quit();

        // Si estás en el Editor, esto no hará nada, pero puedes debuguearlo:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}