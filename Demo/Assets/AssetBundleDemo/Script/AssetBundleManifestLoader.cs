using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载ManiFest文件
/// </summary>
public class AssetBundleManifestLoader
{
    private static AssetBundleManifestLoader m_Instance;

    public static AssetBundleManifestLoader Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new AssetBundleManifestLoader();
            }
            return m_Instance;
        }
    }
    /// <summary>
    /// Manifest文件
    /// </summary>
    private AssetBundleManifest manifest;
    public AssetBundleManifest Manifest=>manifest;
    /// <summary>
    /// 路径
    /// </summary>
    private string manifestPath;
    /// <summary>
    /// 全局存在的AssetBundle
    /// </summary>
    public AssetBundle assetBundle;

    private bool isFinish;

    public System.Action action;

    public bool IsFinish => isFinish;

    //TODO
    private AssetBundleManifestLoader()
    {
        this.manifestPath = PathUtil.GetWWWAeestBundelPath()+"/"+ "StreamingAssets";
        Debug.Log(manifestPath);
        this.manifest = null;
        this.assetBundle = null;
        isFinish = false;
    }
    public IEnumerator Load()
    {
        WWW www = new WWW(manifestPath);
        yield return www;

        if (www.error != null)
        {
            Debug.LogError("加载Manifest出错:www.error");
        }
        else
        {
            if (www.progress >= 1.0f)
            {
                this.assetBundle = www.assetBundle;
                this.manifest = this.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                action.Invoke();
                this.isFinish = true;
            }
        }
    }

    /// <summary>
    /// 获取所有的依赖关系
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <returns></returns>
    public string[] GetDependencies(string bundleName)
    {
        return manifest.GetAllDependencies(bundleName);
    }
    /// <summary>
    /// 卸载manifest
    /// </summary>
    public void UnLoad()
    {
        assetBundle.Unload(true);
    }
}
