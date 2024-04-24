using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public ItemSpawner itemSpawner;
    public PlanetSpawner planetSpawner;

    void Start()
    {
        // Initialize and manage all spawners
    }

    public void SpawnEntity(ISpawnable spawner)
    {
        spawner.Spawn();
    }
}