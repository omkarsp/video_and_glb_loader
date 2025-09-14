using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VideoBundleLoader : MonoBehaviour
{
    string videoClipNameInBundle = "VideoClip";
    VideoPlayer videoPlayer;
    AssetBundle loadedBundle;
    VideoClip currentVideoClip;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (!videoPlayer)
            Debug.LogError("VideoBundleLoader requires a VideoPlayer component on the same GameObject");
    }

    public void StartLoad(string bundleAbsolutePath, string videoClipNameInBundle)
    {
        this.videoClipNameInBundle = videoClipNameInBundle;
        StartCoroutine(LoadBundleCoroutine(bundleAbsolutePath));
    }

    // Overload for when videoClipName is already set
    public void StartLoad(string bundleAbsolutePath)
    {
        StartCoroutine(LoadBundleCoroutine(bundleAbsolutePath));
    }

    private IEnumerator LoadBundleCoroutine(string bundlePath)
    {
        // Unload previous bundle if exists
        if (loadedBundle != null)
        {
            loadedBundle.Unload(true);
            loadedBundle = null;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundlePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load AssetBundle: " + www.error);
            yield break;
        }

        loadedBundle = DownloadHandlerAssetBundle.GetContent(www);
#else
        var bundleLoad = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleLoad;
        loadedBundle = bundleLoad.assetBundle;
#endif

        if (loadedBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle at " + bundlePath);
            yield break;
        }

        // Try to find the clip inside the bundle
        yield return StartCoroutine(LoadVideoFromBundle(loadedBundle));
    }

    private IEnumerator LoadVideoFromBundle(AssetBundle bundle)
    {
        var names = bundle.GetAllAssetNames();
        string match = null;
        string needle = videoClipNameInBundle.ToLowerInvariant();

        foreach (var n in names)
        {
            if (n.ToLowerInvariant().Contains(needle))
            {
                match = n;
                break;
            }
        }

        if (match == null)
        {
            Debug.LogError("VideoClip not found. Available assets:\n" + string.Join("\n", names));
            yield break;
        }

        var clipLoad = bundle.LoadAssetAsync<VideoClip>(match);
        yield return clipLoad;

        VideoClip videoClip = clipLoad.asset as VideoClip;
        if (videoClip == null)
        {
            Debug.LogError("Found asset but it’s not a VideoClip: " + match);
            bundle.Unload(false);
            yield break;
        }

        videoPlayer.clip = videoClip;
        currentVideoClip = videoClip;

        Debug.Log("VideoClip loaded successfully: " + videoClip.name);
    }

    // Call this when you want to unload the bundle and clear the video clip
    public void UnloadAndCleanup()
    {
        if (videoPlayer != null)
        {
            videoPlayer.clip = null;
        }

        if (loadedBundle != null)
        {
            loadedBundle.Unload(true); // unload all loaded assets
            loadedBundle = null;
        }

        currentVideoClip = null;
        Debug.Log("Bundle unloaded and video cleared");
    }

    // Optional: Get reference to current video clip
    public VideoClip GetCurrentVideoClip() => currentVideoClip;

    // Optional: Check if video is loaded and ready
    public bool IsVideoLoaded() => videoPlayer != null && videoPlayer.clip != null;
}
