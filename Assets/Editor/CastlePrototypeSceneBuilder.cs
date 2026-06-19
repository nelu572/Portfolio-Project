using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CastlePrototypeSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/CastlePrototypeScene.unity";
    private const string SourceScenePath = "Assets/Scenes/GameScene.unity";

    [MenuItem("Tools/Prototype/Build Castle Prototype Scene")]
    public static void BuildScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "CastlePrototypeScene";

        var materials = CreateMaterials();
        var root = new GameObject("CastlePrototypeMap");

        CreateLighting(root.transform);
        CreateGround(root.transform, materials);
        CreateCastle(root.transform, materials);
        CreateField(root.transform, materials);

        var coreObjective = CreateCoreObjective(root.transform, materials);
        var gateObjective = CreateGateObjective(root.transform, materials);
        var spawnPoints = CreateSpawnPoints(root.transform);
        var grapplePoints = CreateGrapplePoints(root.transform, materials);

        var playerRoot = CopyPlayerFromGameScene();
        SetupPlayer(playerRoot, grapplePoints);
        var playerCamera = Camera.main;
        SetupViewWeapon(playerCamera.transform, materials);

        var waveDirector = new GameObject("WaveDirector").AddComponent<WaveDirector>();
        var hud = new GameObject("PrototypeHud").AddComponent<PrototypeHud>();
        var runtime = new GameObject("CastleSceneRuntime").AddComponent<CastleSceneRuntime>();
        ConfigureRuntime(runtime, playerRoot.transform, playerCamera, coreObjective, gateObjective, waveDirector, hud, spawnPoints, materials.zombie);

        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, ScenePath);
        UpdateBuildSettings();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static GameObject CopyPlayerFromGameScene()
    {
        var castleScene = SceneManager.GetActiveScene();
        var sourceScene = EditorSceneManager.OpenScene(SourceScenePath, OpenSceneMode.Additive);
        var sourcePlayer = GameObject.Find("PlayerRoot");
        if (sourcePlayer == null)
        {
            throw new FileNotFoundException("GameScene에서 PlayerRoot를 찾지 못했습니다.");
        }

        var playerCopy = Object.Instantiate(sourcePlayer);
        playerCopy.name = "PlayerRoot";
        SceneManager.MoveGameObjectToScene(playerCopy, castleScene);
        EditorSceneManager.CloseScene(sourceScene, true);
        return playerCopy;
    }

    private static void SetupPlayer(GameObject playerRoot, Transform[] grapplePoints)
    {
        playerRoot.transform.position = new Vector3(0f, 1.2f, -5.5f);
        playerRoot.transform.rotation = Quaternion.identity;

        if (playerRoot.GetComponent<PlayerHealth>() == null)
        {
            playerRoot.AddComponent<PlayerHealth>();
        }

        var grapplingHook = playerRoot.GetComponent<GrapplingHookPrototype>();
        if (grapplingHook == null)
        {
            grapplingHook = playerRoot.AddComponent<GrapplingHookPrototype>();
        }

        grapplingHook.SetGrapplePoints(grapplePoints);
    }

    private static void ConfigureRuntime(
        CastleSceneRuntime runtime,
        Transform playerRoot,
        Camera playerCamera,
        DefenseObjective coreObjective,
        CastleGateObjective gateObjective,
        WaveDirector waveDirector,
        PrototypeHud hud,
        Transform[] spawnPoints,
        Material zombieMaterial)
    {
        var serialized = new SerializedObject(runtime);
        serialized.FindProperty("playerRoot").objectReferenceValue = playerRoot;
        serialized.FindProperty("playerCamera").objectReferenceValue = playerCamera;
        serialized.FindProperty("coreObjective").objectReferenceValue = coreObjective;
        serialized.FindProperty("castleGate").objectReferenceValue = gateObjective;
        serialized.FindProperty("waveDirector").objectReferenceValue = waveDirector;
        serialized.FindProperty("hud").objectReferenceValue = hud;
        serialized.FindProperty("zombieMaterial").objectReferenceValue = zombieMaterial;

        var spawnProperty = serialized.FindProperty("spawnPoints");
        spawnProperty.arraySize = spawnPoints.Length;
        for (var i = 0; i < spawnPoints.Length; i++)
        {
            spawnProperty.GetArrayElementAtIndex(i).objectReferenceValue = spawnPoints[i];
        }

        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void CreateLighting(Transform parent)
    {
        var lightObject = new GameObject("ColdSun");
        lightObject.transform.SetParent(parent);
        lightObject.transform.rotation = Quaternion.Euler(48f, -28f, 0f);
        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.15f;

    }

    private static void CreateGround(Transform parent, Materials materials)
    {
        CreateBox("OuterField", parent, new Vector3(0f, -0.08f, 8f), new Vector3(44f, 0.16f, 36f), materials.grass);
        CreateBox("CastleCourtyard", parent, new Vector3(0f, 0.02f, -4f), new Vector3(22f, 0.18f, 18f), materials.dirt);
        CreateBox("MainRoad", parent, new Vector3(0f, 0.08f, 8.5f), new Vector3(4.2f, 0.08f, 20f), materials.road);
    }

    private static void CreateCastle(Transform parent, Materials materials)
    {
        CreateBox("BackKeep", parent, new Vector3(0f, 3f, -13f), new Vector3(12f, 6f, 3f), materials.stone);
        CreateBox("InnerHallDoor", parent, new Vector3(0f, 1.45f, -11.35f), new Vector3(2.4f, 2.9f, 0.32f), materials.darkMetal);

        CreateTower("LeftTower", parent, new Vector3(-8.5f, 3.2f, -11.2f), materials);
        CreateTower("RightTower", parent, new Vector3(8.5f, 3.2f, -11.2f), materials);
        CreateTower("FrontLeftTower", parent, new Vector3(-8.5f, 3.2f, 0.8f), materials);
        CreateTower("FrontRightTower", parent, new Vector3(8.5f, 3.2f, 0.8f), materials);

        CreateBox("WestWall", parent, new Vector3(-8.5f, 2.1f, -5.2f), new Vector3(1.3f, 4.2f, 12f), materials.stone);
        CreateBox("EastWall", parent, new Vector3(8.5f, 2.1f, -5.2f), new Vector3(1.3f, 4.2f, 12f), materials.stone);
        CreateBox("FrontWallLeft", parent, new Vector3(-4.7f, 2.1f, 0.8f), new Vector3(6.3f, 4.2f, 1.25f), materials.stone);
        CreateBox("FrontWallRight", parent, new Vector3(4.7f, 2.1f, 0.8f), new Vector3(6.3f, 4.2f, 1.25f), materials.stone);

        CreateBox("WallWalk_Back", parent, new Vector3(0f, 4.45f, -9.8f), new Vector3(18f, 0.35f, 2f), materials.walkway);
        CreateBox("WallWalk_Left", parent, new Vector3(-8.5f, 4.45f, -5.2f), new Vector3(2f, 0.35f, 12f), materials.walkway);
        CreateBox("WallWalk_Right", parent, new Vector3(8.5f, 4.45f, -5.2f), new Vector3(2f, 0.35f, 12f), materials.walkway);
        CreateBox("MachineGunNest", parent, new Vector3(0f, 4.75f, 0.2f), new Vector3(3.2f, 0.5f, 1.4f), materials.darkMetal);

        CreateBox("RampToWall", parent, new Vector3(-5.7f, 1.9f, -2.2f), new Vector3(2.2f, 0.35f, 7f), materials.walkway, Quaternion.Euler(-24f, 0f, 0f));
        CreateBox("RampToWall_Right", parent, new Vector3(5.7f, 1.9f, -2.2f), new Vector3(2.2f, 0.35f, 7f), materials.walkway, Quaternion.Euler(-24f, 0f, 0f));

        CreateBox("RoyalBanner_Left", parent, new Vector3(-2.2f, 4.2f, -11.25f), new Vector3(0.55f, 2f, 0.12f), materials.royalBlue);
        CreateBox("RoyalBanner_Right", parent, new Vector3(2.2f, 4.2f, -11.25f), new Vector3(0.55f, 2f, 0.12f), materials.royalBlue);
    }

    private static DefenseObjective CreateCoreObjective(Transform parent, Materials materials)
    {
        var core = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        core.name = "RoyalCore_ProtectedTarget";
        core.transform.SetParent(parent);
        core.transform.position = new Vector3(0f, 1.25f, -8.4f);
        core.transform.localScale = new Vector3(0.9f, 1.15f, 0.9f);
        core.GetComponent<Renderer>().sharedMaterial = materials.royalBlue;
        core.AddComponent<KingAllyCombat>();
        return core.AddComponent<DefenseObjective>();
    }

    private static CastleGateObjective CreateGateObjective(Transform parent, Materials materials)
    {
        var gate = CreateBox("MainCastleGate_Objective", parent, new Vector3(0f, 1.75f, 0.95f), new Vector3(3.2f, 3.5f, 0.45f), materials.darkMetal);
        gate.AddComponent<CastleGateObjective>();
        CreateBox("PortcullisBars", gate.transform, Vector3.zero, new Vector3(3.45f, 3.7f, 0.12f), materials.brass);
        return gate.GetComponent<CastleGateObjective>();
    }

    private static void CreateField(Transform parent, Materials materials)
    {
        CreateBox("LeftVillageBlock", parent, new Vector3(-14f, 1.2f, 8.5f), new Vector3(4.4f, 2.4f, 4f), materials.wood);
        CreateBox("RightSupplyShed", parent, new Vector3(14f, 1f, 7f), new Vector3(3.6f, 2f, 3.6f), materials.wood);
        CreateBox("LeftRockCover", parent, new Vector3(-5.7f, 0.6f, 8.4f), new Vector3(2.5f, 1.2f, 1.6f), materials.rock);
        CreateBox("RightRockCover", parent, new Vector3(5.7f, 0.6f, 8.4f), new Vector3(2.5f, 1.2f, 1.6f), materials.rock);
        CreateBox("GateBarricade_Left", parent, new Vector3(-2.9f, 0.65f, 3.2f), new Vector3(2.4f, 1.3f, 0.55f), materials.wood);
        CreateBox("GateBarricade_Right", parent, new Vector3(2.9f, 0.65f, 3.2f), new Vector3(2.4f, 1.3f, 0.55f), materials.wood);
    }

    private static Transform[] CreateSpawnPoints(Transform parent)
    {
        var positions = new[]
        {
            new Vector3(0f, 1.1f, 22f),
            new Vector3(-16f, 1.1f, 17f),
            new Vector3(16f, 1.1f, 17f)
        };

        var points = new Transform[positions.Length];
        for (var i = 0; i < positions.Length; i++)
        {
            var point = new GameObject($"ZombieRoute_{i + 1:00}");
            point.transform.SetParent(parent);
            point.transform.position = positions[i];
            points[i] = point.transform;
            CreateBox($"ZombieRouteMarker_{i + 1:00}", parent, positions[i] + Vector3.up * 0.08f, new Vector3(1.2f, 0.16f, 1.2f), CreateMaterials().red);
        }

        return points;
    }

    private static Transform[] CreateGrapplePoints(Transform parent, Materials materials)
    {
        var positions = new[]
        {
            new Vector3(-8.5f, 5.1f, 0.8f),
            new Vector3(8.5f, 5.1f, 0.8f),
            new Vector3(0f, 5.2f, 0.2f),
            new Vector3(-5.8f, 5.1f, -9.8f),
            new Vector3(5.8f, 5.1f, -9.8f)
        };

        var points = new Transform[positions.Length];
        for (var i = 0; i < positions.Length; i++)
        {
            var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            marker.name = $"GrapplePoint_{i + 1:00}";
            marker.transform.SetParent(parent);
            marker.transform.position = positions[i];
            marker.transform.localScale = Vector3.one * 0.42f;
            marker.GetComponent<Renderer>().sharedMaterial = materials.red;
            Object.DestroyImmediate(marker.GetComponent<Collider>());
            points[i] = marker.transform;
        }

        return points;
    }

    private static void SetupViewWeapon(Transform cameraTransform, Materials materials)
    {
        if (cameraTransform == null || cameraTransform.Find("SteamRevolver") != null)
        {
            return;
        }

        var revolver = CreateBox("SteamRevolver", cameraTransform, new Vector3(0.34f, -0.3f, 0.62f), new Vector3(0.18f, 0.16f, 0.52f), materials.darkMetal);
        revolver.transform.localRotation = Quaternion.Euler(0f, -7f, 0f);
        Object.DestroyImmediate(revolver.GetComponent<Collider>());

        var arm = CreateBox("MechanicalLeftArm", cameraTransform, new Vector3(-0.34f, -0.34f, 0.58f), new Vector3(0.22f, 0.2f, 0.68f), materials.brass);
        arm.transform.localRotation = Quaternion.Euler(0f, 9f, 0f);
        Object.DestroyImmediate(arm.GetComponent<Collider>());
    }

    private static void CreateTower(string name, Transform parent, Vector3 position, Materials materials)
    {
        var tower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tower.name = name;
        tower.transform.SetParent(parent);
        tower.transform.position = position;
        tower.transform.localScale = new Vector3(1.8f, 3.2f, 1.8f);
        tower.GetComponent<Renderer>().sharedMaterial = materials.stone;

        var roof = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        roof.name = $"{name}_Roof";
        roof.transform.SetParent(parent);
        roof.transform.position = position + Vector3.up * 3.45f;
        roof.transform.localScale = new Vector3(2.1f, 0.5f, 2.1f);
        roof.GetComponent<Renderer>().sharedMaterial = materials.roof;
    }

    private static GameObject CreateBox(string name, Transform parent, Vector3 position, Vector3 scale, Material material)
    {
        return CreateBox(name, parent, position, scale, material, Quaternion.identity);
    }

    private static GameObject CreateBox(string name, Transform parent, Vector3 position, Vector3 scale, Material material, Quaternion rotation)
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent);
        box.transform.localPosition = position;
        box.transform.localRotation = rotation;
        box.transform.localScale = scale;
        box.GetComponent<Renderer>().sharedMaterial = material;
        return box;
    }

    private static void UpdateBuildSettings()
    {
        var scenes = EditorBuildSettings.scenes;
        foreach (var scene in scenes)
        {
            if (scene.path == ScenePath)
            {
                return;
            }
        }

        var updated = new EditorBuildSettingsScene[scenes.Length + 1];
        scenes.CopyTo(updated, 0);
        updated[^1] = new EditorBuildSettingsScene(ScenePath, true);
        EditorBuildSettings.scenes = updated;
    }

    private static Materials CreateMaterials()
    {
        return new Materials
        {
            stone = CreateMaterial("Castle_Stone", new Color(0.32f, 0.33f, 0.32f)),
            darkMetal = CreateMaterial("Castle_DarkMetal", new Color(0.08f, 0.08f, 0.08f)),
            brass = CreateMaterial("Castle_Brass", new Color(0.62f, 0.44f, 0.16f)),
            royalBlue = CreateMaterial("Castle_RoyalBlue", new Color(0.1f, 0.12f, 0.34f)),
            grass = CreateMaterial("Castle_FieldGrass", new Color(0.24f, 0.33f, 0.2f)),
            dirt = CreateMaterial("Castle_Dirt", new Color(0.38f, 0.31f, 0.22f)),
            road = CreateMaterial("Castle_Road", new Color(0.28f, 0.25f, 0.21f)),
            wood = CreateMaterial("Castle_Wood", new Color(0.36f, 0.24f, 0.14f)),
            rock = CreateMaterial("Castle_Rock", new Color(0.45f, 0.44f, 0.42f)),
            roof = CreateMaterial("Castle_Roof", new Color(0.19f, 0.13f, 0.09f)),
            walkway = CreateMaterial("Castle_Walkway", new Color(0.25f, 0.26f, 0.25f)),
            zombie = CreateMaterial("Castle_CitizenZombie", new Color(0.24f, 0.38f, 0.25f)),
            red = CreateMaterial("Castle_RedMarker", new Color(0.75f, 0.06f, 0.03f))
        };
    }

    private static Material CreateMaterial(string name, Color color)
    {
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.name = name;
        material.color = color;
        return material;
    }

    private struct Materials
    {
        public Material stone;
        public Material darkMetal;
        public Material brass;
        public Material royalBlue;
        public Material grass;
        public Material dirt;
        public Material road;
        public Material wood;
        public Material rock;
        public Material roof;
        public Material walkway;
        public Material zombie;
        public Material red;
    }
}
