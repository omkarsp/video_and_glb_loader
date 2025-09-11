using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "AppConfig", menuName = "App/AppConfig")]
public class AppConfig : ScriptableObject
{
	[Header("Folder Paths")]
	public string assetBundleFolder = "bundles";
	public string videoFolder = "videos";
	public string glbFolder = "glbs";

	[Header("Active Video")]
	public string bundleName = "videoprefabsbundle";
	public string videoClipName = "VideoClip";

	public string GetAssetBundlePath(string bundle) => Path.Combine(Application.streamingAssetsPath, assetBundleFolder, bundle);
	public string GetVideoPath(string videoName) => Path.Combine(Application.streamingAssetsPath, videoFolder, videoName);
	public string GetGlbPath(string glbName) => Path.Combine(Application.streamingAssetsPath, glbFolder, glbName);

	// Convenience
	public string GetActiveBundlePath() => GetAssetBundlePath(bundleName);
	public string GetActiveVideoClipName() => videoClipName;
}