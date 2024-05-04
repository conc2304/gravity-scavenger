using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private Transform trajectoryStart;

    [Header("Trajectory Line Smoothnes/Length")]
    [SerializeField] private int segmentCount = 30;
    [SerializeField] private float curveLength = 0.35f;
    private float prevNumDashes = 1f;

    private Vector3[] segments;
    private LineRenderer lineRenderer;
    private Material shader;

    private GravityField[] gravityFields; // Reference to the GravityField script
    private bool inGravityField = false;
    private bool lineCollidesWithPlanet = false;

    // Player Vars
    private GameObject player;
    private Rigidbody playerRb;
    private float playerMass;

    // Start is called before the first frame update
    void Start()
    {
        // Get player object mass
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
        playerMass = playerRb.mass;

        // Get line shader
        shader = GetComponent<Renderer>().material;

        // Initialize line segments
        segments = new Vector3[segmentCount];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset flags
        inGravityField = false;
        lineCollidesWithPlanet = false;

        // Set the starting position of the line
        Vector3 startPos = trajectoryStart.position;
        segments[0] = startPos;
        lineRenderer.SetPosition(0, startPos);

        // Set the starting velocity based on player physics
        Vector3 startVelocity = playerRb.velocity;

        float totalLength = 0f; //  Accumulator for line length

        for (int i = 1; i < segmentCount; i++)
        {

            // To prevent really long segments that are flat, cut the timeOffset down when velocity is large
            float timeOffset = i * Time.fixedDeltaTime * curveLength;
            float maxOffset = 1f;
            if (timeOffset > maxOffset) timeOffset /= 2f;

            // Compute the gravity offset
            Vector3 gravityAcceleration = GetGravityOffset(segments[i - 1]) / playerMass;
            gravityAcceleration.z = 0; // never add z force

            // Set the position of the point in the line renderer
            // Calculate the position of an object under constant acceleration
            segments[i] = segments[i - 1] + startVelocity * timeOffset + gravityAcceleration * (Mathf.Pow(timeOffset, 2) / 2);

            // Set start velocity of the player trajectory for the next loop
            startVelocity = (segments[i] - segments[i - 1]) / timeOffset;

            // Calculate the distance between the current point and the previous point
            float segmentLength = Vector3.Distance(segments[i], segments[i - 1]);
            totalLength += segmentLength;

            bool pointOutOfBounds = IsPointOutOfBounds(segments[i]);
            if (pointOutOfBounds || lineCollidesWithPlanet)
            {
                // If new point is invalid just repeat the last one
                // to fill the rest of the line segments
                for (int j = i; j < segmentCount; j++)
                {
                    lineRenderer.SetPosition(j, segments[i - 1]);
                }
                lineRenderer.SetPosition(segmentCount - 1, segments[i - 1]);

                break;
            }
            else
            {
                lineRenderer.SetPosition(i, segments[i]);
            }
        }

        // Enable or disable the Line Renderer based on the total length, otherwise we have a weird blinking dot for a nose
        lineRenderer.enabled = totalLength >= 0.1f && inGravityField;

        UpdateTrajectoryShader(totalLength);
    }



    private Vector3 GetGravityOffset(Vector3 prevSegmentPosition)
    {
        Vector3 totalForce = new Vector3(0, 0, 0);
        gravityFields = FindObjectsOfType<GravityField>();

        // Apply the gravitational force of each gravity field to our force vector
        foreach (var gravityField in gravityFields)
        {
            if (gravityField == null) continue;

            Vector3 force = GetForce(prevSegmentPosition, gravityField);
            // If trajectory never passes througha a gravity field
            // disable the trajectory line
            if (force.magnitude > 0)
            {
                inGravityField = true;
            }
            totalForce += force;
        }

        return totalForce;
    }


    public Vector3 GetForce(Vector3 segmentPosition, GravityField gf)
    {
        Vector3 force;
        Rigidbody planetRb = gf.GetComponent<Rigidbody>();
        Collider collider = gf.GetComponent<MeshCollider>();
        Vector3 direction = planetRb.position - segmentPosition;
        float distance = direction.magnitude;

        // Check if our trajectory line has collided with the planet's rigid body
        if (collider.bounds.Contains(segmentPosition))
        {
            Debug.Log("Planet Collision Trajectory");
            lineCollidesWithPlanet = true;
        }

        // don't apply force if it is too far or if it collides 
        if (distance == 0f || distance > gf.maxDistance || lineCollidesWithPlanet)
        {
            force = new Vector3(0, 0, 0);
        }
        else
        {
            float forceMagnitude = planetRb.mass * playerMass / Mathf.Pow(distance, 2);
            force = direction.normalized * forceMagnitude;
        }

        return force;
    }

    private bool IsPointOutOfBounds(Vector3 segmentPos)
    {
        // Validate segment point
        // Use the bottom-left of screen to calculate out of bounds
        float cameraDepth = Mathf.Abs(Camera.main.transform.position.z); // Assumes our game happens at z = 0
        Vector3 segPt = Camera.main.WorldToScreenPoint(segmentPos);
        Vector3 worldBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, cameraDepth));
        bool pointOutOfBounds = segPt.x > Screen.width || segPt.y > Screen.height || segPt.x < worldBottomLeft.x || segPt.y < worldBottomLeft.y;
        return pointOutOfBounds;
    }

    private void UpdateTrajectoryShader(float totalLength)
    {
        // When the line is long, render more dashes and vice versa
        // At its longest it should have 5 dashes, at its shortest half of a dash 
        float numShaderDashes = Map(totalLength, 1f, 25f, 0.25f, 5f);
        float lerpedValue = Mathf.Lerp(prevNumDashes, numShaderDashes, 0.1f); // interpolate for smoothness
        prevNumDashes = numShaderDashes;
        // Set the x tiling of ths shader via number of dashes prop
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
