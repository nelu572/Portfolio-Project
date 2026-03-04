using Unity.VisualScripting;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;

    [SerializeField] private float bulletSpeed = 7;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(bulletRb.transform.forward * bulletSpeed, ForceMode.Impulse);
    }
}
