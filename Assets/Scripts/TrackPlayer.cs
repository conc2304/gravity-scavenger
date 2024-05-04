using UnityEngine;

public class TrackPlayer : MonoBehaviour
{

    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    public GameObject player;

    private Transform target;
    private Vector3 vel = Vector3.zero;
    private Rigidbody rb;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        target = player.GetComponent<Transform>();
        vel = rb.velocity;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!target) GetTarget();
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    // FixedUpdate is called once per physics tick
    private void FixedUpdate()
    {
        if (!target) return;

        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z; // Keep camera in the same z position
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, damping);
    }
}
