using System.Collections;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    public RectTransform logo;  
    public float moveDuration = 1.0f;  // Czas trwania wejœcia logo na ekran
    public float scaleDuration = 0.5f;  // Czas trwania animacji skalowania
    public float scaleAmount = 1.1f;  // Jak bardzo logo siê powiêksza

    void Start()
    {
        Vector2 startPos = logo.localPosition;
        // Ustawienie logo poza ekranem na pocz¹tku (np. wysoko nad ekranem)
        logo.anchoredPosition = new Vector2(0, Screen.height);

        // Animacja wejœcia logo na ekran
        LeanTween.move(logo, startPos, moveDuration).setEase(LeanTweenType.easeOutBounce).setOnComplete(StartPulsing);
    }

    // Funkcja uruchamiana po zakoñczeniu animacji wejœcia
    void StartPulsing()
    {
        // Skalowanie w pêtli - logo pulsuje
        LeanTween.scale(logo, Vector3.one * scaleAmount, scaleDuration)
                 .setEase(LeanTweenType.easeInOutSine)
                 .setLoopPingPong();  // Pêtla, która zmienia wielkoœæ logo na przemian
    }
}
