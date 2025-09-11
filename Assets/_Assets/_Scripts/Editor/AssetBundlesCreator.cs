using UnityEditor;
using UnityEngine;
using System.IO;

public class AssetBundlesCreator
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "bundles");
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

        BuildPipeline.BuildAssetBundles(
            path,
            BuildAssetBundleOptions.None,
            target
        );

        Debug.Log("AssetBundles built to: " + path);
    }

    [MenuItem("Assets/Delete Built AssetBundles")]
    static void DeleteBuiltAssetBundles()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "bundles");
        
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path); // Recreate empty directory
            AssetDatabase.Refresh();
            Debug.Log("Built AssetBundles deleted from: " + path);
        }
        else
        {
            Debug.Log("No AssetBundles directory found.");
        }
    }
}

