using PortfolioFilling.DebugTools;
using PortfolioFilling.Defense;
using PortfolioFilling.Enemy;
using PortfolioFilling.Player;
using PortfolioFilling.Visual;
using PortfolioFilling.Weapon;
using UnityEngine;

namespace PortfolioFilling.Core
{
    public sealed class HarnessSceneInstaller : MonoBehaviour
    {
        private void Awake()
        {
            Install();
        }

        private void Install()
        {
            var systems = CreateSystemsRoot();
            var config = systems.GetComponent<ConfigManager>();
            var resourceSystem = systems.AddComponent<ResourceSystem>();
            resourceSystem.SetScrap(config.Runtime.startingScrap);
            systems.GetComponent<GameRegistry>().Register(resourceSystem);

            var objective = CreateObjective();
            systems.GetComponent<GameRegistry>().Register(objective);

            var player = CreatePlayer(config);
            systems.GetComponent<GameRegistry>().Register(player.GetComponent<PlayerHealth>());
            systems.GetComponent<GameRegistry>().Register(player.GetComponent<PlayerWeaponController>());

            var spawner = CreateEnemySpawner(player.transform, objective, config);
            var waveManager = spawner.GetComponent<WaveManager>();

            var registry = systems.GetComponent<GameRegistry>();
            registry.Register(spawner.GetComponent<EnemySpawner>());
            registry.Register(waveManager);

            CreateEnvironment();
            CreateVisualHarness();
            CreateDebugHarness(config, resourceSystem);
        }

        private GameObject CreateSystemsRoot()
        {
            var root = new GameObject("GameSystems");
            root.transform.SetParent(transform);

            var registry = root.AddComponent<GameRegistry>();
            var gameManager = root.AddComponent<GameManager>();
            var sceneLoader = root.AddComponent<SceneLoader>();
            var timeManager = root.AddComponent<TimeManager>();
            var saveManager = root.AddComponent<SaveManager>();
            var configManager = root.AddComponent<ConfigManager>();

            registry.Register(registry);
            registry.Register(gameManager);
            registry.Register(sceneLoader);
            registry.Register(timeManager);
            registry.Register(saveManager);
            registry.Register(configManager);
            return root;
        }

        private void CreateEnvironment()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            ground.transform.localScale = new Vector3(4f, 1f, 4f);
            ground.GetComponent<Renderer>().sharedMaterial.color = new Color(0.25f, 0.22f, 0.18f);

            CreateWall("NorthWall", new Vector3(0f, 2f, 19f), new Vector3(40f, 4f, 1f));
            CreateWall("SouthWall", new Vector3(0f, 2f, -19f), new Vector3(40f, 4f, 1f));
            CreateWall("EastWall", new Vector3(19f, 2f, 0f), new Vector3(1f, 4f, 40f));
            CreateWall("WestWall", new Vector3(-19f, 2f, 0f), new Vector3(1f, 4f, 40f));

            for (var i = 0; i < 4; i++)
            {
                var pipe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pipe.name = $"SteamPipe_{i}";
                pipe.transform.position = new Vector3(-10f + i * 5f, 1f, 12f);
                pipe.transform.localScale = new Vector3(0.6f, 1f + i * 0.3f, 0.6f);
                pipe.GetComponent<Renderer>().sharedMaterial.color = new Color(0.46f, 0.37f, 0.24f);
            }
        }

        private static void CreateWall(string wallName, Vector3 position, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = wallName;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial.color = new Color(0.16f, 0.15f, 0.15f);
        }

        private DefenseObjective CreateObjective()
        {
            var objectiveObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectiveObject.name = "DefenseObjective";
            objectiveObject.transform.SetParent(transform);
            objectiveObject.transform.position = new Vector3(0f, 1f, 10f);
            objectiveObject.transform.localScale = new Vector3(2.5f, 2f, 2.5f);
            objectiveObject.GetComponent<Renderer>().sharedMaterial.color = new Color(0.65f, 0.55f, 0.2f);

            var objective = objectiveObject.AddComponent<DefenseObjective>();
            var barricade = objectiveObject.AddComponent<Barricade>();
            objective.SetBarricade(barricade);
            return objective;
        }

