using System;
using UnityEngine;
using System.Collections;

public class AssetBundleLoader : MonoBehaviour
{
    string path;
    GameObject instantiated;
    
    public static event Action<GameObject> OnFinish;

    void Awake()
    {
        path = Application.streamingAssetsPath +  "/bundles";
    }

    public void StartBundleLoad(string bundleName, string assetName, Transform parent = null)
    {
        StartCoroutine(LoadAssetBundleRoutine(bundleName, assetName, parent));
    }

    IEnumerator LoadAssetBundleRoutine(string bundleName, string assetName, Transform parent = null)
    {
        AssetBundleCreateRequest loadRequest = AssetBundle.LoadFromFileAsync(path + "/" + bundleName);
        yield return loadRequest;
        
        var bundleLoad = loadRequest.assetBundle;
        if (!bundleLoad) yield break;

        var prefabLoad = bundleLoad.LoadAssetAsync<GameObject>(assetName);
        yield return prefabLoad;
        
        GameObject prefab = prefabLoad.asset as GameObject;
        if(!prefab)  yield break;
        instantiated = Instantiate(prefab);
        
        OnFinish?.Invoke(instantiated);
    }
}
