using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class ZombieEnemy : MonoBehaviour, IDamageable
{
    [Header("좀비")]
    [SerializeField] private float maxHealth = 60f;
    [SerializeField] private float moveSpeed = 2.2f;
    [SerializeField] private float attackRange = 1.35f;
    [SerializeField] private float attackDamage = 12f;
    [SerializeField] private float attackInterval = 1.1f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float playerAggroRange = 7f;
    [SerializeField] private float hitStunTime = 0.18f;
    [SerializeField] private float knockbackDecay = 8f;

    private CharacterController characterController;
    private Renderer bodyRenderer;
    private Transform player;
    private IDamageable playerHealth;
    private DefenseObjective objective;
    private WaveDirector waveDirector;
    private float currentHealth;
    private float attackTimer;
    private float hitStunTimer;
    private float flashTimer;
    private float verticalVelocity;
    private Vector3 knockbackVelocity;
    private Color baseColor;

    public bool IsAlive => currentHealth > 0f;

    public Transform TargetTransform => SelectTargetTransform();

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        bodyRenderer = GetComponent<Renderer>();
        if (bodyRenderer != null)
        {
            baseColor = bodyRenderer.material.color;
        }

        currentHealth = maxHealth;
    }

    public void Initialize(Transform playerTransform, IDamageable playerDamageable, DefenseObjective defenseObjective, WaveDirector owner)
    {
        player = playerTransform;
        playerHealth = playerDamageable;
        objective = defenseObjective;
        waveDirector = owner;
    }

    private void Update()
    {
        if (!IsAlive)
        {
            return;
        }

        attackTimer -= Time.deltaTime;
        hitStunTimer -= Time.deltaTime;
        flashTimer -= Time.deltaTime;

        UpdateHitFlash();

        var targetTransform = SelectTargetTransform();
        var targetDamageable = SelectTargetDamageable(targetTransform);
        if (targetTransform == null || targetDamageable == null || !targetDamageable.IsAlive)
        {
            return;
        }

        var toTarget = targetTransform.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude <= attackRange * attackRange)
        {
            Attack(targetDamageable);
            return;
        }

        if (hitStunTimer > 0f)
        {
            ApplyKnockbackOnly();
            return;
        }

        Move(toTarget);
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive)
        {
            return;
        }

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        hitStunTimer = hitStunTime;
        flashTimer = 0.12f;
        ApplyKnockbackFromPlayer();

        if (!IsAlive)
        {
            Die();
        }
    }

    private Transform SelectTargetTransform()
    {
        if (player != null && playerHealth != null && playerHealth.IsAlive)
        {
            var toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            if (toPlayer.sqrMagnitude <= playerAggroRange * playerAggroRange)
            {
                return player;
            }
        }

        if (objective != null && objective.IsAlive)
        {
            return objective.transform;
        }

        return player;
    }

    private IDamageable SelectTargetDamageable(Transform targetTransform)
    {
        if (objective != null && targetTransform == objective.transform)
        {
            return objective;
        }

        return playerHealth;
    }

    private void Move(Vector3 toTarget)
    {
        var direction = toTarget.normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        var velocity = direction * moveSpeed + knockbackVelocity;
        velocity.y = verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDecay * Time.deltaTime);
    }

    private void Attack(IDamageable target)
    {
        if (attackTimer > 0f)
        {
            return;
        }

        attackTimer = attackInterval;
        target.TakeDamage(attackDamage);
    }

    private void ApplyKnockbackFromPlayer()
    {
        if (player == null)
        {
            return;
        }

        var direction = transform.position - player.position;
        direction.y = 0f;
        knockbackVelocity += direction.normalized * 4.2f;
    }

    private void ApplyKnockbackOnly()
    {
        if (characterController == null)
        {
            return;
        }

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        var velocity = knockbackVelocity;
        velocity.y = verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDecay * Time.deltaTime);
    }

    private void UpdateHitFlash()
    {
        if (bodyRenderer == null)
        {
            return;
        }

        bodyRenderer.material.color = flashTimer > 0f ? new Color(1f, 0.22f, 0.12f) : baseColor;
    }

    private void Die()
    {
        waveDirector?.NotifyEnemyKilled(this);
        Destroy(gameObject);
    }
}
