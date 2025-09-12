using UnityEngine;

public class HomeManager : MonoBehaviour
{
	public AppConfig appConfig;
	public VideoBundleLoader project1Loader;
	
	public void OnProject1Clicked()
	{
		GameManager.instance.ChangeGameState(GameState.Video);
		
		var bundlePath = appConfig.GetActiveVideoBundlePath();
		var clipName = appConfig.GetActiveVideoClipName();
		project1Loader.StartLoad(bundlePath, clipName);
		
	}

	public void OnProject2Clicked()
	{
		GameManager.instance.ChangeGameState(GameState.Glb);
		
		var bundlePath = appConfig.GetActiveBundleGlbPath();
		var glbName = appConfig.GetActiveGlbName();
		project1Loader.StartLoad(bundlePath, glbName);
	}
}