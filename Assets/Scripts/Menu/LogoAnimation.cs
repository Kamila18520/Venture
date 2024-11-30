using System.Collections;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    public RectTransform logo;  
    public float moveDuration = 1.0f;  // Czas trwania wej�cia logo na ekran
    public float scaleDuration = 0.5f;  // Czas trwania animacji skalowania
    public float scaleAmount = 1.1f;  // Jak bardzo logo si� powi�ksza

    void Start()
    {
        Vector2 startPos = logo.localPosition;
        // Ustawienie logo poza ekranem na pocz�tku (np. wysoko nad ekranem)
        logo.anchoredPosition = new Vector2(0, Screen.height);

        // Animacja wej�cia logo na ekran
        LeanTween.move(logo, startPos, moveDuration).setEase(LeanTweenType.easeOutBounce).setOnComplete(StartPulsing);
    }

    // Funkcja uruchamiana po zako�czeniu animacji wej�cia
    void StartPulsing()
    {
        // Skalowanie w p�tli - logo pulsuje
        LeanTween.scale(logo, Vector3.one * scaleAmount, scaleDuration)
                 .setEase(LeanTweenType.easeInOutSine)
                 .setLoopPingPong();  // P�tla, kt�ra zmienia wielko�� logo na przemian
    }
}
