using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private SO_Chest SO_Chest;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && SO_Chest.allChestsAreOpen)
        {
            GameObject menuUI = GameObject.FindGameObjectWithTag("MainUI");

            Debug.Log("Player won game");
            menuUI.GetComponent<MenuGameManager>().ShowEndGamePanel();
        }
    }  
}
