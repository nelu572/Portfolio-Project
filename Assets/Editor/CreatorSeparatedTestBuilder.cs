using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CreatorSeparatedTestBuilder
{
    private const string FolderPath = "Assets/Resource/Model/Weapon/CreatorSeparated";
    private const string ArmsFbxPath = FolderPath + "/Creator_Arms.fbx";
    private const string GunFbxPath = FolderPath + "/Creator_Gun.fbx";
    private const string ArmsControllerPath = FolderPath + "/Creator_Arms.controller";
    private const string GunControllerPath = FolderPath + "/Creator_Gun.controller";
    private const string ScenePath = "Assets/Scenes/Test/CreatorSeparated_TestScene.unity";
    private const string MarkerPath = "Library/CreatorSeparatedTestBuilder.done";

    [InitializeOnLoadMethod]
    private static void RunAfterReload()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                return;
            }

            if (!File.Exists(ArmsFbxPath) || !File.Exists(GunFbxPath))
            {
                return;
            }

            var markerValue = File.GetLastWriteTimeUtc(ArmsFbxPath).Ticks + ":" + File.GetLastWriteTimeUtc(GunFbxPath).Ticks;
            if (File.Exists(MarkerPath) && File.ReadAllText(MarkerPath) == markerValue && File.Exists(ScenePath))
            {
                return;
            }

            Build();
            Directory.CreateDirectory(Path.GetDirectoryName(MarkerPath) ?? "Library");
            File.WriteAllText(MarkerPath, markerValue);
        };
    }

    [MenuItem("Tools/Test/Build Creator Separated Test")]
    public static void Build()
    {
        Directory.CreateDirectory(FolderPath);
        Directory.CreateDirectory("Assets/Scenes/Test");

        ConfigureModelImporter(ArmsFbxPath);
        ConfigureModelImporter(GunFbxPath);
        AssetDatabase.ImportAsset(ArmsFbxPath, ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(GunFbxPath, ImportAssetOptions.ForceUpdate);

        var armsController = BuildController(ArmsControllerPath, ArmsFbxPath);
        var gunController = BuildController(GunControllerPath, GunFbxPath);
        BuildScene(armsController, gunController);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Creator separated test scene built.");
    }

    private static void ConfigureModelImporter(string path)
    {
        var importer = AssetImporter.GetAtPath(path) as ModelImporter;
        if (importer == null)
        {
            throw new FileNotFoundException($"FBX importer not found: {path}");
        }

        importer.importAnimation = true;
        importer.animationType = ModelImporterAnimationType.Generic;
        importer.avatarSetup = ModelImporterAvatarSetup.NoAvatar;
        importer.resampleCurves = false;
        importer.animationCompression = ModelImporterAnimationCompression.Off;
        importer.optimizeGameObjects = false;
        importer.importAnimatedCustomProperties = true;
        importer.SaveAndReimport();
    }

    private static AnimatorController BuildController(string controllerPath, string fbxPath)
    {
        var clips = AssetDatabase.LoadAllAssetsAtPath(fbxPath)
            .OfType<AnimationClip>()
            .Where(clip => !clip.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (clips.Length == 0)
        {
            throw new InvalidOperationException($"No animation clips found in {fbxPath}");
        }

        AssetDatabase.DeleteAsset(controllerPath);
        var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        controller.layers = Array.Empty<AnimatorControllerLayer>();

        var stateMachine = new AnimatorStateMachine
        {
            name = "Base Layer",
            hideFlags = HideFlags.HideInHierarchy
        };
        AssetDatabase.AddObjectToAsset(stateMachine, controller);

        AddState(stateMachine, "idle_pose", FindClip(clips, "idle_pose", "idle"), 0);
        AddState(stateMachine, "fire", FindClip(clips, "fire"), 1);
        AddState(stateMachine, "reload", FindClip(clips, "reload"), 2);
        AddOptionalState(stateMachine, clips, "walk", "walk", 3);
        AddOptionalState(stateMachine, clips, "ref_pose", "ref_pose", 4);

        controller.AddLayer(new AnimatorControllerLayer
        {
            name = "Base Layer",
            stateMachine = stateMachine,
            defaultWeight = 1f,
            blendingMode = AnimatorLayerBlendingMode.Override,
            syncedLayerIndex = -1,
            iKPass = false
        });

        EditorUtility.SetDirty(controller);
        Debug.Log($"{Path.GetFileName(fbxPath)} clips: {string.Join(", ", clips.Select(clip => clip.name))}");
        return controller;
    }

    private static void AddState(AnimatorStateMachine stateMachine, string stateName, AnimationClip clip, int index)
    {
        var state = stateMachine.AddState(stateName, new Vector3(280f, 80f + index * 70f, 0f));
        state.motion = clip;
        state.writeDefaultValues = false;

        if (index == 0)
        {
            stateMachine.defaultState = state;
        }
    }

    private static void AddOptionalState(AnimatorStateMachine stateMachine, AnimationClip[] clips, string stateName, string clipName, int index)
    {
        var clip = TryFindClip(clips, clipName);
        if (clip != null)
        {
            AddState(stateMachine, stateName, clip, index);
        }
    }

    private static AnimationClip FindClip(AnimationClip[] clips, params string[] names)
    {
        var clip = names.Select(name => TryFindClip(clips, name)).FirstOrDefault(match => match != null);
        if (clip == null)
        {
            throw new InvalidOperationException($"Clip not found: {string.Join("/", names)}");
        }

        return clip;
    }

    private static AnimationClip TryFindClip(AnimationClip[] clips, string name)
    {
        var normalizedName = Normalize(name);
        return clips.FirstOrDefault(clip => Normalize(clip.name).Contains(normalizedName));
    }

    private static string Normalize(string value)
    {
        return value.Replace("_", string.Empty)
            .Replace("-", string.Empty)
            .Replace(" ", string.Empty)
            .Replace("|", string.Empty)
            .ToLowerInvariant();
    }

    private static void BuildScene(RuntimeAnimatorController armsController, RuntimeAnimatorController gunController)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "CreatorSeparated_TestScene";

        var root = new GameObject("CreatorSeparatedPreviewRoot");
        var arms = InstantiateModel(ArmsFbxPath, "Creator_Arms", armsController, root.transform);
        var gun = InstantiateModel(GunFbxPath, "Creator_Gun", gunController, root.transform);

        var player = root.AddComponent<CreatorSeparatedKeyboardAnimationPlayer>();
        player.Configure(arms.GetComponent<Animator>(), gun.GetComponent<Animator>());

        CreateLighting();
        CreateCamera(root.transform);

        EditorSceneManager.SaveScene(scene, ScenePath);
    }

    private static GameObject InstantiateModel(string fbxPath, string name, RuntimeAnimatorController controller, Transform parent)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
        if (prefab == null)
        {
            throw new FileNotFoundException($"FBX prefab not found: {fbxPath}");
        }

        var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (instance == null)
        {
            throw new InvalidOperationException($"Failed to instantiate {fbxPath}");
        }

        instance.name = name;
        instance.transform.SetParent(parent, false);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one;

        var animator = instance.GetComponent<Animator>();
        if (animator == null)
        {
            animator = instance.AddComponent<Animator>();
        }

        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = false;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        return instance;
    }

    private static void CreateCamera(Transform target)
    {
        var cameraObject = new GameObject("Camera");
        var camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.fieldOfView = 60f;
        camera.nearClipPlane = 0.01f;
        cameraObject.AddComponent<AudioListener>();

        var bounds = CalculateBounds(target);
        cameraObject.transform.position = bounds.center + new Vector3(0f, bounds.size.y * 0.15f, -Mathf.Max(1.25f, bounds.size.magnitude * 1.2f));
        cameraObject.transform.LookAt(bounds.center);
    }

    private static Bounds CalculateBounds(Transform target)
    {
        var renderers = target.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(target.position, Vector3.one);
        }

        var bounds = renderers[0].bounds;
        for (var i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }

    private static void CreateLighting()
    {
        var lightObject = new GameObject("Directional Light");
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
    }
}
