using UnityEngine;
using System;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [Header("TEST")]
    public bool generateEmptyWord;
    public bool generateTerrain;
    public bool spawnRandomPlayer;
    public bool spawnEnemies;
    public bool spawnChests;
    public bool spawnWalkie;
    public bool showMap;


    [Header("Perlin Noise values")]
    [Range(450, 1000)]
    public int TextureSize;

    private static float _noiseScale = 0.7f;

    public static float NoiseScale
    {
        get => _noiseScale;
        set => _noiseScale = value;
    }
    public float IslandSize;
    [Range(1, 20)] public int NoiseOctaves;
    [Range(0, 999)] public int SeedRange;
    private float[,] _noiseMap;

    [Header("Islands values")]
    [Range(-5f, 5f)]
    public float range;
    private Color _color;


    [Header("Material")]
    [SerializeField] private Material _mainMaterial;
    private Texture2D _mainTexture2D;
    [SerializeField] private Image _mainImage;


    [Header("Ground")]
    public GameObject GroundGrid;
    [SerializeField] private Transform _groundParent;
    public TerrainGenerator TerrainGenerator;

    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private int playerSpawnHeight = 5;

    [Header("Enemy")]
    [SerializeField] private EnemiesSpawner _enemySpawner;


    [Header("Chest")]
    [SerializeField] private GameObject _chest;
    [SerializeField] private int chestSpawnHeight = 5;
    [SerializeField] private Transform _chestParent;

    [Header("WalkieTalkie")]
    [SerializeField] private GameObject _walkie;
    [SerializeField] private int walkieSpawnHeight = 5;



    private void Start()
    {
        Cursor.visible = false;

        if(TextureSize <=0)
        {
            Debug.Log("The texture size parameters are incorrect.");
        }


        _noiseMap = new float[TextureSize, TextureSize];
        _mainTexture2D = new Texture2D(TextureSize, TextureSize);
        _mainMaterial.SetTexture("_MainTex", _mainTexture2D);

        if (generateTerrain) generateEmptyWord = true;
        if(generateEmptyWord) GenerateMainMap();

        if(spawnEnemies)
        {
            _enemySpawner.enabled = true;
        }
        else
        {
            _enemySpawner.enabled = false;
        }
        
    }


    public void GenerateMainMap()
    {

        GeneratePerlinNoiseTexture2D();

        if (generateTerrain)
        {
            TerrainGenerator.GenerateTerrain();
            Debug.Log("The Terrain has been successfully generated.");
        }

        if (spawnRandomPlayer)
        {
            SpawnObject.SpawnObjectAtRandomPlace(_player, playerSpawnHeight, _groundParent);
        }

        if (spawnChests)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject chest = Instantiate(_chest);
                SpawnObject.SpawnObjectAtRandomPlace(chest, chestSpawnHeight, _groundParent);
            }
        }

        if (spawnWalkie)
        {
            SpawnObject.SpawnObjectAtRandomPlace(_walkie, walkieSpawnHeight, _groundParent);
        }

        

    }

    private void GeneratePerlinNoiseTexture2D()
    {
        Vector2 Org = new Vector2(UnityEngine.Random.Range(0f, SeedRange), UnityEngine.Random.Range(0f, SeedRange));

        // Iterowanie po pikselach tekstury i przypisywanie wartoœci na podstawie szumu
        for (int x = 0; x < TextureSize; x++)
        {
            for (int y = 0; y < TextureSize; y++)
            {
                // Obliczenie wartoœci szumu
                _noiseMap[x, y] = PerlinNoise2D.PerlinNoiseValue(x, y, Org, TextureSize, _noiseScale, NoiseOctaves, IslandSize);


                if (_noiseMap[x, y] > range)
                {
                    GenerateMap(x, y);
                    _color = Color.black;
                }
                else
                {

                    _color = Color.white;
                }

                _mainTexture2D.SetPixel(x, y, _color);
            }
        }


        _mainTexture2D.Apply();
        _mainTexture2D.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < _groundParent.childCount; i++)
        {
            GameObject child = _groundParent.GetChild(i).gameObject;

            // Sprawdzamy, czy dziecko ma komponent GroundEdgeController
            GroundEdgeController groundEdgeController = child.GetComponent<GroundEdgeController>();
            if (groundEdgeController != null)
            {
                groundEdgeController.CheckNeighbors();  // Wywo³ujemy metodê CheckNeighbors() dla tego obiektu
            }
        }
        _mainTexture2D.Apply();
        _mainTexture2D.wrapMode = TextureWrapMode.Clamp;

        // Przypisanie tekstury do UI (Image)
        if (showMap)
        {
            _mainImage.sprite = Sprite.Create(
                _mainTexture2D,
                new Rect(0, 0, _mainTexture2D.width, _mainTexture2D.height),
                new Vector2(0.5f, 0.5f));
        }
        else
        {
            _mainImage.enabled = false;
        }
        Debug.Log("The map has been successfully generated.");
    }

    public void GenerateMap(int x, int y)
    {
       
        GameObject cube = Instantiate(GroundGrid,new Vector3(x - (TextureSize/2),0,y - (TextureSize/2)), Quaternion.Euler(0,0,0), _groundParent);
    } 
}
