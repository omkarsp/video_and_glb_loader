using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class AssetBundlesCreator
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string outputPath = Path.Combine(Application.streamingAssetsPath, "bundles");
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

        // 1) Build videos explicitly
        List<string> videoAssets = CollectAndConvertVideos();
        if (videoAssets.Count > 0)
        {
            AssetBundleBuild build = new AssetBundleBuild
            {
                assetBundleName = "videos",
                assetNames = videoAssets.ToArray()
            };

            BuildPipeline.BuildAssetBundles(
                outputPath,
                new[] { build },
                BuildAssetBundleOptions.None,
                target
            );

        }

        // 2) Build all Inspector-assigned bundles (glbs, prefabs, etc.)
        BuildPipeline.BuildAssetBundles(
            outputPath,
            BuildAssetBundleOptions.None,
            target
        );

        Debug.Log("All AssetBundles built to: " + outputPath);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Finds all .mp4 files, duplicates to .bytes, and returns their paths.
    /// </summary>
    private static List<string> CollectAndConvertVideos()
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { "Assets" });
        List<string> collected = new List<string>();

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!assetPath.EndsWith(".mp4")) continue;

            string newPath = assetPath + ".bytes";
            File.Copy(assetPath, newPath, true);
            AssetDatabase.ImportAsset(newPath);

            collected.Add(newPath);
        }

        return collected;
    }

    [MenuItem("Assets/Delete Built AssetBundles")]
    static void DeleteBuiltAssetBundles()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "bundles");

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
            Debug.Log("Deleted all built AssetBundles at: " + path);
        }
        else
        {
            Debug.Log("No AssetBundles directory found.");
        }
    }
}
