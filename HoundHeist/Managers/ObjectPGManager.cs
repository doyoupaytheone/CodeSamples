using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPGManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject[] sceneObjects;
    [SerializeField] private Tilemap groundTileMap;

    [SerializeField] private int totalEnemiesToSpawn = 10;
    [SerializeField] private int width = 30;
    [SerializeField] private int height = 30;

    private Grid sceneGrid;
    private int[,] openSpaceReference;
    private int[,] objectMap;

    private void Awake()
    {
        sceneGrid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        CheckForTiles();
        LevelGeneration();
    }

    //Checks the scene for where the floor tiles have been placed and records that information
    private void CheckForTiles()
    {
        openSpaceReference = GenerateArray(width, height, false);
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (groundTileMap.HasTile(new Vector3Int(-x, -y, 0)))
                    openSpaceReference[x, y] = 0;
                else
                    openSpaceReference[x, y] = 1;
            }
        }
    }

    //Plans where objects will spawn and generates them
    private void LevelGeneration()
    {
        objectMap = GenerateArray(width, height, false);
        objectMap = RecordObjectPlacement(objectMap);
        RenderSceneObjects(sceneObjects);

        InvokeRepeating("ChooseEnemyLocations", 2, 8);
    }

    //Creates an array of the desired dimensions
    private int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (empty) ? 0 : 1;
            }
        }

        return map;
    }

    //Catalogs the map of places that scene objects will be spawned
    private int[,] RecordObjectPlacement(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (openSpaceReference[x, y] == 1)
                    map[x, y] = 0;
                else if (openSpaceReference[x - 1, y] == 1
                    || openSpaceReference[x + 1, y] == 1
                    || openSpaceReference[x, y - 1] == 1
                    || openSpaceReference[x, y + 1] == 1)
                    map[x, y] = 0;
                else
                {
                    var randNum = Random.Range(0, 100);

                    if (randNum > 80)
                    {
                        map[x, y] = 1;
                        openSpaceReference[x, y] = 1;
                    }
                    else map[x, y] = 0;
                }
            }
        }

        return map;
    }

    //Renders/positions all game objects
    private void RenderSceneObjects(GameObject[] objects)
    {
        //On each x column in the level
        for (int x = 0; x < width; x++)
        {
            //Spawning needed tiles at appropriate height
            for (int y = 0; y < height; y++)
            {
                if (objectMap[x, y] == 1)
                {  
                    SpawnGameObject(objects[ChooseObjectToSpawn(objects)], -x, -y);
                }
            }
        }
    }

    private void ChooseEnemyLocations()
    {
        if (totalEnemiesToSpawn <= 0) return;
        
        var xCoord = Random.Range(0, width);
        var yCoord = Random.Range(0, height);

        if (openSpaceReference[xCoord, yCoord] == 0)
        {
            SpawnGameObject(enemies[0], -xCoord, -yCoord);
            totalEnemiesToSpawn--;
        }
        else
            ChooseEnemyLocations();
    }

    private int ChooseObjectToSpawn(GameObject[] objects)
    {
        int randomItemIndex;
        randomItemIndex = Random.Range(0, objects.Length);
        return randomItemIndex;
    }

    private void SpawnGameObject(GameObject obj, int x, int y)
    {
        //Finds the world location of the desired cell to place the object
        var cellLocation = sceneGrid.CellToWorld(new Vector3Int(x + 1, y + 1, 0));

        obj = Instantiate(obj, cellLocation, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
