using System.Collections;
using UnityEngine;
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
        if (!videoPlayer) Debug.LogError("VideoBundleLoader requires a VideoPlayer component on the same GameObject");
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

        // Load bundle from file path
        var bundleLoad = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleLoad;
        var bundle = bundleLoad.assetBundle;
        if (bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle at " + bundlePath);
            yield break;
        }

        // Load VideoClip from bundle
        var clipLoad = bundle.LoadAssetAsync<VideoClip>(videoClipNameInBundle);
        yield return clipLoad;
        VideoClip videoClip = clipLoad.asset as VideoClip;
        if (videoClip == null)
        {
            Debug.LogError("VideoClip not found inside bundle: " + videoClipNameInBundle);
            bundle.Unload(false);
            yield break;
        }

        // Assign clip to VideoPlayer
        videoPlayer.clip = videoClip;
        currentVideoClip = videoClip;
        loadedBundle = bundle; // Keep reference to bundle
        
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
    public VideoClip GetCurrentVideoClip()
    {
        return currentVideoClip;
    }

    // Optional: Check if video is loaded and ready
    public bool IsVideoLoaded()
    {
        return videoPlayer != null && videoPlayer.clip != null;
    }
}

