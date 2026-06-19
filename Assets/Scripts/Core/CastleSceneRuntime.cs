using UnityEngine;

public sealed class CastleSceneRuntime : MonoBehaviour
{
    [Header("씬 연결")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private DefenseObjective coreObjective;
    [SerializeField] private CastleGateObjective castleGate;
    [SerializeField] private WaveDirector waveDirector;
    [SerializeField] private PrototypeHud hud;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Material zombieMaterial;

    private void Start()
    {
        if (playerRoot == null)
        {
            playerRoot = GameObject.Find("PlayerRoot")?.transform;
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (playerRoot == null || playerCamera == null || coreObjective == null || waveDirector == null)
        {
            Debug.LogWarning("CastleSceneRuntime 연결이 부족합니다.");
            return;
        }

        var playerHealth = playerRoot.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = playerRoot.gameObject.AddComponent<PlayerHealth>();
        }

        var weapon = playerCamera.GetComponent<PlayerHitscanWeapon>();
        if (weapon == null)
        {
            weapon = playerCamera.gameObject.AddComponent<PlayerHitscanWeapon>();
        }

        weapon.Initialize(playerCamera);
        waveDirector.Initialize(playerRoot, playerHealth, coreObjective, spawnPoints, zombieMaterial, castleGate);

        if (hud != null)
        {
            hud.Initialize(playerHealth, coreObjective, weapon, waveDirector, castleGate);
        }
    }
}
