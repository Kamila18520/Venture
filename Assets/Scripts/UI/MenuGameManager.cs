using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuGameManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject _courtine;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private float duration;
    [SerializeField] private PlayerValues healthValues;
    private bool isPaused = false;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else { UnPauseGame();
            }
        }

        if(healthValues.currentValue <=0)
        {
            ShowEndGamePanel();
        }
    }

    public void ShowEndGamePanel()
    {
        Cursor.visible = true;
        endGamePanel.SetActive(true);
        Time.timeScale = 0f;

    }

    public void PauseGame()
    {
        isPaused = true;
        Cursor.visible = true; 
        pausePanel.SetActive(true );
        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        isPaused = false ;
        Cursor.visible = false; // Ukryj kursor
        pausePanel.SetActive(false);

        AudioListener.pause = false;


    }


    public void PlayAgain()
    {

        StartCoroutine(ChangeFillAmountCoroutine(duration, 1));
    }


    public void LoadMenu()
    {
        StartCoroutine(ChangeFillAmountCoroutine(duration, 0));

    }

    private IEnumerator ChangeFillAmountCoroutine(float duration, int num)
    {
        _courtine.SetActive(true);

        Image courtineImage = _courtine.GetComponent<Image>();
        if (courtineImage == null)
        {
            Debug.LogError("Obiekt _courtine nie ma komponentu Image!");
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Procent ukoñczenia animacji (0-1)
            float easedT = EaseInOut(t);      // Zastosowanie krzywej ³agodzenia

            courtineImage.fillAmount = easedT;

            elapsedTime += Time.unscaledDeltaTime; // U¿yj rzeczywistego czasu, niezale¿nego od Time.timeScale
            yield return null;
        }

        courtineImage.fillAmount = 1f;

        yield return new WaitForSecondsRealtime(1);

        LoadLevel(num);
    }

    // Funkcja EaseInOut dla p³ynnej interpolacji
    private float EaseInOut(float t)
    {
        return t < 0.5f
            ? 2f * t * t
            : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
    }

    //  private IEnumerator ChangeFillAmountCoroutine(float duration, int num)
    //  {
    //      _courtine.SetActive(true);
    //      float startValue = 0f;
    //      float targetValue = 1f;
    //
    //      LeanTween.value(gameObject, startValue, targetValue, duration)
    //          .setOnUpdate((float value) => {
    //              _courtine.GetComponent<Image>().fillAmount = value;
    //          })
    //          .setEase(LeanTweenType.easeInOutCirc);
    //
    //      yield return new WaitForSecondsRealtime(duration); // U¿yj rzeczywistego czasu
    //
    //      yield return new WaitForSecondsRealtime(1); // Drugi odczekany czas
    //
    //      LoadLevel(num);
    //  }

    private void LoadLevel(int num)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;


        SceneManager.LoadScene(num);
    }
}
