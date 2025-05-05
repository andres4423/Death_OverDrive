using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
    public AudioClip sceneMusic;
    public bool playOnStart = true;
    public float volume = 0.15f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Configura el AudioSource si no está configurado
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = sceneMusic;
        audioSource.volume = volume;
        audioSource.loop = true;
        
        if (playOnStart && sceneMusic != null)
        {
            audioSource.Play();
        }
    }
    
    // Métodos públicos para control desde otros scripts
    public void PlayMusic()
    {
        if (sceneMusic != null)
        {
            audioSource.Play();
        }
    }
    
    public void StopMusic()
    {
        audioSource.Stop();
    }
    
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    
    public void SetVolume(float newVolume)
    {
        audioSource.volume = newVolume;
    }
}