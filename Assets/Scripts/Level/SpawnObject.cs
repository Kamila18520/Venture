using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class SpawnObject
{
    public static void SpawnObjectAtRandomPlace(GameObject objectToSpawn, int objectStartHeight, Transform GroundParent)
    {

       
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Object is null.");
            return;
        }
        else { 
        
        objectToSpawn.SetActive(false);
        }

        // Uzyskanie wszystkich dzieci obiektu GroundParent
        Transform[] groundChildren = GroundParent.GetComponentsInChildren<Transform>();


        if (groundChildren.Length == 0)
        {
            Debug.LogWarning("GroundParent is empty.");
            return;
        }

         int randomIndex = UnityEngine.Random.Range(1, groundChildren.Length); 
        Transform randomGroundChild = groundChildren[randomIndex];

        Vector3 spawnPosition = new Vector3(randomGroundChild.position.x, randomGroundChild.position.y + objectStartHeight, randomGroundChild.position.z);


            // Instancjowanie obiektu na losowej pozycji
            objectToSpawn.transform.position = spawnPosition;

            objectToSpawn.SetActive(true);

            Debug.Log("Object has been spawned at position: " + spawnPosition);
        

    }

    
}
