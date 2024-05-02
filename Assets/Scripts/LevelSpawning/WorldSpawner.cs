using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject Player;
    // we are 
    private Vector2 chunkSize;
    private readonly Dictionary<Vector2Int, Chunk> chunks = new();
    private Vector2Int currentChunk;
    private HashSet<Vector2Int> loadedChunks = new();

    public List<GameObject> PlanetPrefabs = new();
    public List<GameObject> EnemyPrefabs = new();
    public List<GameObject> PickupPrefabs = new();
    public GameObject SpaceStationPrefab;

    private bool isFirstLoad = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        // TODO find a way to get the z position of the background game object from here
        // Assuming the camera's z position is negative, facing along the positive z-axis, and that the game plane is at z=0 

        chunkSize = GetChunkSize();
        InitializeChunks();
        currentChunk = GetCurrentChunk(Player.transform.position);
        UpdateChunks(currentChunk);
        isFirstLoad = false;
    }


    // Create a virtual grid of 5x5 with current view as the center.  
    // Use the grid to selectively spawn planetary systems so that they are not too close together
    private void InitializeChunks()
    {

        Debug.Log("InitializeChunks");
        int gridPadding = 2; // Define padding to create a 5x5 grid around the center of the screen
        Vector2Int centerChunk = new Vector2Int(0, 0); // Center chunk position

        // Loop through grid coordinates within the specified padding
        for (int x = -gridPadding; x <= gridPadding; x++)
        {
            for (int y = -gridPadding; y <= gridPadding; y++)
            {
                // Create a variable representing the chunk's coordinates
                Vector2Int chunkCoordinates = new Vector2Int(x, y);
                // Create a new Chunk object at the specified coordinates with the given chunk size
                chunks[chunkCoordinates] = new Chunk(chunkCoordinates, chunkSize, EnemyPrefabs, PickupPrefabs, PlanetPrefabs, SpaceStationPrefab);

                // If this is the first load and current chunk is center, then skip spawning entities
                bool skipSpawning = isFirstLoad && x == centerChunk.x && y == centerChunk.y;
                if (skipSpawning)
                {
                    // Set it to active before load so that we do not spawn anything on chunk.Load();
                    chunks[chunkCoordinates].isActive = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player) return;

        Vector2Int newChunk = GetCurrentChunk(Player.transform.position);

        if (newChunk != currentChunk)
        {
            Debug.Log("New Chunk");
            UpdateChunks(newChunk);
            currentChunk = newChunk;
        }
    }

    void UpdateChunks(Vector2Int newChunk)
    {
        Debug.Log("UpdateChunks");
        // HashSet to store chunks that need to be loaded
        HashSet<Vector2Int> chunksToLoad = new HashSet<Vector2Int>();

        // Loop through neighboring chunks around the newChunk
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Calculate the coordinates of the nearby chunk
                Vector2Int nearbyChunk = new Vector2Int(newChunk.x + x, newChunk.y + y);

                // Add the nearby chunk to the set of chunks to load
                chunksToLoad.Add(nearbyChunk);

                // Check if the nearby chunk is not already loaded, and if not then load one
                if (!loadedChunks.Contains(nearbyChunk))
                {
                    if (!chunks.ContainsKey(nearbyChunk))
                    {
                        chunks[nearbyChunk] = new Chunk(nearbyChunk, chunkSize, EnemyPrefabs, PickupPrefabs, PlanetPrefabs, SpaceStationPrefab);
                    }
                    chunks[nearbyChunk].Load();
                }
            }
        }

        // Unload chunks that are no longer nearby
        foreach (var chunk in loadedChunks)
        {
            if (!chunksToLoad.Contains(chunk))
            {
                chunks[chunk].Unload();
            }
        }

        loadedChunks = chunksToLoad;
    }

    private Vector2 GetChunkSize()
    {
        // We are defining the space of a chunk to be the size of the game screen times the multiplier
        float multiplier = 1.5f;
        float cameraDepth = Mathf.Abs(mainCamera.transform.position.z); // Assumes our game happens at z = 0

        // Use the bottom-left and top-right corners in world coordinates
        Vector3 worldBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, cameraDepth));
        Vector3 worldTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraDepth));

        // Calculate width and height
        float worldWidth = worldTopRight.x - worldBottomLeft.x;
        float worldHeight = worldTopRight.y - worldBottomLeft.y;

        Debug.Log("World Width: " + worldWidth);
        Debug.Log("World Height: " + worldHeight);

        return new Vector2(worldWidth * multiplier, worldHeight * multiplier);
    }
    // Calculate the chunk coordinates based on the player's position in the world space.
    public Vector2Int GetCurrentChunk(Vector3 playerPosition)
    {
        // Calculate the chunk coordinates based on the player's position divided by the chunk size
        int chunkX = Mathf.FloorToInt(playerPosition.x / chunkSize.x);
        int chunkY = Mathf.FloorToInt(playerPosition.y / chunkSize.y);

        // Return the chunk coordinates
        return new Vector2Int(chunkX, chunkY);
    }
}
