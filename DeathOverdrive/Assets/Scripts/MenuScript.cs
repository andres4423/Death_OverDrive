using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MenuScript: MonoBehaviour, IPointerClickHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
        switch (gameObject.name)
        {
            case "Jugar":
                Jugar();
                break;
            case "Configuración":
                Configuracion();
                break;
            case "Salir":
                Salir();
                break;
            default:
                Debug.Log("Opción no reconocida: " + gameObject.name);
                break;
        }
    }

   
    private void Jugar()
    {
        SceneManager.LoadScene("Tutorial"); 
    }

   
    private void Configuracion()
    {
        Debug.Log("Abriendo menú de configuración...");
       
    }

    
    private void Salir()
    {
        Application.Quit(); 
    }
}

