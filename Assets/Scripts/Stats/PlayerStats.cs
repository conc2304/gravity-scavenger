using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Derive Player Stats from entity Stats
public class PlayerStats : EntityStats
{

    // only main player is concerned with shield, fuel, and money
    public Stat shield;
    public Stat fuel;
    public Stat money;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
