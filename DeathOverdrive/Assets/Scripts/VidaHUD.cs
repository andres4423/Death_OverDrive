using UnityEngine;
using UnityEngine.UI;

public class VidaHUD : MonoBehaviour
{
    public Slider vidaSlider;

    void OnEnable()
    {
        if (VidaJugadorManager.Instance != null)
        {
            vidaSlider.maxValue = VidaJugadorManager.Instance.vidaMaxima;
            vidaSlider.value = VidaJugadorManager.Instance.vidaActual;
        }
    }


    void Update()
    {
        vidaSlider.value = VidaJugadorManager.Instance.vidaActual;
    }
}
