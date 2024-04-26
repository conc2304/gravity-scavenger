using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public Vector2Int Coordinates;
    public bool IsActive;
    private readonly List<GameObject> items = new();
    private Vector2 size;
    private Rect boundary;

    private readonly int minMass = 40;
    private readonly int maxMass = 50;
    private readonly int minGravityRange = 20;
    private readonly int maxGravityRange = 50;

    private readonly List<GameObject> PlanetPrefabs = new();
    private readonly List<GameObject> EnemyPrefabs = new();
    private readonly List<GameObject> PickupPrefabs = new();
    private readonly GameObject SpaceStationPrefab;

    private readonly float Spawn_Z = -0.5f;

    public Chunk(Vector2Int coordinates, Vector2 chunkSize, List<GameObject> enemyPrefabs, List<GameObject> pickupPrefabs, List<GameObject> planetPrefabs, GameObject spaceStationPrefab)
    {
        Coordinates = coordinates;
        size = chunkSize;

        PlanetPrefabs = planetPrefabs;
        EnemyPrefabs = enemyPrefabs;
        PickupPrefabs = pickupPrefabs;
        SpaceStationPrefab = spaceStationPrefab;

        // Calculate the boundary of the chunk based on its size and position
        boundary = new Rect(
            coordinates.x * size.x,
            coordinates.y * size.y,
            size.x,
            size.y
        );
    }

    public void Load()
    {
        if (IsActive) return;   // if we have already activated it then dont respawn anything here

        IsActive = true;

        // % chance that this zone will be totally empty
        float chanceOfEmptiness = 50f;
        float r = Random.Range(0, 100);
        if (r > chanceOfEmptiness)
        {
            SpawnItems();
        }
        else if (r < 5)
        {
            // Only Put Space Stations in empty chunks, and only in some of them
            SpawnSpaceStation();
        }

    }

    public void Unload()
    {
        Debug.Log("Unload");
        IsActive = false;
        DespawnItems();
    }

    private void SpawnItems()
    {
        // Make spawning game entities more random
        float r0 = Random.Range(0, 100);
        float r1 = Random.Range(0, 100);
        float r2 = Random.Range(0, 100);

        //  TODO - Update the chance of enemies depending on level/gamepoints
        if (r0 < 20) SpawnEnemies();
        if (r1 < 25) SpawnPickups();
        if (r2 < 50) SpawnPlanets();
    }

    private void SpawnDebugger()
    {
        // put in the middle
        Debug.Log("SpawnDebugger");
        Debug.Log(boundary);
        float spawnX = boundary.center.x;
        float spawnY = boundary.center.y;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, Spawn_Z);
        GameObject spawnedObj = Object.Instantiate(PickupPrefabs[0], spawnPosition, PickupPrefabs[0].transform.rotation);
        items.Add(spawnedObj);
    }

    private void SpawnSpaceStation()
    {
        Debug.Log("SpawnSpaceStation");
        // Put station in the middle of the chunk
        float spawnX = boundary.center.x;
        float spawnY = boundary.center.y;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, Spawn_Z);
        GameObject spawnedObj = Object.Instantiate(SpaceStationPrefab, spawnPosition, SpaceStationPrefab.transform.rotation);
        items.Add(spawnedObj);
    }

    private void SpawnEnemies()
    {

        int enemyCount = Random.Range(0, 2);
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnObject(EnemyPrefabs[Mathf.RoundToInt(Random.Range(0, EnemyPrefabs.Count))]);
            // We don't add enemies to items so that if an enemy is chasing us it does not get despawn when we are running
            // Enemy despawning is handled on its own
        }
    }

    private void SpawnPickups()
    {
        int pickupCount = Random.Range(0, 3); // Example range
        for (int i = 0; i < pickupCount; i++)
        {
            GameObject pickup = SpawnObject(PickupPrefabs[Mathf.RoundToInt(Random.Range(0, PickupPrefabs.Count))]);
            items.Add(pickup);
        }
    }

    private void SpawnPlanets()
    {
        int planetCount = Random.Range(0, 2); // Example range
        for (int i = 0; i < planetCount; i++)
        {
            GameObject planet = SpawnObject(PlanetPrefabs[Mathf.RoundToInt(Random.Range(0, PlanetPrefabs.Count))]);
            // Set Gravity props
            planet.GetComponent<Rigidbody>().mass = Random.Range(minMass, maxMass);
            planet.GetComponent<GravityField>().maxDistance = Random.Range(minGravityRange, maxGravityRange);
            planet.transform.localScale *= Random.Range(0.75f, 3.5f);

            items.Add(planet);
        }
    }

    private GameObject SpawnObject(GameObject prefab)
    {
        Debug.Log("SpawnObject");
        ValidateItemsList();

        float spawnX = Random.Range(boundary.xMin, boundary.xMax);
        float spawnY = Random.Range(boundary.yMin, boundary.yMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, Spawn_Z);
        GameObject spawnedObj = Object.Instantiate(prefab, spawnPosition, prefab.transform.rotation);

        return spawnedObj;
    }

    private void DespawnItems()
    {
        ValidateItemsList();
        Debug.Log("DespawnItems");
        foreach (var item in items)
        {
            Object.Destroy(item);
        }
        items.Clear();
    }


    // Some game objects like pickups and enemies get destroyed outside of Chunk
    // Remove null references before we iterate over the items
    private void ValidateItemsList()
    {
        items.RemoveAll(item => item == null);
    }
}
