using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPrediction : MonoBehaviour
{

    [SerializeField] private GameObject TrajectoryPrefab;

    private Quaternion rotation = Quaternion.identity;
    private Vector3 position;


    public int trajectoryLength = 5;
    public int trajectoryStep = 5;

    private readonly List<GameObject> trajectoryPoints = new();
    private GravityField[] gravityFields; // Reference to the GravityField script

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < trajectoryStep; i++)
        {
            GameObject trajPoint = Instantiate(TrajectoryPrefab, position, rotation);
            trajectoryPoints.Add(trajPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gravityFields = FindObjectsOfType<GravityField>();
    }

    private void FixedUpdate()
    {

    }
}
