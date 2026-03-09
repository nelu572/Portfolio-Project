using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private LayerMask ZombieLayer;

    [SerializeField] private float Damage = 15;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            if ((ZombieLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                Debug.Log("좀비 맞음");
                GameObject zombie = collision.gameObject;
                ZombieState zombieState = zombie.GetComponent<ZombieState>();
                zombieState.Hit(Damage);
            }
            Destroy(gameObject);
        }
    }
}
