using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class AssetBundleHelper
{
    // Coroutine version (for VideoLoader etc.)
    public static IEnumerator LoadBundleCoroutine(string path, System.Action<AssetBundle> onComplete)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("AssetBundle load failed: " + www.error);
            onComplete?.Invoke(null);
        }
        else
        {
            onComplete?.Invoke(DownloadHandlerAssetBundle.GetContent(www));
        }
#else
        var bundleLoad = AssetBundle.LoadFromFileAsync(path);
        yield return bundleLoad;
        onComplete?.Invoke(bundleLoad.assetBundle);
#endif
    }

    // Async/await version (for GLBLoader etc.)
    public static async Task<AssetBundle> LoadBundleAsync(string path)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path);
        await www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("AssetBundle load failed: " + www.error);
            return null;
        }
        return DownloadHandlerAssetBundle.GetContent(www);
#else
        var abRequest = AssetBundle.LoadFromFileAsync(path);
        await abRequest;
        return abRequest.assetBundle;
#endif
    }
}