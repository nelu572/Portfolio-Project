using System.Collections.Generic;
using UnityEngine;

public sealed class WaveDirector : MonoBehaviour
{
    [Header("웨이브")]
    [SerializeField] private int baseEnemyCount = 4;
    [SerializeField] private int enemyIncreasePerWave = 2;
    [SerializeField] private float timeBetweenWaves = 4f;
    [SerializeField] private float spawnInterval = 0.85f;

    private readonly List<ZombieEnemy> aliveEnemies = new();
    private Transform player;
    private IDamageable playerHealth;
    private DefenseObjective objective;
    private Transform[] spawnPoints;
    private Material enemyMaterial;
    private int currentWave;
    private int enemiesRemainingToSpawn;
    private float nextWaveTimer;
    private float spawnTimer;
    private bool gameOver;
    private string waveMessage = "왕을 지켜라";
    private float waveMessageTimer = 2.5f;

    public int CurrentWave => currentWave;

    public int AliveEnemyCount => aliveEnemies.Count;

    public int EnemiesRemainingToSpawn => enemiesRemainingToSpawn;

    public bool GameOver => gameOver;

    public string WaveMessage => waveMessageTimer > 0f ? waveMessage : string.Empty;

    public float NextWaveTimer => Mathf.Max(0f, nextWaveTimer);

    public void Initialize(
        Transform playerTransform,
        IDamageable playerDamageable,
        DefenseObjective defenseObjective,
        Transform[] enemySpawnPoints,
        Material zombieMaterial)
    {
        player = playerTransform;
        playerHealth = playerDamageable;
        objective = defenseObjective;
        spawnPoints = enemySpawnPoints;
        enemyMaterial = zombieMaterial;
        nextWaveTimer = 1f;
        waveMessage = "왕을 지켜라";
        waveMessageTimer = 2.5f;
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        waveMessageTimer -= Time.deltaTime;

        if (objective == null || !objective.IsAlive || playerHealth == null || !playerHealth.IsAlive)
        {
            gameOver = true;
            waveMessage = objective == null || !objective.IsAlive ? "왕이 쓰러졌다" : "플레이어 사망";
            waveMessageTimer = 999f;
            return;
        }

        if (enemiesRemainingToSpawn > 0)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                enemiesRemainingToSpawn--;
                spawnTimer = spawnInterval;
            }
        }

        if (aliveEnemies.Count > 0 || enemiesRemainingToSpawn > 0)
        {
            return;
        }

        nextWaveTimer -= Time.deltaTime;
        if (nextWaveTimer <= 0f)
        {
            StartNextWave();
        }
    }

    public void NotifyEnemyKilled(ZombieEnemy enemy)
    {
        aliveEnemies.Remove(enemy);

        if (aliveEnemies.Count == 0 && enemiesRemainingToSpawn == 0)
        {
            nextWaveTimer = timeBetweenWaves;
            waveMessage = $"웨이브 {currentWave} 정리";
            waveMessageTimer = 2f;
        }
    }

    private void StartNextWave()
    {
        currentWave++;
        enemiesRemainingToSpawn = baseEnemyCount + ((currentWave - 1) * enemyIncreasePerWave);
        spawnTimer = 0f;
        waveMessage = $"웨이브 {currentWave} 시작";
        waveMessageTimer = 2.25f;
    }

    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return;
        }

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var enemyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemyObject.name = $"CitizenZombie_Wave{currentWave:00}";
        enemyObject.transform.position = spawnPoint.position;
        enemyObject.transform.localScale = new Vector3(0.85f, 1.15f, 0.85f);

        if (enemyMaterial != null)
        {
            enemyObject.GetComponent<Renderer>().sharedMaterial = enemyMaterial;
        }

        var controller = enemyObject.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.42f;
        controller.center = new Vector3(0f, 1f, 0f);

        var enemy = enemyObject.AddComponent<ZombieEnemy>();
        enemy.Initialize(player, playerHealth, objective, this);
        aliveEnemies.Add(enemy);
    }
}
