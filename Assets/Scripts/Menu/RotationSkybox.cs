using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSkybox : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject MainCamera;

    void Update()
    {
        MainCamera.transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }
}
