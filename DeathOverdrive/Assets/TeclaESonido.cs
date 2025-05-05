using UnityEngine;

public class TeclaESonido : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    public AudioClip sonidoPresionE; // Arrastra tu archivo de sonido aquí
    [Range(0, 1)] public float volumen = 0.1f; // Control deslizante para volumen

    private AudioSource audioSource;

    void Awake()
    {
        // Configura el AudioSource automáticamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Detecta cuando se presiona la tecla E
        if (Input.GetKeyDown(KeyCode.E) && sonidoPresionE != null)
        {
            // Reproduce el sonido una vez con el volumen especificado
            audioSource.PlayOneShot(sonidoPresionE, volumen);
        }
    }
}