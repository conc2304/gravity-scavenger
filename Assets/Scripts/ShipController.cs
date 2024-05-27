using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Laser Gun Variabls
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioSource laserSoundSource;
    [SerializeField] private AudioSource respawnSoundSource;
    [SerializeField] private AudioSource thrustSoundSource;

    private float fireRate;     // Throttles how often a user can shoot
    private float fireTimer;
    private float thrust;
    [SerializeField] private ParticleSystem jetEnginePS;
    private readonly float jetEmissionRateOn = 100f;
    private readonly float jetEmissionRateOff = 0f;
    private readonly float emissionDecayTime = 0.2f;
    private float emissioRate = 0f;

    private GravityField[] gravityFields; // Reference to the GravityField script

    // Controller Variables
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject respawnAnimation;

    Vector2 mousePos;
    Vector3 playerPos;

    private void Awake()
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        fireRate = PlayerStatsManager.Instance.fireRate.currentValue;
        thrust = PlayerStatsManager.Instance.thrust.currentValue;


        // Play Spawn sound and 
        respawnSoundSource.Play();
        GameObject anim = Instantiate(respawnAnimation, transform.position, respawnAnimation.transform.rotation);
        Destroy(anim, 2f);

        // Change the Z scale of the particle system to simulate a longer engine trail for different thrust levels as players upgrade
        jetEnginePS.transform.localScale = new Vector3(jetEnginePS.transform.localScale.x, jetEnginePS.transform.localScale.y, Map(thrust, PlayerStatsManager.Instance.thrust.baseValue, PlayerStatsManager.Instance.thrust.maxValue, 0.1f, 1f));
    }


    void Update()
    {
        if (!cam) return;

        // If these variables were assigned in FixedUpdate(), they would only be updated at fixed intervals, 
        // potentially causing jerky or delayed movement.
        mousePos = Input.mousePosition;
        playerPos = cam.WorldToScreenPoint(player.position);

        // Left mouse button or Space to fire lasers 
        if ((Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

        // Start the thurst audio on mouse down and stop it on mouse up
        var emission = jetEnginePS.emission;
        float prevRate = emissioRate;
        if (Input.GetMouseButtonDown(0))
        {
            thrustSoundSource.Play();
            emissioRate = jetEmissionRateOn;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            thrustSoundSource.Pause();
            emissioRate = jetEmissionRateOff;
        }

        emission.rateOverTime = Mathf.Lerp(emissioRate, prevRate, emissionDecayTime);

        // Quit on "Q"
        if (Input.GetKey(KeyCode.Q)) Application.Quit();
    }

    // FixedUpdate is called fixed intervals determined by the physics system
    private void FixedUpdate()
    {
        // Calculate angle of the mouse in relation to the player
        // Point the player in the direction of the mouse
        Vector2 pos;
        pos.x = playerPos.x;
        pos.y = playerPos.y;

        Vector2 lookDir = mousePos - pos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f; // offset angle by 90 degress

        // Set the rotation
        Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        player.localRotation = newRotation;

        // Left click mouse to add thrust - allows for mouse being held down
        if (Input.GetMouseButton(0))
        {

            // Add thrust in the forward direction 
            if (rigidBody != null) rigidBody.AddForce(thrust * Time.deltaTime * -transform.up);

            // Deplete Fuel levels
            gameObject.GetComponent<PlayerStats>().DepleteFuel();
        }
    }

    private void Shoot()
    {
        // Shoot Laser
        GameObject laser = Instantiate(laserPrefab, firingPoint.position, firingPoint.rotation);

        // Update laser stats
        laser.GetComponent<Laser>().SetShooterTag(gameObject.tag);
        laser.GetComponent<Laser>().damage = PlayerStatsManager.Instance.damage.currentValue;
        laser.GetComponent<Laser>().range = PlayerStatsManager.Instance.fireRange.currentValue;

        // Play Laser audio
        laserSoundSource.Play();
    }

    private void OnDestroy()
    {
        gravityFields = FindObjectsOfType<GravityField>();
        foreach (GravityField gravityField in gravityFields)
        {
            // Check if the gravityField reference is not null
            if (gravityField != null && gravityField.GetComponent<Rigidbody>() != null)
            {
                // Remove the attractee to the GravityField's list of attractees
                gravityField.RemoveAttractee(gameObject);
            }
            else
            {
                Debug.LogError("Invalid GravityField object");
            }
        }
    }

    private float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        // normalize the value within the input range
        float normalizedValue = (value - inputMin) / (inputMax - inputMin);

        // scale the normalized value to the output range
        return outputMin + (normalizedValue * (outputMax - outputMin));
    }
}
