using UnityEngine;

public class HomeManager : MonoBehaviour
{
	public AppConfig appConfig;
	public VideoBundleLoader project1Loader;
	
	public void OnProject1Clicked()
	{
		var bundlePath = appConfig.GetActiveBundlePath();
		var clipName = appConfig.GetActiveVideoClipName();
		project1Loader.StartLoad(bundlePath, clipName);
	}

	public void OnProject2Clicked()
	{
		
	}
}