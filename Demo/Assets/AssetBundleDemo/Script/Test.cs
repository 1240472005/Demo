using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Test : MonoBehaviour
{
    private void Awake()
    {
        //StartCoroutine(AssetBundleManifestLoader.Instance.Load());
    }
    // Start is called before the first frame update
    void Start()
    {
        //AssetBundleManifestLoader.Instance.action = LoadCube;
        string path = PathUtil.GetAssetBundelOutPath() + "/" + "StreamingAssets";
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        AssetBundleManifest manifest = bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        LoadCube(manifest);
    }
    string[] depe;
    private void LoadCube(AssetBundleManifest manifest)
    {
        depe= manifest.GetAllAssetBundles();
        string path = PathUtil.GetAssetBundelOutPath() + "/" + depe[2];

        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        var prefab = assetBundle.LoadAsset("image");
        Instantiate(prefab, transform);
    }
    public void Click()
    {
        string path = PathUtil.GetAssetBundelOutPath() + "/" + depe[1];

        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        SceneManager.LoadScene("Scene1");
        //StartCoroutine(Load());
    }
    public IEnumerator Load()
    {
        WWW www = new WWW(PathUtil.GetWWWAeestBundelPath() + "/" + depe[1]);
        yield return www;

        if (www.error != null)
        {
            Debug.LogError("加载Manifest出错:www.error");
        }
        else
        {
            if (www.progress >= 1.0f)
            {
                AssetBundle assetBundle = www.assetBundle;
                SceneManager.LoadScene("scene1");
            }
        }
    }
    

}

public interface ICommond 
{
    string commondType { get; set; }
    void Action();
}
