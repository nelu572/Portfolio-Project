using UnityEngine;

public class DeleteBullet : MonoBehaviour
{

    [SerializeField] private LayerMask targetLayers;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
