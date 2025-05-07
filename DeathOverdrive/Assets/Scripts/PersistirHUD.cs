using UnityEngine;

public class PersistirHUD : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Mantiene el HUD entre escenas
    }
}
