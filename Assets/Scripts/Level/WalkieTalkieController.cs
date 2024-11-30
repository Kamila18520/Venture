using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieTalkieController : MonoBehaviour
{
    [SerializeField] private AudioSource audioClip;
    [SerializeField] private GameObject letterE;
    private bool isMusicHasBeenPlayed = false;
    private bool isPlayerInTrigger = false;

    private void Update()
    {
        if (letterE.activeSelf)
        {
            letterE.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 100);
        }


        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && !isMusicHasBeenPlayed)
        {
            audioClip.Play();
            isMusicHasBeenPlayed = true;
            EVisibility(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            EVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            EVisibility(false);
        }
    }

    private void EVisibility(bool visibility)
    {
        letterE.SetActive(visibility);
    }
}
