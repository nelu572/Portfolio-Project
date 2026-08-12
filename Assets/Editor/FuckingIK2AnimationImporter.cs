using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public static class FuckingIK2AnimationImporter
{
    private const string FbxPath = "Assets/Resource/Model/Weapon/fuckingIK2.fbx";
    private const string ControllerPath = "Assets/Resource/Model/Weapon/tlqkf-z.controller";
    private const string DoneMarkerPath = "Library/FuckingIK2AnimationImporter.done";

    [InitializeOnLoadMethod]
    private static void RunAfterReload()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                return;
            }

            var fbxFullPath = Path.GetFullPath(FbxPath);
            if (!File.Exists(fbxFullPath))
            {
                return;
            }

            var markerFullPath = Path.GetFullPath(DoneMarkerPath);
            var fbxWriteTime = File.GetLastWriteTimeUtc(fbxFullPath).Ticks.ToString();
            if (File.Exists(markerFullPath) && File.ReadAllText(markerFullPath) == fbxWriteTime)
            {
                return;
            }

            ApplyImportSettings();
            AssetDatabase.ImportAsset(FbxPath, ImportAssetOptions.ForceUpdate);
            RebuildController();

            Directory.CreateDirectory(Path.GetDirectoryName(markerFullPath) ?? "Library");
            File.WriteAllText(markerFullPath, fbxWriteTime);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("fuckingIK2 FBX import settings and animator controller rebuilt.");
        };
    }

    [MenuItem("Tools/Test/Rebuild FuckingIK2 Animations")]
    public static void RebuildNow()
    {
        ApplyImportSettings();
        AssetDatabase.ImportAsset(FbxPath, ImportAssetOptions.ForceUpdate);
        RebuildController();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void ApplyImportSettings()
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

    private static void RebuildController()
    {
        var clips = AssetDatabase.LoadAllAssetsAtPath(FbxPath)
            .OfType<AnimationClip>()
            .Where(clip => !clip.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (clips.Length == 0)
        {
            throw new InvalidOperationException($"No animation clips found in {FbxPath}");
        }

        var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        if (controller == null)
        {
            controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        }

        ClearController(controller);
        AddLayer(controller, "Arms", clips, true);
        AddLayer(controller, "Gun", clips, false);

        EditorUtility.SetDirty(controller);
        Debug.Log($"fuckingIK2 clips: {string.Join(", ", clips.Select(clip => clip.name))}");
    }

    private static void ClearController(AnimatorController controller)
    {
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(ControllerPath))
        {
            if (asset != controller && asset != null)
            {
                UnityEngine.Object.DestroyImmediate(asset, true);
            }
        }

        controller.parameters = Array.Empty<AnimatorControllerParameter>();
        controller.layers = Array.Empty<AnimatorControllerLayer>();
    }

    private static void AddLayer(AnimatorController controller, string layerName, AnimationClip[] clips, bool arms)
    {
        var stateMachine = new AnimatorStateMachine
        {
            name = layerName,
            hideFlags = HideFlags.HideInHierarchy
        };

        AssetDatabase.AddObjectToAsset(stateMachine, controller);

        AddState(stateMachine, "Idle", FindClip(clips, arms, "idle_pose", "idle"), 0);
        AddState(stateMachine, "Fire", FindClip(clips, arms, "fire"), 1);
        AddState(stateMachine, "Reload", FindClip(clips, arms, "reload"), 2);
        AddState(stateMachine, "Tactical_Reload", FindClip(clips, arms, "tactical_reload"), 3);
        AddState(stateMachine, "Inspect", FindClip(clips, arms, "inspect"), 4);

        controller.AddLayer(new AnimatorControllerLayer
        {
            name = layerName,
            stateMachine = stateMachine,
            defaultWeight = 1f,
            blendingMode = AnimatorLayerBlendingMode.Override,
            iKPass = false,
            syncedLayerIndex = -1
        });
    }

    private static void AddState(AnimatorStateMachine stateMachine, string stateName, AnimationClip clip, int index)
    {
        var state = stateMachine.AddState(stateName, new Vector3(280f, 80f + index * 70f, 0f));
        state.motion = clip;
        state.writeDefaultValues = true;

        if (index == 0)
        {
            stateMachine.defaultState = state;
        }
    }

    private static AnimationClip FindClip(AnimationClip[] clips, bool arms, params string[] nameParts)
    {
        var rigPrefix = arms ? "armsrig" : "gunrig";
        var scoped = clips.Where(clip => Normalize(clip.name).Contains(rigPrefix)).ToArray();
        if (scoped.Length == 0)
        {
            scoped = clips;
        }

        foreach (var namePart in nameParts)
        {
            var normalizedPart = Normalize(namePart);
            var match = scoped.FirstOrDefault(clip => Normalize(clip.name).Contains(normalizedPart));
            if (match != null)
            {
                return match;
            }
        }

        throw new InvalidOperationException($"Clip not found for {(arms ? "Arms" : "Gun")}: {string.Join(", ", nameParts)}");
    }

    private static string Normalize(string value)
    {
        return value
            .Replace("_", string.Empty)
            .Replace("-", string.Empty)
            .Replace(" ", string.Empty)
            .Replace("|", string.Empty)
            .ToLowerInvariant();
    }
}
