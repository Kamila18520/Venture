using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEdgeController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool[] raycast = new bool[4];  // Tablica do przechowywania wyników raycastów
    [SerializeField] private GameObject defaultObject;
    [SerializeField] private GameObject edgeObject;
    [SerializeField] private Rotation[] rotations;     // Tablica rotacji

    private int num;  // Liczba wykrytych obiektów
    private Vector3[] vectors = new Vector3[]  // Tablica wektorów reprezentuj¹cych ró¿ne kierunki
    {
        new Vector3(1, 2, 0),
        new Vector3(0, 2, 1),
        new Vector3(-1, 2, 0),
        new Vector3(0, 2, -1)
    };



    public void CheckNeighbors()
    {
        num = 0;
        for (int i = 0; i < raycast.Length; i++)
        {

            raycast[i] = Physics.Raycast(transform.position + vectors[i], Vector3.down, 4f, groundLayer);

            if (raycast[i])
                num++;
        }

        ManageObjects();
    }

    private void ManageObjects()
    {
        if (num != 2 && edgeObject != null)
        {
            Destroy(edgeObject);
        }
        else if (num == 2 && defaultObject != null)
        {
            Destroy(defaultObject);
        }

        CheckBools();
    }

    private void CheckBools()
    {
        for (int i = 0; i < rotations.Length; i++)
        {
            if (AreBoolsMatching(rotations[i].bools, raycast))
            {
                if (edgeObject != null)
                {
                    edgeObject.transform.rotation = Quaternion.Euler(0, rotations[i].rot, 0);
                }
                break;
            }
        }
    }

    private bool AreBoolsMatching(bool[] boolsToMatch, bool[] raycastBools)
    {
        if (boolsToMatch.Length != raycastBools.Length)
            return false;

        for (int i = 0; i < boolsToMatch.Length; i++)
        {
            if (boolsToMatch[i] != raycastBools[i])
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class Rotation
{
    public string des;
    public int rot;  
    public bool[] bools = new bool[4]; 
}
