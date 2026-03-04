using UnityEngine;

public class PlayerLookMouse : MonoBehaviour
{
    [SerializeField] private GameObject camPoint;
    private Vector3 camPos = new Vector3();

    [SerializeField] private Camera cam;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            Vector3 offset = targetPoint - transform.position;
            offset.y = 0f;

            float maxDistance = 10f;
            offset = Vector3.ClampMagnitude(offset, maxDistance);

            camPos = transform.position + offset * 0.3f;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    void LateUpdate()
    {
        camPoint.transform.position = camPos;
    }
}