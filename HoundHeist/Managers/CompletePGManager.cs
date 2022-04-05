using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class CompletePGManager : MonoBehaviour
{
    [SerializeField] private GameObject companionPrefab;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Tilemap[] groundTileMaps;

    [SerializeField] private int width = 25;
    [SerializeField] private int height = 25;
    [SerializeField] private int dimensionDeviation = 10;

    private NavMeshSurface2d navMesh;

    private int[,] walkableMap;
    private int[,] wallMap;
    private int[,] colliderMap;
    private int[,] collectiveMap;

    private void Awake() => navMesh = FindObjectOfType<NavMeshSurface2d>();

    private void Start()
    {
        //Assign a new random height and width depending on the deviation
        width = Random.Range(width - dimensionDeviation, width + dimensionDeviation);
        height = Random.Range(height - dimensionDeviation, height + dimensionDeviation);
        
        //Procedurally creates a room
        LevelGeneration();

        //Bakes a new NavMesh based on the room layout
        //navMesh.BuildNavMesh();

        //Creates your companion in scene
        Instantiate(companionPrefab, new Vector3(1, -4, 0), Quaternion.identity);
    }

    private void LevelGeneration()
    {
        //Creates the arrays for the map data
        walkableMap = GenerateArray(width, height, true);
        wallMap = GenerateArray(width, height, true);
        colliderMap = GenerateArray(width, height, true);
        collectiveMap = GenerateArray(width, height, true);

        //Populates the map array with data
        walkableMap = RecordWalkableSurface(walkableMap);
        wallMap = RecordWallSurfaces(wallMap);
        wallMap = RecordCornerSurfaces(wallMap);
        colliderMap = RecordCollisionSurfaces(colliderMap);
        RecordCollectiveMap(walkableMap, wallMap, colliderMap);

        //Renders the scene based on the random population
        RenderTerrain(walkableMap, groundTileMaps[0], groundTile);
        RenderTerrain(wallMap, groundTileMaps[1], wallTile);
        //RenderTerrain(colliderMap, groundTileMaps[2], wallTile);
    }

    //Creates an array of the desired dimensions
    private int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width + 2, height + 2];

        for (int x = 0; x < width + 2; x++)
        {
            for (int y = 0; y < height + 2; y++)
            {
                map[x, y] = (empty) ? 0 : 1;
            }
        }

        return map;
    }

    //Catalogs the floor map with flooring
    private int[,] RecordWalkableSurface(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) //If this is the first or last row or column, make it empty
                    map[x, y] = 0;
                else if (x == 1 || y == 1 || x == width - 2 || y == height - 2) //If this is the second or second to last row or column, make it empty
                    map[x, y] = 0;
                else map[x, y] = 1; //Default to walkable if none of the above conditions are true
            }
        }

        var indentIndex = 0;
        var randNum = Random.Range(0, 100);

        if (randNum < 35) return map;
        else if (randNum < 60) indentIndex = 1;
        else if (randNum < 85) indentIndex= 2;
        else indentIndex = 3;

        var randomX = Random.Range((int)(width * 0.25f), (int)(width * 0.75f));
        var randomY = Random.Range((int)(height * 0.25f), (int)(height * 0.75f));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (indentIndex == 1 || indentIndex == 3)
                {
                    if (x < randomX + 1 && y > randomY + 1) map[x, y] = 0;
                }
                if (indentIndex == 2 || indentIndex == 3)
                {
                    if (x > randomX - 1 && y < randomY - 1) map[x, y] = 0;
                }
            }
        }

        return map;
    }

    //Catalogs the wall map with top walls
    private int[,] RecordWallSurfaces(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) //If this is the first or last row or column, make it empty
                    map[x, y] = 0;
                else if (walkableMap[x, y] == 1) //If the path is walkable, don't place a wall
                    map[x, y] = 0;
                else //Checks center of array
                {
                    if (walkableMap[x + 1, y] == 1 || walkableMap[x, y + 1] == 1) //Checks if there is an adjacent floor below
                        map[x, y] = 1;
                    else if (walkableMap[x - 1, y] == 1 || walkableMap[x, y - 1] == 1) //Checks if there is an adjacent floor above
                        map[x, y] = 1;
                    else
                        map[x, y] = 0;
                }
            }
        }

        return map;
    }

    //Adds corners to the map passed into the function
    private int[,] RecordCornerSurfaces(int[,] map)
    {
        int[,] cornerMap = GenerateArray(width, height, true);

        //Checks the map one more time to fill in corners
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x != 0 && y != 0 && x != width && y != height) //If this is not the padding of the array
                {
                    if (walkableMap[x, y] == 0 && map[x, y] == 0) //If there is neither a wall or a floor at this point
                    {
                        //Check for how many adjacent walls are at this spot
                        var adjacentWallCount = 0;
                        if (map[x + 1, y] == 1) adjacentWallCount++;
                        if (map[x - 1, y] == 1) adjacentWallCount++;
                        if (map[x, y + 1] == 1) adjacentWallCount++;
                        if (map[x, y - 1] == 1) adjacentWallCount++;

                        //If there is more than one adjacent wall...
                        if (adjacentWallCount > 1)
                        {
                            //If this is a convex corner, add a corner piece
                            if (map[x + 1, y - 1] == 1 || map[x + 1, y + 1] == 1) cornerMap[x, y] = 0;
                            else cornerMap[x, y] = 1;
                        }
                    }
                    else if (map[x, y] == 1) cornerMap[x, y] = 1;
                }
            }
        }

        return cornerMap;
    }

    //Catalogs the map of collision surfaces that are not visible to the player
    private int[,] RecordCollisionSurfaces(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) //If this is the first or last row or column, make it empty
                    map[x, y] = 0;
                else if (walkableMap[x, y] == 1) //If the path is walkable, don't place a wall
                    map[x, y] = 0;
                else
                {
                    if (walkableMap[x - 1, y] == 1)
                    {
                        map[x, y] = 1;
                    }
                    else if (walkableMap[x, y - 1] == 1) 
                    {
                        map[x, y] = 1;
                    }
                    else
                        map[x, y] = 0;
                }
            }
        }

        return map;
    }

    //Catalogs a map of every position that there is an obstruction to use as reference to be sure nothing will spawn there
    private void RecordCollectiveMap(int[,] surfaceMap, int[,] wallMap, int[,] colliderMap)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (wallMap[x, y] == 1 || colliderMap[x, y] == 1)
                    collectiveMap[x, y] = 1;
                else
                    collectiveMap[x, y] = 0;
            }
        }
    }

    //Renders the tiles
    private void RenderTerrain(int[,] map, Tilemap groundTileMap, TileBase groundTilebase)
    {  
        //On each x column in the level
        for (int x = 0; x < width; x++)
        {
            //Spawning needed tiles at appropriate height
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1) groundTileMap.SetTile(new Vector3Int(-x, -y, 0), groundTilebase);
            }
        }
    }
}