        private GameObject CreatePlayer(ConfigManager config)
        {
            var playerRoot = new GameObject("PlayerRoot");
            playerRoot.transform.SetParent(transform);
            playerRoot.transform.position = new Vector3(0f, 1.1f, -8f);

            var controller = playerRoot.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.35f;
            controller.center = new Vector3(0f, 0.9f, 0f);

            var input = playerRoot.AddComponent<PlayerInputReader>();
            var movement = playerRoot.AddComponent<PlayerMovement>();
            var health = playerRoot.AddComponent<PlayerHealth>();
            var interactor = playerRoot.AddComponent<PlayerInteractor>();
            var weaponController = playerRoot.AddComponent<PlayerWeaponController>();
            var look = playerRoot.AddComponent<PlayerLook>();

            var cameraObject = Camera.main != null ? Camera.main.gameObject : new GameObject("Main Camera");
            var camera = cameraObject.GetComponent<Camera>() ?? cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
            cameraObject.transform.SetParent(playerRoot.transform);
            cameraObject.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            cameraObject.transform.localRotation = Quaternion.identity;

            movement.Initialize(input, controller, config);
            health.Initialize(config.Runtime.playerMaxHealth);
            interactor.Initialize(input, camera.transform, config.Runtime.interactRange);
            look.Initialize(input, playerRoot.transform, camera.transform, config);
            weaponController.Initialize(input, camera.transform);
            AttachStarterWeapon(weaponController);

            return playerRoot;
        }

        private static void AttachStarterWeapon(PlayerWeaponController weaponController)
        {
            var weaponObject = new GameObject("RustyRifle");
            weaponObject.transform.SetParent(weaponController.transform);
            weaponObject.transform.localPosition = new Vector3(0.28f, -0.18f, 0.45f);
            weaponObject.transform.localRotation = Quaternion.identity;

            var weapon = weaponObject.AddComponent<HitscanWeapon>();
            weapon.Configure(new WeaponData
            {
                displayName = "Rusty Rifle",
                damage = 20f,
                fireInterval = 0.18f,
                clipSize = 8,
                reserveAmmo = 48,
                reloadDuration = 1.3f,
                range = 55f
            });

            weaponController.AddWeapon(weapon);
        }

        private GameObject CreateEnemySpawner(Transform player, DefenseObjective objective, ConfigManager config)
        {
            var spawnerObject = new GameObject("EnemySystems");
            spawnerObject.transform.SetParent(transform);

            var spawner = spawnerObject.AddComponent<EnemySpawner>();
            var waveManager = spawnerObject.AddComponent<WaveManager>();

            spawner.Initialize(player, objective, config);
            waveManager.Initialize(spawner);
            return spawnerObject;
        }

        private void CreateVisualHarness()
        {
            var settingsObject = new GameObject("VisualHarness");
            settingsObject.transform.SetParent(transform);
            settingsObject.AddComponent<PsxVisualSettings>();

            var flickerLightObject = new GameObject("FlickerLight");
            flickerLightObject.transform.SetParent(transform);
            flickerLightObject.transform.position = new Vector3(0f, 4f, 8f);
            var light = flickerLightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.72f, 0.45f);
            light.range = 18f;
            light.intensity = 1.5f;
            flickerLightObject.AddComponent<FlickerLight>();
        }

        private void CreateDebugHarness(ConfigManager config, ResourceSystem resourceSystem)
        {
            var debugObject = new GameObject("DebugHarness");
            debugObject.transform.SetParent(transform);

            var overlay = debugObject.AddComponent<DebugOverlay>();
            overlay.Initialize(config, resourceSystem);

            debugObject.AddComponent<DebugCheatController>();
        }
    }
}
