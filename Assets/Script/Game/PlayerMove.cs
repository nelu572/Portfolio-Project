using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    Rigidbody rb;

    float h;
    float v;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(h, 0, v).normalized;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x * speed;
        velocity.z = move.z * speed;

        rb.linearVelocity = velocity;
    }
}
