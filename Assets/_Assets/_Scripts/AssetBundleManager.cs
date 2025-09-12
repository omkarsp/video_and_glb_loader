using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

public class AssetBundleManager : MonoBehaviour
{
    private Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
    
    public async Task<T> LoadAssetFromBundle<T>(string bundleName, string assetName) where T : Object
    {
        AssetBundle bundle = await LoadBundle(bundleName);
        return bundle.LoadAsset<T>(assetName);
    }
    
    private async Task<AssetBundle> LoadBundle(string bundleName)
    {
        if (loadedBundles.ContainsKey(bundleName))
            return loadedBundles[bundleName];
            
        string bundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles", bundleName);
        var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        await bundleRequest;
        
        loadedBundles[bundleName] = bundleRequest.assetBundle;
        return bundleRequest.assetBundle;
    }
}