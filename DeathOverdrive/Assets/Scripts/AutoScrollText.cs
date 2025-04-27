using UnityEngine;
using TMPro;

public class AutoScrollText : MonoBehaviour
{
    public float scrollSpeed = 30f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
       
    }

    void Update()
    {
        // Mueve el texto usando unscaledDeltaTime (funciona aunque Time.timeScale = 0)
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.unscaledDeltaTime;
    }
}