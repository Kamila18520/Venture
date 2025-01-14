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
        else { objectToSpawn.SetActive(false); }

        Transform[] groundChildren = GroundParent.GetComponentsInChildren<Transform>();

        if (groundChildren.Length == 0)
        {
            Debug.LogWarning("GroundParent is empty.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(1, groundChildren.Length);
        Transform randomGroundChild = groundChildren[randomIndex];
        Vector3 spawnPosition = new Vector3(randomGroundChild.position.x, randomGroundChild.position.y + objectStartHeight, randomGroundChild.position.z);
        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.SetActive(true);
        Debug.Log("Object has been spawned at position: " + spawnPosition);
    }
}
