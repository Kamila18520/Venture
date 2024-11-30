using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationWord : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        transform.Rotate(new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, _speed * Time.deltaTime));
    }
}
