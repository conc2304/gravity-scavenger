using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Vector2Int coordinates;
    public bool isActive;
    private readonly List<GameObject> items = new();
    private Vector2 size;
    private Rect boundary;

    // Mass dictates gravity pull, higher mass = more gravity
    private readonly int minMass = 20;
    private int maxMass = 30;

    // The range at which gravity can affect an attractee
    private readonly int minGravityRange = 10;
    private readonly int maxGravityRange = 50;

    private readonly List<GameObject> PlanetPrefabs = new();
    private readonly List<GameObject> EnemyPrefabs = new();
    private readonly List<GameObject> PickupPrefabs = new();
    private readonly List<GameObject> AsteroidPrefabs = new();
    private readonly GameObject SpaceStationPrefab;

    private readonly float Spawn_Z = -0.5f;
    private float playerXp = 0f;
    private float minXPForEnemies = 200f;
    private bool isEmptySpace;

    public Chunk(Vector2Int coordinates, Vector2 chunkSize, List<GameObject> enemyPrefabs, List<GameObject> pickupPrefabs, List<GameObject> planetPrefabs, List<GameObject> asteroidPrefabs, GameObject spaceStationPrefab)
    {
        this.coordinates = coordinates;
        size = chunkSize;

        PlanetPrefabs = planetPrefabs;
        EnemyPrefabs = enemyPrefabs;
        PickupPrefabs = pickupPrefabs;
        AsteroidPrefabs = asteroidPrefabs;
        SpaceStationPrefab = spaceStationPrefab;

        // Calculate the boundary of the chunk based on its size and position
        boundary = new Rect(
            // Offset for half of width or height so that center of (0,0) is center of screen
            coordinates.x * size.x - (size.x / 2),
            coordinates.y * size.y - (size.y / 2),
            size.x,
            size.y
        );

        // Dynamically increase the gravity as the player gains experience
        playerXp = PlayerStatsManager.Instance.points;
        // for every 75xp over 100xp increment the maxMass by 5 up until 55
        maxMass = Mathf.Min((int)(35 + (Mathf.Max(0f, playerXp - 100) / 75 * 5)), 55);
    }

    // Spawn items in the new chunk
    public void Load()
    {
        Debug.Log("Load Chunk: " + isActive);
        // If alread active  then don't respawn anything here
        if (isActive) return;
        isActive = true;

        // Percent chance that this zone will be totally empty
        float chanceOfEmptiness = 50f;
        float r = Random.Range(0, 100);
        float r2 = Random.Range(0, 100);
        float r3 = Random.Range(0, 100);
        if (r < chanceOfEmptiness)
        {
            SpawnItems();
            isEmptySpace = false;
        }
        else if (r2 < 15f)
        {
            // Percent chance that empty zone has a space station
            SpawnSpaceStation();
            isEmptySpace = false;
        }
        else if (r3 < 50)
        {
            // Spawn Asteroids in empty spaces
            isEmptySpace = true;
            SpawnAsteroids();
        }
        isEmptySpace = true;
    }

    // Despawn and Cleanup
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
        float r3 = Random.Range(0, 100);

        float playerXP = PlayerStatsManager.Instance.points;
        //  TODO - Update the chance of enemies depending on level/gamepoints
        // Delay the introduction of enemies until player has reached minimum xp
        if ((float)playerXP > minXPForEnemies && r0 < 20) SpawnEnemies();
        if (r1 < 70) SpawnPickups();
        if (r2 < 60) SpawnPlanets();
        if (r3 < 25) SpawnAsteroids();
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

    private void SpawnAsteroids()
    {
        // If space is empty allow for larger asteroid fields
        int asteroidMax = isEmptySpace ? 12 : 4;
        int asteroidCount = Random.Range(1, asteroidMax);
        bool sameDirection = Random.Range(0f, 100f) < 45F;

        static Vector3 GetRandomDirection() => new(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        Vector3 direction = GetRandomDirection();

        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject asteroid = SpawnObject(AsteroidPrefabs[Mathf.RoundToInt(Random.Range(0, AsteroidPrefabs.Count))]);
            if (!sameDirection) direction = GetRandomDirection();
            asteroid.GetComponent<Asteroid>().initialForce = direction;

            items.Add(asteroid);
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
            // Gravity is dictated by planetary mass
            planet.GetComponent<Rigidbody>().mass = Random.Range(minMass, maxMass);
            // Sets the radius of gravitational pull
            planet.GetComponent<GravityField>().maxDistance = Random.Range(minGravityRange, maxGravityRange);
            float planetScale = Random.Range(0.4f, 1.75f);
            planet.transform.localScale *= planetScale;

            // Add some number of power ups circling the planet
            SpawnOrbiters(planet, planetScale);

            items.Add(planet);
        }
    }
    private void SpawnOrbiters(GameObject planet, float planetScale)
    {
        int pickupCount = Random.Range(1, 5);
        float angleIncrement = 360f / pickupCount;
        float pickupSize = 1f;
        float radius = (planetScale * Random.Range(1.5f, 3.5f)) + pickupSize;

        float chanceOfOrbiting = playerXp > 200f ? 20f : 0f;
        float chanceOfPlanetInOrbit = playerXp > 350f ? 30f : 0f;
        float chanceOfObstacle = playerXp > 275f ? 30f : 0;
        bool addObstacle = Random.Range(1f, 100f) < chanceOfObstacle;
        bool enableOrbit = Random.Range(1f, 100f) < chanceOfOrbiting;

        // At above 275 xp, make the radius of the orbit go in and out
        float chanceOfDynamicRadius = playerXp > 275f ? 30f : 0f;
        bool enableDynamicRadius = Random.Range(1f, 100f) < chanceOfDynamicRadius;
        float dynamicRadiusSize = Random.Range(1, 1.25f);
        float rotationSpeed = Random.Range(0.1f, 0.75f);

        GameObject spawnedObj;
        for (int j = 0; j < pickupCount; j++)
        {
            // Calculate the angle for this item
            float angle = j * angleIncrement;

            // Calculate the position using trigonometry
            float x = planet.transform.position.x + (radius * Mathf.Cos(angle * Mathf.Deg2Rad));
            float y = planet.transform.position.y + (radius * Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject prefab;
            // Pick a random prefab and spawn it
            if (addObstacle)
            {
                // For higher difficulty add obstacles like planets and  asteroids in the orbiting path.
                if (Random.Range(1f, 100) < chanceOfPlanetInOrbit)
                {
                    // Add Planet to orbit
                    prefab = PlanetPrefabs[Mathf.RoundToInt(Random.Range(0, PlanetPrefabs.Count))];
                    Vector3 spawnPosition = new Vector3(x, y, prefab.transform.position.z);
                    spawnedObj = Object.Instantiate(prefab, spawnPosition, prefab.transform.rotation);
                    // Set Gravity props
                    // Gravity is dictated by planetary mass
                    spawnedObj.GetComponent<Rigidbody>().mass = Random.Range(minMass, maxMass);
                    // Sets the radius of gravitational pull
                    spawnedObj.GetComponent<GravityField>().maxDistance = Random.Range(minGravityRange, maxGravityRange);
                    planet.transform.localScale *= planetScale / Random.Range(2f, 4f);
                }
                else
                {
                    // Add Asteroid to orbit
                    prefab = AsteroidPrefabs[Mathf.RoundToInt(Random.Range(0, AsteroidPrefabs.Count))];
                    Vector3 spawnPosition = new Vector3(x, y, prefab.transform.position.z);
                    spawnedObj = Object.Instantiate(prefab, spawnPosition, prefab.transform.rotation);
                }
            }
            else
            {
                // Add Pickup item
                prefab = PickupPrefabs[Mathf.RoundToInt(Random.Range(0, PickupPrefabs.Count))];
                Vector3 spawnPosition = new Vector3(x, y, prefab.transform.position.z);
                spawnedObj = Object.Instantiate(prefab, spawnPosition, prefab.transform.rotation);
            }


            // For higher difficulty, make the objects orbit around the main planet
            if (enableOrbit && spawnedObj.TryGetComponent(out
            OrbitObject orbitCenterComponent))
            {
                orbitCenterComponent.enabled = true; ;
                orbitCenterComponent.Initialize(angle, radius, planet.transform.position, rotationSpeed);

                if (enableDynamicRadius)
                {
                    orbitCenterComponent.dynamicRadius = true;
                    orbitCenterComponent.dynamicRadiusSize = dynamicRadiusSize;

                }
            }

            // Add object to chunk registry
            items.Add(spawnedObj);
        }
    }

    private GameObject SpawnObject(GameObject prefab)
    {
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

