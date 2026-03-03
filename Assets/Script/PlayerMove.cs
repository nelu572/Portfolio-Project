using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveVec = new Vector3(h, 0, v).normalized;
        rb.linearVelocity = moveVec * speed;
    }
}
