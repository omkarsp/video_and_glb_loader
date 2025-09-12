using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "AppConfig", menuName = "App/AppConfig")]
public class AppConfig : ScriptableObject
{
	[Header("Folder Paths")]
	public string assetBundleFolder = "bundles";

	[Header("Active Video")]
	public string videoBundleName = "videos";
	public string videoClipName = "VideoClip";
	
	[Header("GLBs")]
	public string glbBundleName = "glbs";
	public string glbName = "GLB";

	public string GetVideoBundlePath(string bundle) => Path.Combine(Application.streamingAssetsPath, assetBundleFolder, bundle);
	public string GetGlbBundlePath(string bundle) => Path.Combine(Application.streamingAssetsPath, assetBundleFolder, bundle);

	// Convenience
	public string GetActiveVideoBundlePath() => GetVideoBundlePath(videoBundleName);
	public string GetActiveVideoClipName() => videoClipName;
	public string GetActiveGlbBundlePath() => GetGlbBundlePath(glbBundleName);
	public string GetActiveGlbName() => glbName;
	
}