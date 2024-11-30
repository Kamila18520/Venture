using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GeneratedPrefab[] _TerrainGenerator;

    [SerializeField] private Transform _groundParent;
    [SerializeField] private Transform _elementsParent;

    [SerializeField] int countForFun;
    Transform[] groundChildren;
    List<int> groundChildernPositions = new List<int>();
    int randomIndex;
    int numberOfAllElements;
    [SerializeField] int _groundParentLenght;

    public void GenerateTerrain()
    {

        groundChildren = _groundParent.GetComponentsInChildren<Transform>();
        _groundParentLenght = groundChildren.Length;
        for (int i = 0; i < _TerrainGenerator.Length; i++)
        {
            numberOfAllElements += _TerrainGenerator[i].NumberOfObjects;
        }

        for(int i = 0; i < _groundParentLenght; i++)
        {
            groundChildernPositions.Add(i);
        }

        if (numberOfAllElements > groundChildren.Length)
        {
            Debug.LogError("Mapa nie ma wystarczaj¹co du¿o miejsca na wszystkie elementy");
            return;
        }

        for (int i = 0; i < _TerrainGenerator.Length; i++)
        {
            GenerateRandomElements(_TerrainGenerator[i]);
        }
    }

    private void GenerateRandomElements(GeneratedPrefab generatedPrefab)
    {
        int NumberOfObjects = generatedPrefab.NumberOfObjects;

        if (groundChildren.Length == 0)
        {
            Debug.LogWarning("Brak dzieci w obiekcie GroundParent.");
            return;
        }

        for (int i = 0; i < NumberOfObjects; i++)
        {
            randomIndex = RandomIndexGenerator();


            Transform groundChild = groundChildren[randomIndex];
            Vector3 terrainObjectPosition = new Vector3( groundChild.position.x, 0.5f, groundChild.position.z );

            int randomRotationY = Random.Range(0, 360);

            
            GameObject newTerrainObject = Instantiate(generatedPrefab.Prefab, terrainObjectPosition, Quaternion.Euler(0, randomRotationY, 0), _elementsParent);

            float randomScale = Random.Range(generatedPrefab.minScale, generatedPrefab.maxScale);

            newTerrainObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            countForFun++;
        }
    }

    

    private int RandomIndexGenerator()
    {
        int choosenIndex;
        randomIndex = Random.Range(1, groundChildernPositions.Count);
        choosenIndex =groundChildernPositions[randomIndex];
        groundChildernPositions.Remove(randomIndex);
        return randomIndex;

        

    }
}

[System.Serializable]
public class GeneratedPrefab
{
    public string Name;
    public GameObject Prefab;
    public int NumberOfObjects;

    public float maxScale = 1.2f;
    public float minScale = 2f;
}
