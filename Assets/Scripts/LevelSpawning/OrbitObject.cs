
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    public float angle;
    public float radius;
    public Vector3 centerPosition;
    public float rotationSpeed;
    private float time = 0;
    public bool dynamicRadius = false;
    public float dynamicRadiusSize = 1f;
    private bool isInitialized = false;

    // Update is called once per frame
    void Update()
    {

        if (dynamicRadius)
        {
            radius *= Map(Mathf.Sin(time), -1, 1, radius / dynamicRadiusSize, radius * dynamicRadiusSize);
        }

        if (!isInitialized) return;

        float x = centerPosition.x + (radius * Mathf.Cos(angle * Mathf.Deg2Rad + time));
        float y = centerPosition.y + (radius * Mathf.Sin(angle * Mathf.Deg2Rad + time));

        time += Time.deltaTime * rotationSpeed;

        gameObject.transform.position = new Vector3(x, y, -0.5f);

    }

    public void Initialize(float angle, float radius, Vector3 centerPosition, float rotationSpeed)
    {
        this.angle = angle;
        this.radius = radius;
        this.centerPosition = centerPosition;
        this.rotationSpeed = rotationSpeed;
        isInitialized = true;
    }

    private float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        // normalize the value within the input range
        float normalizedValue = (value - inputMin) / (inputMax - inputMin);

        // scale the normalized value to the output range
        return outputMin + (normalizedValue * (outputMax - outputMin));
    }
}
