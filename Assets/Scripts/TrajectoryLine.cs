using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private Transform trajectoryStart;

    [Header("Trajectory Line Smoothnes/Length")]
    [SerializeField] private int segmentCount = 50;
    [SerializeField] private float curveLength = 3.5f;
    private float prevNumDashes = 1f;

    private Vector3[] segments;
    private LineRenderer lineRenderer;
    private Material shader;


    // Player Vars
    private GameObject player;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        // get player object
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        shader = GetComponent<Renderer>().material;

        // initialize segments
        segments = new Vector3[segmentCount];

        // get line renderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;
    }

    // Update is called once per frame
    void Update()
    {
        // set the starting position of the line
        Vector3 startPos = trajectoryStart.position;
        // Vector3 startPos = player.transform.position;
        segments[0] = startPos;
        lineRenderer.SetPosition(0, startPos);

        // set the starting velocity based on player physics
        Vector3 startVelocity = rb.velocity;
        float totalLength = 0f; //  total length of the line

        for (int i = 1; i < segmentCount; i++)
        {
            // Calculate the distance between the current point and the previous point
            float segmentLength = Vector3.Distance(segments[i], segments[i - 1]);
            // Add the distance to the total length
            totalLength += segmentLength;


            float timeOffset = i * Time.fixedDeltaTime * curveLength;
            // compute the gravity offset
            Vector3 gravityOffset = new Vector3(0, 0, 0); // TODO - update gravity here
            // set the position of the point in the line renderer
            segments[i] = segments[0] + startVelocity * timeOffset + gravityOffset;
            lineRenderer.SetPosition(i, segments[i]);
        }

        // enable or disable the Line Renderer based on the total length, otherwise we have a weird blinking dot for a nose
        lineRenderer.enabled = totalLength >= 0.25f;

        // when the line is long render more dashes and vice versa
        // at its longest it should have 5 dashes, at its shortest half of a dash 
        float numShaderDashes = Map(totalLength, 1f, 25f, 0.25f, 5f);
        float lerpedValue = Mathf.Lerp(prevNumDashes, numShaderDashes, 0.1f); // interpolate for smoothness
        prevNumDashes = numShaderDashes;
        // this is the Reference our exposed property on the TrajectoryLine shader material which control the x-tiling

        shader.SetFloat("_Number_Of_Dashes", lerpedValue);
    }

    private float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        // normalize the value within the input range
        float normalizedValue = (value - inputMin) / (inputMax - inputMin);

        // scale the normalized value to the output range
        return outputMin + (normalizedValue * (outputMax - outputMin));
    }
}
