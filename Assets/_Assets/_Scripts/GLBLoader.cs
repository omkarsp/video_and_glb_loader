using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using GLTFast;

public class GLBLoader : MonoBehaviour
{
	[SerializeField] Transform parent;

	GameObject _root;
	AssetBundle _bundle;
	GltfImport _import;

	public async void StartLoad(string bundlePath, string assetName)
	{
		await LoadGlbFromBundle(bundlePath, assetName);
	}

	public async Task<GameObject> LoadGlbFromBundle(string bundlePath, string assetName)
	{
		Clear();

		_bundle = await AssetBundleHelper.LoadBundleAsync(bundlePath);

		if (_bundle == null)
		{
			Debug.LogError($"GLBLoader: Failed to load AssetBundle at {bundlePath}");
			return null;
		}
		
		Debug.Log("Bundle contains assets:\n" + string.Join("\n", _bundle.GetAllAssetNames()));

		var parentTx = parent != null ? parent : transform;

		// 1) Try to load as Prefab (glTFast imported asset)
		GameObject prefab = null;
		prefab = _bundle.LoadAsset<GameObject>(assetName)
			?? _bundle.LoadAsset<GameObject>(assetName + ".prefab");

		if (prefab == null)
		{
			var names = _bundle.GetAllAssetNames();
			string match = null;
			var needle = assetName.ToLowerInvariant();
			foreach (var n in names)
			{
				var file = Path.GetFileNameWithoutExtension(n);
				if (file == needle || n.Contains(needle))
				{
					match = n;
					break;
				}
			}
			if (!string.IsNullOrEmpty(match))
			{
				prefab = _bundle.LoadAsset<GameObject>(match);
			}
		}

		if (prefab != null)
		{
			_root = Instantiate(prefab, parentTx);
			_root.name = prefab.name;
			FixupShaders(_root);
			return _root;
		}

		// 2) Fallback: Try raw GLB bytes as TextAsset and instantiate via glTFast
		TextAsset glb = null;
		glb = _bundle.LoadAsset<TextAsset>(assetName)
			?? _bundle.LoadAsset<TextAsset>(assetName + ".glb");

		if (glb == null)
		{
			var names = _bundle.GetAllAssetNames();
			string match = null;
			var needle = assetName.ToLowerInvariant();
			foreach (var n in names)
			{
				if (n.EndsWith(".glb") && n.Contains(needle))
				{
					match = n;
					break;
				}
			}
			if (!string.IsNullOrEmpty(match))
			{
				glb = _bundle.LoadAsset<TextAsset>(match);
			}
		}

		if (glb == null)
		{
			Debug.LogError(
				$"GLBLoader: Asset '{assetName}' not found in bundle '{Path.GetFileName(bundlePath)}' as Prefab or TextAsset.\n" +
				string.Join("\n", _bundle.GetAllAssetNames())
			);
			return null;
		}

		_import = new GltfImport();

		var loaded = await _import.LoadGltfBinary(glb.bytes);
		if (!loaded)
		{
			Debug.LogError("GLBLoader: glTFast failed to parse GLB data.");
			return null;
		}

		_root = new GameObject(string.IsNullOrEmpty(assetName) ? "GLB_Root" : assetName);
		_root.transform.SetParent(parentTx, false);

		var instantiated = await _import.InstantiateMainSceneAsync(_root.transform);
		if (!instantiated)
		{
			Debug.LogError("GLBLoader: glTFast failed to instantiate scene.");
			Clear();
			return null;
		}

		FixupShaders(_root);

		return _root;
	}

	public void Clear()
	{
		if (_root != null)
		{
			Destroy(_root);
			_root = null;
		}

		if (_import != null)
		{
			_import.Dispose();
			_import = null;
		}

		if (_bundle != null)
		{
			_bundle.Unload(false);
			_bundle = null;
		}
	}

	void FixupShaders(GameObject go)
	{
		if (go == null) return;
		var renderers = go.GetComponentsInChildren<Renderer>(true);
		for (int r = 0; r < renderers.Length; r++)
		{
			var mats = renderers[r].sharedMaterials;
			for (int m = 0; m < mats.Length; m++)
			{
				var mat = mats[m];
				if (mat == null) continue;
				var shaderName = mat.shader != null ? mat.shader.name : null;
				if (string.IsNullOrEmpty(shaderName)) continue;
				var found = Shader.Find(shaderName);
				if (found != null && found != mat.shader)
				{
					mat.shader = found;
				}
			}
		}
	}
}