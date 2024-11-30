using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private bool openChect = false;
    [SerializeField] private bool playerInRange;

    [SerializeField] private float timeToOpenChest;

    [SerializeField] private SO_Chest SO_Chest;
    private float count;
    private ChestUIController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SphereCollider col;



    private void Start()
    {
        GameObject mainUI = GameObject.FindWithTag("MainUI");
        controller = mainUI.GetComponent<ChestUIController>();
        SO_Chest.AddChest();
    }

    private void Update()
    {
        if (playerInRange)
        {
            count += +Time.deltaTime;

            if (count > timeToOpenChest && !openChect)
            {
                openChect = true;
                col.enabled = false;
                animator.SetBool("OpenChest", true);
                SO_Chest.OpenChest(controller);
            }
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            audioSource.Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (!openChect)
                audioSource.Stop();
            count = 0;
        }
    }
}
