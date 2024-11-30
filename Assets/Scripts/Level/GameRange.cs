using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRange : MonoBehaviour
{
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject menuUI = GameObject.FindGameObjectWithTag("MainUI");

            Debug.Log("EndGame");
            menuUI.GetComponent<MenuGameManager>().ShowEndGamePanel();
        }

        else if( other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().enemyCount.RemoveValue(1);
            Destroy(other);
        }
    }
}
