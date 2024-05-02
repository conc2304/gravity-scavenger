
using UnityEngine;

public class OrbitObject : MonoBehaviour
{
    public float angle;
    public float radius;
    public Vector3 centerPosition;
    public float rotationSpeed;
    private float time = 0;
    //  bool isInitialized = false;

    // Update is called once per frame
    void Update()
    {
        // if (!isInitialized) return;

        float x = centerPosition.x + (radius * Mathf.Cos(angle * Mathf.Deg2Rad + time));
        float y = centerPosition.y + (radius * Mathf.Sin(angle * Mathf.Deg2Rad + time));

        time += Time.deltaTime * rotationSpeed;
        Debug.Log("Time: " + time);

        gameObject.transform.position = new Vector3(x, y, -0.5f);

    }

    public void Initialize(float angle, float radius, Vector3 centerPosition, float rotationSpeed)
    {
        this.angle = angle;
        this.radius = radius;
        this.centerPosition = centerPosition;
        this.rotationSpeed = rotationSpeed;
        // isInitialized = true;
    }
}
