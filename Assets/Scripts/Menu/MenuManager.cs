using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject[] _menuPanels;
    [SerializeField] private GameObject _courtine;
    [SerializeField] private float playDuration = 2.5f;
    private void Start()
    {
        Cursor.visible = true;
        
        //Start game
        _courtine.SetActive(false);
        mainPanel.SetActive(true);
        mainPanel.GetComponent<CanvasGroup>().interactable = true;

        foreach (var panel in _menuPanels)
        {
            panel.gameObject.SetActive(false);
        }

    }

    public void OpenPanel(GameObject panel)
    {
        PanelAnimation(panel, true); // Animacja wejœcia
        mainPanel.GetComponent<CanvasGroup>().interactable = false;

    }

    public void ClosePanel(GameObject panel)
    {
        PanelAnimation(panel, false); // Animacja wyjœcia
        mainPanel.GetComponent<CanvasGroup>().interactable = true;

    }

    public void PlayButton()
    {
        mainPanel.GetComponent<CanvasGroup>().interactable = false;
        _courtine.SetActive(true);

        StartCoroutine(ChangeFillAmountCoroutine(playDuration, _courtine));
       
    }

    private IEnumerator ChangeFillAmountCoroutine(float duration, GameObject obj)
    {
        float startValue = 0f;
        float targrtValue = 1f;

        LeanTween.value(gameObject, startValue, targrtValue, duration)
            .setOnUpdate((float value) => {
                obj.GetComponent<Image>().fillAmount = value;
            })
            .setEase(LeanTweenType.easeInOutCirc);

        yield return new WaitForSeconds(duration);
        PanelAnimation(mainPanel, false);


        yield return new WaitForSeconds(1);
        LoadLevel();

    }

    private void LoadLevel()
    {
            SceneManager.LoadScene(1);
            Debug.Log("Scene has been loaded");

    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); // Zamyka grê po zbudowaniu
#endif
    }

    private void PanelAnimation(GameObject panel, bool inOut)
    {
        panel.GetComponent<CanvasGroup>().interactable = inOut;

        if (inOut)
        {
            //mainPanel.GetComponent<CanvasGroup>().interactable = false;

            // Animacja wejœcia - pokazanie panelu i powiêkszenie do pe³nego rozmiaru
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero; // Ustawiamy skalê na 0
            LeanTween.scale(panel, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack); // Powiêkszenie
        }
        else
        {
            // Animacja wyjœcia - zmniejszenie rozmiaru i wy³¹czenie panelu
            LeanTween.scale(panel, Vector3.zero, 0.5f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() => panel.SetActive(false)); // Wy³¹czenie po animacji

           // mainPanel.GetComponent<CanvasGroup>().interactable = true;
        }
    }
}
