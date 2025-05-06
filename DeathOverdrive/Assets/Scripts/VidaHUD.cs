using UnityEngine;
using UnityEngine.UI;

public class VidaHUD : MonoBehaviour
{
    public Slider vidaSlider;

    void Start()
    {
        vidaSlider.maxValue = VidaJugadorManager.Instance.vidaMaxima;
    }

    void Update()
    {
        vidaSlider.value = VidaJugadorManager.Instance.vidaActual;
    }
}
