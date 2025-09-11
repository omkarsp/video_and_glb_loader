using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "PathConfig", menuName = "PathConfig")]
public class PathConfig : ScriptableObject
{
    public string assetBundleFolder = "bundles";
    public string videoFolder = "videos";
    public string glbFolder = "glbs";

    public string GetAssetBundlePath(string bundleName) => Path.Combine(Application.streamingAssetsPath, assetBundleFolder, bundleName);

    public string GetVideoPath(string videoName) => Path.Combine(Application.streamingAssetsPath, videoFolder, videoName);

    public string GetGlbPath(string glbName) => Path.Combine(Application.streamingAssetsPath, glbFolder, glbName);
}
