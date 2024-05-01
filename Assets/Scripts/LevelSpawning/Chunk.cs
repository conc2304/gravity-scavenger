using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Vector2Int coordinates;
    public bool isActive;
    private readonly List<GameObject> items = new();
    private Vector2 size;
    private Rect boundary;

    private readonly int minMass = 25;
    private readonly int maxMass = 50;
    private readonly int minGravityRange = 10;
    private readonly int maxGravityRange = 50;

    private readonly List<GameObject> PlanetPrefabs = new();
    private readonly List<GameObject> EnemyPrefabs = new();
    private readonly List<GameObject> PickupPrefabs = new();
    private readonly GameObject SpaceStationPrefab;

    private readonly float Spawn_Z = -0.5f;

    public Chunk(Vector2Int coordinates, Vector2 chunkSize, List<GameObject> enemyPrefabs, List<GameObject> pickupPrefabs, List<GameObject> planetPrefabs, GameObject spaceStationPrefab)
    {
        this.coordinates = coordinates;
        size = chunkSize;

        PlanetPrefabs = planetPrefabs;
        EnemyPrefabs = enemyPrefabs;
        PickupPrefabs = pickupPrefabs;
        SpaceStationPrefab = spaceStationPrefab;

        // Calculate the boundary of the chunk based on its size and position
        boundary = new Rect(
            // Offset for half of width or height so that center of (0,0) is center of screen
            coordinates.x * size.x - (size.x / 2),
            coordinates.y * size.y - (size.y / 2),
            size.x,
            size.y
        );
    }

    public void Load()
    {
        Debug.Log("Load Chunk: " + isActive);
        // If alread active  then don't respawn anything here
        if (isActive) return;
        isActive = true;


        // % chance that this zone will be totally empty
        float chanceOfEmptiness = 50f;
        float r = Random.Range(0, 100);
        float r2 = Random.Range(0, 100);
        if (r < chanceOfEmptiness)
        {
            SpawnItems();
        }
        else if (r2 < 8f)
        {
            // Only put Space Stations in a few empty chunks
            SpawnSpaceStation();
        }

        // SpawnDebugger();
    }

    public void Unload()
    {
        Debug.Log("Unload");
        isActive = false;
        DespawnItems();
    }

    private void SpawnItems()
    {
        // Make spawning game entities more random
        float r0 = Random.Range(0, 100);
        float r1 = Random.Range(0, 100);
        float r2 = Random.Range(0, 100);

        float playerXP = PlayerStatsManager.Instance.points;
        //  TODO - Update the chance of enemies depending on level/gamepoints

        if (r0 < 20) SpawnEnemies();
        if (r1 < 70) SpawnPickups();
        if (r2 < 60) SpawnPlanets();
    }

    private void SpawnDebugger()
    {
        // Put in the middle to get a sense of how big the chunks are
        Debug.Log("SpawnDebugger");
        Debug.Log(boundary);
        float spawnX = boundary.center.x;
        float spawnY = boundary.center.y;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, Spawn_Z);
        int index = coordinates == new Vector2Int(0, 0) ? 2 : 0;
        GameObject spawnedObjC = Object.Instantiate(PickupPrefabs[index], spawnPosition, PickupPrefabs[0].transform.rotation);
        GameObject spawnedObjBL = Object.Instantiate(PickupPrefabs[1], new Vector3(boundary.xMin, boundary.yMin, Spawn_Z), PickupPrefabs[0].transform.rotation);
        GameObject spawnedObjBR = Object.Instantiate(PickupPrefabs[1], new Vector3(boundary.xMax, boundary.yMin, Spawn_Z), PickupPrefabs[0].transform.rotation);
        GameObject spawnedObjTR = Object.Instantiate(PickupPrefabs[1], new Vector3(boundary.xMax, boundary.yMax, Spawn_Z), PickupPrefabs[0].transform.rotation);
        GameObject spawnedObjTL = Object.Instantiate(PickupPrefabs[1], new Vector3(boundary.xMin, boundary.yMax, Spawn_Z), PickupPrefabs[0].transform.rotation);

        // items.Add(spawnedObj);
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
        int enemyCount = Random.Range(1, 2);
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnObject(EnemyPrefabs[Mathf.RoundToInt(Random.Range(0, EnemyPrefabs.Count))]);
            // We don't add enemies to items so that if an enemy is chasing us it does not get despawn when we are running
            // Enemy despawning is handled on its own
        }
    }

    private void SpawnPickups()
    {
        int pickupCount = Random.Range(1, 3);
        for (int i = 0; i < pickupCount; i++)
        {
            GameObject pickup = SpawnObject(PickupPrefabs[Mathf.RoundToInt(Random.Range(0, PickupPrefabs.Count))]);
            items.Add(pickup);
        }
    }

    private void SpawnPlanets()
    {
        int planetCount = Random.Range(1, 2);
        for (int i = 0; i < planetCount; i++)
        {
            GameObject planet = SpawnObject(PlanetPrefabs[Mathf.RoundToInt(Random.Range(0, PlanetPrefabs.Count))]);
            // Set Gravity props
            planet.GetComponent<Rigidbody>().mass = Random.Range(minMass, maxMass);
            planet.GetComponent<GravityField>().maxDistance = Random.Range(minGravityRange, maxGravityRange);
            float planetScale = Random.Range(0.4f, 1.75f);
            planet.transform.localScale *= planetScale;

            // Add some number of power ups circling the planet
            // TODO - Make pickups and moons to orbit the planet
            int pickupCount = Random.Range(1, 5);
            float angleIncrement = 360f / pickupCount;
            float pickupSize = 1f;
            float radius = planetScale * Random.Range(1f, 2f) + pickupSize;
            for (int j = 0; j < pickupCount; j++)
            {
                // Calculate the angle for this item
                float angle = j * angleIncrement;
                // Calculate the position using trigonometry
                float x = planet.transform.position.x + (radius * Mathf.Cos(angle * Mathf.Deg2Rad));
                float y = planet.transform.position.y + (radius * Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject prefab = PickupPrefabs[Mathf.RoundToInt(Random.Range(0, PickupPrefabs.Count))];
                Vector3 spawnPosition = new Vector3(x, y, prefab.transform.position.z);

                GameObject spawnedObj = Object.Instantiate(prefab, spawnPosition, prefab.transform.rotation);
                items.Add(spawnedObj);
            }

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

