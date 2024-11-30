using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject _courtine; // Referencja do obiektu, który bêdzie animowany
    public UnityEngine.Object menuScene; // Obiekt sceny menu
    [SerializeField] private float playDuration = 2.5f; // Czas animacji

    // Funkcja wywo³ywana po naciœniêciu przycisku "Play"
    public void PlayButton()
    {
        _courtine.SetActive(true); // Pokazuje animacjê "wczytywania"
        StartCoroutine(ChangeFillAmountCoroutine(playDuration)); // Uruchamia animacjê wype³niania
    }

    // Korutyna animuj¹ca wype³nianie obiektu _courtine
    private IEnumerator ChangeFillAmountCoroutine(float duration)
    {
        float startValue = 0f; // Pocz¹tkowa wartoœæ wype³nienia
        float targetValue = 1f; // Koñcowa wartoœæ wype³nienia

        // Animacja wype³niania obiektu _courtine
        LeanTween.value(gameObject, startValue, targetValue, duration)
            .setOnUpdate((float value) => {
                _courtine.GetComponent<Image>().fillAmount = value; // Aktualizowanie wype³nienia
            })
            .setEase(LeanTweenType.easeInOutCirc); // Ustawienie efektu animacji

        // Czekanie na zakoñczenie animacji
        yield return new WaitForSeconds(duration);

        // Po zakoñczeniu animacji, zamknij g³ówny panel (jeœli istnieje)
        PanelAnimation(false);

        // Czekanie przez dodatkowy czas, jeœli potrzeba
        yield return new WaitForSeconds(1);

        // Za³aduj scenê menu
        LoadLevel();
    }

    // Funkcja ³adowania sceny
    private void LoadLevel()
    {
        if (menuScene != null)
        {
            SceneManager.LoadScene(menuScene.name);
            Debug.Log("Scene has been loaded");
        }
        else
        {
            Debug.LogWarning("Menu scene is not assigned.");
        }
    }

    // Funkcja do animacji zamykania/otwierania paneli
    private void PanelAnimation(bool inOut)
    {
        // Dzia³ania przy animacji otwierania lub zamykania panelu
        var panel = _courtine; // Przyjmujemy, ¿e _courtine to panel, który chcemy animowaæ

        if (inOut)
        {
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero; // Ustawiamy skalê na 0
            LeanTween.scale(panel, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack); // Powiêkszenie
        }
        else
        {
            LeanTween.scale(panel, Vector3.zero, 0.5f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() => panel.SetActive(false)); // Ukrycie panelu po animacji
        }
    }
}
