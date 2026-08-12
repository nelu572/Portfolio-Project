using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CreatorReloadTestBuilder
{
    private const string FolderPath = "Assets/Resource/Model/Weapon/CreatorReloadTest";
    private const string FbxPath = FolderPath + "/Creator_Reload.fbx";
    private const string ControllerPath = FolderPath + "/Creator_Reload.controller";
    private const string ScenePath = "Assets/Scenes/Test/CreatorReload_TestScene.unity";
    private const string MarkerPath = "Library/CreatorReloadTestBuilder.done";

    [InitializeOnLoadMethod]
    private static void RunAfterReload()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                return;
            }

            if (!File.Exists(FbxPath))
            {
                return;
            }

            var fbxWriteTime = File.GetLastWriteTimeUtc(FbxPath).Ticks.ToString();
            if (File.Exists(MarkerPath) && File.ReadAllText(MarkerPath) == fbxWriteTime && File.Exists(ScenePath))
            {
                return;
            }

            Build();
            Directory.CreateDirectory(Path.GetDirectoryName(MarkerPath) ?? "Library");
            File.WriteAllText(MarkerPath, fbxWriteTime);
        };
    }

    [MenuItem("Tools/Test/Build Creator Reload Test")]
    public static void Build()
    {
        Directory.CreateDirectory(FolderPath);
        Directory.CreateDirectory("Assets/Scenes/Test");

        ConfigureModelImporter();
        AssetDatabase.ImportAsset(FbxPath, ImportAssetOptions.ForceUpdate);

        var clips = LoadClips();
        var controller = BuildController(ControllerPath, clips);

        BuildScene(controller);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Creator reload test scene built.");
    }

    private static void ConfigureModelImporter()
    {
        var importer = AssetImporter.GetAtPath(FbxPath) as ModelImporter;
        if (importer == null)
        {
            throw new FileNotFoundException($"FBX importer not found: {FbxPath}");
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

    private static AnimationClip[] LoadClips()
    {
        var clips = AssetDatabase.LoadAllAssetsAtPath(FbxPath)
            .OfType<AnimationClip>()
            .Where(clip => !clip.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (clips.Length == 0)
        {
            throw new InvalidOperationException($"No animation clips found in {FbxPath}");
        }

        Debug.Log($"Creator reload clips: {string.Join(", ", clips.Select(clip => clip.name))}");
        return clips;
    }

    private static AnimatorController BuildController(string path, AnimationClip[] clips)
    {
        AssetDatabase.DeleteAsset(path);
        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
        controller.layers = Array.Empty<AnimatorControllerLayer>();

        var stateMachine = new AnimatorStateMachine
        {
            name = "Base Layer",
            hideFlags = HideFlags.HideInHierarchy
        };
        AssetDatabase.AddObjectToAsset(stateMachine, controller);

        AddState(stateMachine, "Idle", FindClip(clips, "idle_pose", "idle"), 0);
        AddState(stateMachine, "Fire", FindClip(clips, "fire"), 1);
        AddState(stateMachine, "Reload", FindClip(clips, "reload"), 2);
        AddOptionalState(stateMachine, clips, "Walk", "walk", 3);
        AddOptionalState(stateMachine, clips, "RefPose", "ref_pose", 4);

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

    private static void BuildScene(RuntimeAnimatorController controller)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "CreatorReload_TestScene";

        var model = InstantiateModel(controller);
        CreateLighting();
        CreateCamera(model.transform);

        EditorSceneManager.SaveScene(scene, ScenePath);
    }

    private static GameObject InstantiateModel(RuntimeAnimatorController controller)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FbxPath);
        if (prefab == null)
        {
            throw new FileNotFoundException($"FBX prefab not found: {FbxPath}");
        }

        var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (instance == null)
        {
            throw new InvalidOperationException($"Failed to instantiate {FbxPath}");
        }

        instance.name = "Creator_Reload";
        instance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        RemoveNestedAnimators(instance);
        var animator = ConfigureAnimator(instance, controller);

        var player = instance.AddComponent<CreatorReloadKeyboardAnimationPlayer>();
        player.Configure(animator);

        return instance;
    }

    private static Animator ConfigureAnimator(GameObject target, RuntimeAnimatorController controller)
    {
        var animator = target.GetComponent<Animator>();
        if (animator == null)
        {
            animator = target.AddComponent<Animator>();
        }

        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = false;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        return animator;
    }

    private static void RemoveNestedAnimators(GameObject root)
    {
        foreach (var nestedAnimator in root.GetComponentsInChildren<Animator>(true))
        {
            if (nestedAnimator.gameObject != root)
            {
                UnityEngine.Object.DestroyImmediate(nestedAnimator);
            }
        }
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
