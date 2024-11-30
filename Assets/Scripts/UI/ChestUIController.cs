using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ChestUIController : MonoBehaviour
{
    [SerializeField] private SO_Chest SO_Chest;
    [SerializeField] private int count;
    [SerializeField] private bool winGame;
    [SerializeField] private TextMeshProUGUI textCount;

    [SerializeField] private GameObject WinPanel;

    private void Awake()
    {
        WinPanel.SetActive(false);

        count = 0;
        winGame = false;
        SO_Chest.ClearSOChest();

        UpdateChestCount();
    }


    public void UpdateChestCount()
    {

        count = 0; 
        for (int i = 0; i < SO_Chest.chestsOpened.Count; i++)
        {
            if (SO_Chest.chestsOpened[i])
            {
                count++;
            }
        }
        if (SO_Chest.allChestAreOpen)
        {
            winGame = true;
        }

        // Zaktualizuj tekst
        textCount.text = count.ToString() + "/3";
    }

    private IEnumerator WinGame()
    {
        yield return new WaitForSeconds(2);
        WinPanel.SetActive(true);

    }
}
