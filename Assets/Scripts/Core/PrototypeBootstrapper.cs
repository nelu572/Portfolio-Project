using UnityEngine;
using UnityEngine.SceneManagement;

public static class PrototypeBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            return;
        }

        var playerRoot = GameObject.Find("PlayerRoot");
        var camera = Camera.main;
        if (playerRoot == null || camera == null || Object.FindFirstObjectByType<WaveDirector>() != null)
        {
            return;
        }

        var materials = CreateMaterials();
        var playerHealth = EnsurePlayerHealth(playerRoot);
        var weapon = EnsureWeapon(camera);
        var objective = CreateKingObjective(materials);
        var spawnPoints = CreateSpawnPoints();

        CreatePrototypeProps(materials);

        var directorObject = new GameObject("PrototypeWaveDirector");
        var director = directorObject.AddComponent<WaveDirector>();
        director.Initialize(playerRoot.transform, playerHealth, objective, spawnPoints, materials.zombie);

        var hudObject = new GameObject("PrototypeHud");
        var hud = hudObject.AddComponent<PrototypeHud>();
        hud.Initialize(playerHealth, objective, weapon, director);
    }

    private static PlayerHealth EnsurePlayerHealth(GameObject playerRoot)
    {
        var playerHealth = playerRoot.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = playerRoot.AddComponent<PlayerHealth>();
        }

        return playerHealth;
    }

    private static PlayerHitscanWeapon EnsureWeapon(Camera camera)
    {
        var weapon = camera.GetComponent<PlayerHitscanWeapon>();
        if (weapon == null)
        {
            weapon = camera.gameObject.AddComponent<PlayerHitscanWeapon>();
        }

        weapon.Initialize(camera);
        CreateViewWeapon(camera.transform);
        return weapon;
    }

    private static DefenseObjective CreateKingObjective((Material brass, Material zombie, Material darkMetal, Material red, Material royalBlue, Material stone) materials)
    {
        var objectiveObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        objectiveObject.name = "King_ProtectedTarget";
        objectiveObject.transform.position = new Vector3(0f, 1.1f, 0f);
        objectiveObject.transform.localScale = new Vector3(0.78f, 1.05f, 0.78f);
        objectiveObject.GetComponent<Renderer>().sharedMaterial = materials.royalBlue;

        var crown = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        crown.name = "King_Crown";
        crown.transform.SetParent(objectiveObject.transform, false);
        crown.transform.localPosition = new Vector3(0f, 1.15f, 0f);
        crown.transform.localScale = new Vector3(0.55f, 0.12f, 0.55f);
        crown.GetComponent<Renderer>().sharedMaterial = materials.brass;
        Object.Destroy(crown.GetComponent<Collider>());

        var kingRifle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        kingRifle.name = "King_SteamRifle";
        kingRifle.transform.SetParent(objectiveObject.transform, false);
        kingRifle.transform.localPosition = new Vector3(0.42f, 0.24f, 0.48f);
        kingRifle.transform.localRotation = Quaternion.Euler(0f, -22f, 0f);
        kingRifle.transform.localScale = new Vector3(0.14f, 0.12f, 0.72f);
        kingRifle.GetComponent<Renderer>().sharedMaterial = materials.darkMetal;
        Object.Destroy(kingRifle.GetComponent<Collider>());

        CreateBox("RoyalDefensePlate", new Vector3(0f, 0.08f, 0f), new Vector3(4.4f, 0.16f, 4.4f), materials.stone);

        objectiveObject.AddComponent<KingAllyCombat>();
        return objectiveObject.AddComponent<DefenseObjective>();
    }

    private static Transform[] CreateSpawnPoints()
    {
        var positions = new[]
        {
            new Vector3(0f, 1.1f, 13.2f),
            new Vector3(-13.2f, 1.1f, 2.2f),
            new Vector3(13.2f, 1.1f, 2.2f)
        };

        var spawnPoints = new Transform[positions.Length];
        for (var i = 0; i < positions.Length; i++)
        {
            var spawnPoint = new GameObject($"EnemySpawn_{i + 1:00}");
            spawnPoint.transform.position = positions[i];
            spawnPoints[i] = spawnPoint.transform;
        }

        return spawnPoints;
    }

    private static void CreateViewWeapon(Transform cameraTransform)
    {
        if (cameraTransform.Find("SteamRevolver") != null)
        {
            return;
        }

        var revolver = GameObject.CreatePrimitive(PrimitiveType.Cube);
        revolver.name = "SteamRevolver";
        revolver.transform.SetParent(cameraTransform, false);
        revolver.transform.localPosition = new Vector3(0.34f, -0.3f, 0.62f);
        revolver.transform.localRotation = Quaternion.Euler(0f, -7f, 0f);
        revolver.transform.localScale = new Vector3(0.18f, 0.16f, 0.52f);
        Object.Destroy(revolver.GetComponent<Collider>());

        var barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        barrel.name = "SteamRevolver_Barrel";
        barrel.transform.SetParent(cameraTransform, false);
        barrel.transform.localPosition = new Vector3(0.34f, -0.28f, 0.95f);
        barrel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        barrel.transform.localScale = new Vector3(0.07f, 0.24f, 0.07f);
        Object.Destroy(barrel.GetComponent<Collider>());

        var leftArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftArm.name = "MechanicalLeftArm";
        leftArm.transform.SetParent(cameraTransform, false);
        leftArm.transform.localPosition = new Vector3(-0.34f, -0.34f, 0.58f);
        leftArm.transform.localRotation = Quaternion.Euler(0f, 9f, 0f);
        leftArm.transform.localScale = new Vector3(0.22f, 0.2f, 0.68f);
        Object.Destroy(leftArm.GetComponent<Collider>());
    }

    private static void CreatePrototypeProps((Material brass, Material zombie, Material darkMetal, Material red, Material royalBlue, Material stone) materials)
    {
        CreateBox("Castle_BackKeep", new Vector3(0f, 2.2f, -10.8f), new Vector3(9.5f, 4.4f, 0.55f), materials.stone);
        CreateBox("Castle_LeftTower", new Vector3(-8.4f, 2.8f, -9.4f), new Vector3(2.2f, 5.6f, 2.2f), materials.stone);
        CreateBox("Castle_RightTower", new Vector3(8.4f, 2.8f, -9.4f), new Vector3(2.2f, 5.6f, 2.2f), materials.stone);

        CreateGate("NorthInvasionGate", new Vector3(0f, 1.9f, 11.8f), new Vector3(5.6f, 3.8f, 0.38f), materials.darkMetal, materials.red);
        CreateGate("WestInvasionGate", new Vector3(-12.4f, 1.85f, 2f), new Vector3(0.38f, 3.7f, 5.2f), materials.darkMetal, materials.red);
        CreateGate("EastInvasionGate", new Vector3(12.4f, 1.85f, 2f), new Vector3(0.38f, 3.7f, 5.2f), materials.darkMetal, materials.red);

        CreateBox("LowCover_NorthLeft", new Vector3(-4.8f, 0.55f, 5.5f), new Vector3(2.6f, 1.1f, 0.7f), materials.stone);
        CreateBox("LowCover_NorthRight", new Vector3(4.8f, 0.55f, 5.5f), new Vector3(2.6f, 1.1f, 0.7f), materials.stone);
        CreateBox("LowCover_West", new Vector3(-6.5f, 0.55f, -1.8f), new Vector3(0.8f, 1.1f, 3.1f), materials.stone);
        CreateBox("LowCover_East", new Vector3(6.5f, 0.55f, -1.8f), new Vector3(0.8f, 1.1f, 3.1f), materials.stone);

        CreateBox("BoilerBlock_A", new Vector3(-8.4f, 1.15f, 5.6f), new Vector3(1.45f, 2.3f, 1.45f), materials.darkMetal);
        CreateBox("BoilerBlock_B", new Vector3(8.4f, 1.15f, 5.6f), new Vector3(1.45f, 2.3f, 1.45f), materials.darkMetal);
        CreateBox("PressureTank_West", new Vector3(-9.2f, 1.6f, -3.6f), new Vector3(0.95f, 3.2f, 0.95f), materials.brass);
        CreateBox("PressureTank_East", new Vector3(9.2f, 1.6f, -3.6f), new Vector3(0.95f, 3.2f, 0.95f), materials.brass);

        CreateBox("Pipe_NorthLine", new Vector3(0f, 2.2f, 7.2f), new Vector3(9.5f, 0.2f, 0.2f), materials.brass);
        CreateBox("Pipe_WestLine", new Vector3(-8.7f, 1.85f, 1.1f), new Vector3(0.2f, 0.2f, 7.2f), materials.brass);
        CreateBox("Pipe_EastLine", new Vector3(8.7f, 1.85f, 1.1f), new Vector3(0.2f, 0.2f, 7.2f), materials.brass);

        CreateBox("RaisedWalkway_Left", new Vector3(-5.8f, 1.05f, -6.4f), new Vector3(3.6f, 0.36f, 2.2f), materials.stone);
        CreateBox("RaisedWalkway_Right", new Vector3(5.8f, 1.05f, -6.4f), new Vector3(3.6f, 0.36f, 2.2f), materials.stone);

        CreateBox("RoyalWarningLight", new Vector3(0f, 2.9f, 0f), new Vector3(0.48f, 0.48f, 0.48f), materials.red);
        CreateBox("RoyalBanner_Left", new Vector3(-2.4f, 3.05f, -10.45f), new Vector3(0.62f, 1.9f, 0.12f), materials.royalBlue);
        CreateBox("RoyalBanner_Right", new Vector3(2.4f, 3.05f, -10.45f), new Vector3(0.62f, 1.9f, 0.12f), materials.royalBlue);
    }

    private static void CreateGate(string name, Vector3 position, Vector3 scale, Material gateMaterial, Material warningMaterial)
    {
        CreateBox(name, position, scale, gateMaterial);
        CreateBox($"{name}_WarningLeft", position + new Vector3(scale.x > scale.z ? -2.2f : 0f, 2.35f, scale.z > scale.x ? -2f : 0f), Vector3.one * 0.32f, warningMaterial);
        CreateBox($"{name}_WarningRight", position + new Vector3(scale.x > scale.z ? 2.2f : 0f, 2.35f, scale.z > scale.x ? 2f : 0f), Vector3.one * 0.32f, warningMaterial);
    }

    private static void CreateBox(string name, Vector3 position, Vector3 scale, Material material)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.position = position;
        box.transform.localScale = scale;
        box.GetComponent<Renderer>().sharedMaterial = material;
    }

    private static (Material brass, Material zombie, Material darkMetal, Material red, Material royalBlue, Material stone) CreateMaterials()
    {
        var brass = CreateMaterial("Prototype_Brass", new Color(0.62f, 0.45f, 0.18f));
        var zombie = CreateMaterial("Prototype_CitizenZombie", new Color(0.24f, 0.38f, 0.25f));
        var darkMetal = CreateMaterial("Prototype_DarkMetal", new Color(0.09f, 0.1f, 0.1f));
        var red = CreateMaterial("Prototype_Red", new Color(0.75f, 0.06f, 0.03f));
        var royalBlue = CreateMaterial("Prototype_RoyalBlue", new Color(0.12f, 0.14f, 0.36f));
        var stone = CreateMaterial("Prototype_CastleStone", new Color(0.33f, 0.34f, 0.32f));
        return (brass, zombie, darkMetal, red, royalBlue, stone);
    }

    private static Material CreateMaterial(string name, Color color)
    {
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.name = name;
        material.color = color;
        return material;
    }
}
