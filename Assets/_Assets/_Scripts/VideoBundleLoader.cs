using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VideoBundleLoader : MonoBehaviour
{
    string videoClipNameInBundle = "VideoClip";
    VideoPlayer videoPlayer;
    AssetBundle loadedBundle;
    string currentVideoPath;

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

    public void StartLoad(string bundleAbsolutePath)
    {
        StartCoroutine(LoadBundleCoroutine(bundleAbsolutePath));
    }

    private IEnumerator LoadBundleCoroutine(string bundlePath)
    {
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
            Debug.LogError("Video not found. Assets in bundle:\n" + string.Join("\n", names));
            yield break;
        }

        // Load as TextAsset
        var clipLoad = bundle.LoadAssetAsync<TextAsset>(match);
        yield return clipLoad;

        var textAsset = clipLoad.asset as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError("Found asset but not a TextAsset: " + match);
            yield break;
        }

        // Normalize filename (remove .bytes/.mp4.bytes)
        string cleanName = Path.GetFileNameWithoutExtension(match); // removes last extension
        if (cleanName.EndsWith(".mp4")) // handles .mp4.bytes case
            cleanName = Path.GetFileNameWithoutExtension(cleanName);

        string tempPath = Path.Combine(Application.persistentDataPath, cleanName + ".mp4");
        File.WriteAllBytes(tempPath, textAsset.bytes);

        // Play from file
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = tempPath;
        videoPlayer.Prepare();

        currentVideoPath = tempPath;

    }

    public void UnloadAndCleanup()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
            videoPlayer.url = null;
        }

        if (loadedBundle != null)
        {
            loadedBundle.Unload(true);
            loadedBundle = null;
        }

        if (!string.IsNullOrEmpty(currentVideoPath) && File.Exists(currentVideoPath))
        {
            File.Delete(currentVideoPath);
        }

        currentVideoPath = null;
    }

    public bool IsVideoLoaded() => videoPlayer != null && !string.IsNullOrEmpty(videoPlayer.url);
}
