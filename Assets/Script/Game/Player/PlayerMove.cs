using DG.Tweening;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 6;
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (PlayerState.Instance.CanDash)
            {
                Dash();
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(h, 0, v).normalized;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x * speed;
        velocity.z = move.z * speed;

        rb.linearVelocity = velocity;
    }

    void Dash()
    {
        speed = 15;
        DOVirtual.DelayedCall(0.5f, () => { speed = 6; });

        PlayerState.Instance.UseDash();
    }
}
