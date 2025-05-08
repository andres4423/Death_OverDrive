using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance { get; private set; }
    
    [Header("Configuración de Slow Motion")]
    public float slowMotionFactor = 0.3f;

    public bool IsInSlowMotion { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateSlowMotion()
    {
        if (!IsInSlowMotion)
        {
            IsInSlowMotion = true;
            Time.timeScale = slowMotionFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Debug.Log("Slow Motion Activado");
        }
    }

    public void DeactivateSlowMotion()
    {
        if (IsInSlowMotion)
        {
            IsInSlowMotion = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            Debug.Log("Slow Motion Desactivado");
        }
    }
}
