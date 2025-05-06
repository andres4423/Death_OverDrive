using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Para los eventos de UI

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public EventTrigger continueTrigger;
    public EventTrigger menuTrigger;
    public EventTrigger quitTrigger;

    void Start()
    {
        // Configurar eventos para el texto "Continuar"
        EventTrigger.Entry continueEntry = new EventTrigger.Entry();
        continueEntry.eventID = EventTriggerType.PointerClick;
        continueEntry.callback.AddListener((data) => { TogglePause(); });
        continueTrigger.triggers.Add(continueEntry);

        // Configurar eventos para el texto "Menú Principal"
        EventTrigger.Entry menuEntry = new EventTrigger.Entry();
        menuEntry.eventID = EventTriggerType.PointerClick;
        menuEntry.callback.AddListener((data) => { GoToMainMenu(); });
        menuTrigger.triggers.Add(menuEntry);

        // Configurar eventos para el texto "Salir"
        EventTrigger.Entry quitEntry = new EventTrigger.Entry();
        quitEntry.eventID = EventTriggerType.PointerClick;
        quitEntry.callback.AddListener((data) => { QuitGame(); });
        quitTrigger.triggers.Add(quitEntry);
    }

    // Resto del código permanece igual...
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}