using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject _courtine; // Referencja do obiektu, kt�ry b�dzie animowany
    public UnityEngine.Object menuScene; // Obiekt sceny menu
    [SerializeField] private float playDuration = 2.5f; // Czas animacji

    // Funkcja wywo�ywana po naci�ni�ciu przycisku "Play"
    public void PlayButton()
    {
        _courtine.SetActive(true); // Pokazuje animacj� "wczytywania"
        StartCoroutine(ChangeFillAmountCoroutine(playDuration)); // Uruchamia animacj� wype�niania
    }

    // Korutyna animuj�ca wype�nianie obiektu _courtine
    private IEnumerator ChangeFillAmountCoroutine(float duration)
    {
        float startValue = 0f; // Pocz�tkowa warto�� wype�nienia
        float targetValue = 1f; // Ko�cowa warto�� wype�nienia

        // Animacja wype�niania obiektu _courtine
        LeanTween.value(gameObject, startValue, targetValue, duration)
            .setOnUpdate((float value) => {
                _courtine.GetComponent<Image>().fillAmount = value; // Aktualizowanie wype�nienia
            })
            .setEase(LeanTweenType.easeInOutCirc); // Ustawienie efektu animacji

        // Czekanie na zako�czenie animacji
        yield return new WaitForSeconds(duration);

        // Po zako�czeniu animacji, zamknij g��wny panel (je�li istnieje)
        PanelAnimation(false);

        // Czekanie przez dodatkowy czas, je�li potrzeba
        yield return new WaitForSeconds(1);

        // Za�aduj scen� menu
        LoadLevel();
    }

    // Funkcja �adowania sceny
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
        // Dzia�ania przy animacji otwierania lub zamykania panelu
        var panel = _courtine; // Przyjmujemy, �e _courtine to panel, kt�ry chcemy animowa�

        if (inOut)
        {
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero; // Ustawiamy skal� na 0
            LeanTween.scale(panel, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack); // Powi�kszenie
        }
        else
        {
            LeanTween.scale(panel, Vector3.zero, 0.5f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() => panel.SetActive(false)); // Ukrycie panelu po animacji
        }
    }
}
