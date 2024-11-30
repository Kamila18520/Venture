using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecstDebugPosition : MonoBehaviour
{
    [SerializeField] private bool isDebugActive;
    [SerializeField] private GameObject platform;
    [SerializeField] private float gameobjectY;
    void Update()
    {
        gameobjectY = gameObject.transform.position.y;
        if (gameobjectY < 0)
        {
            platform.SetActive(true);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isDebugActive = true;
        }
        else
        {
            platform.SetActive(false);
        }
    }
}
