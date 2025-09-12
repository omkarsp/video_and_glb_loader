using UnityEngine;

public class HomeManager : MonoBehaviour
{
	public AppConfig appConfig;
	public VideoBundleLoader project1Loader;
	public GLBLoader project2Loader;
	
	public void OnProject1Clicked()
	{
		GameManager.instance.ChangeGameState(GameState.Video);
		
		var bundlePath = appConfig.GetActiveVideoBundlePath();
		var clipName = appConfig.GetActiveVideoClipName();
		project1Loader.StartLoad(bundlePath, clipName);
		
	}

	public async void OnProject2Clicked()
	{
		GameManager.instance.ChangeGameState(GameState.Glb);
		
		var bundlePath = appConfig.GetActiveGlbBundlePath();
		var glbName = appConfig.GetActiveGlbName();

		// project2Loader.StartLoad(bundlePath, glbName);

		var root = await project2Loader.LoadGlbFromBundle(bundlePath, glbName);
		if (root != null)
		{
			root.transform.localPosition = Vector3.zero;
		}
	}
}