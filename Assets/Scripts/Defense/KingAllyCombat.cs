using UnityEngine;

public sealed class KingAllyCombat : MonoBehaviour
{
    [Header("왕 전투")]
    [SerializeField] private float scanRadius = 9f;
    [SerializeField] private float fireInterval = 0.75f;
    [SerializeField] private float damage = 18f;
    [SerializeField] private float rotateSpeed = 7f;

    private float fireTimer;
    private Transform riflePivot;

    private void Start()
    {
        riflePivot = transform.Find("King_SteamRifle");
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;

        var target = FindTarget();
        if (target == null)
        {
            return;
        }

        RotateToward(target.transform.position);

        if (fireTimer > 0f)
        {
            return;
        }

        fireTimer = fireInterval;
        target.TakeDamage(damage);
        SpawnKingShot(target.transform.position);
    }

    private ZombieEnemy FindTarget()
    {
        var candidates = Object.FindObjectsByType<ZombieEnemy>(FindObjectsSortMode.None);
        ZombieEnemy bestTarget = null;
        var bestDistance = scanRadius * scanRadius;

        foreach (var candidate in candidates)
        {
            if (candidate == null || !candidate.IsAlive)
            {
                continue;
            }

            var distance = (candidate.transform.position - transform.position).sqrMagnitude;
            if (distance > bestDistance)
            {
                continue;
            }

            bestDistance = distance;
            bestTarget = candidate;
        }

        return bestTarget;
    }

    private void RotateToward(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude <= 0.01f)
        {
            return;
        }

        var targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void SpawnKingShot(Vector3 targetPosition)
    {
        var origin = riflePivot != null ? riflePivot.position : transform.position + Vector3.up * 1.3f;
        var tracer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tracer.name = "KingShotTracer";

        var midpoint = (origin + targetPosition) * 0.5f;
        var direction = targetPosition - origin;
        tracer.transform.position = midpoint;
        tracer.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        tracer.transform.localScale = new Vector3(0.045f, 0.045f, direction.magnitude);

        var renderer = tracer.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial(new Color(0.65f, 0.9f, 1f));

        Destroy(tracer.GetComponent<Collider>());
        Destroy(tracer, 0.08f);
    }

    private static Material CreateMaterial(Color color)
    {
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        return material;
    }
}
