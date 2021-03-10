using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneAssetBundles 
{
    private Dictionary<string, AssetBundleRelation> nameBundleDict;

    private string sceneName;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sceneName"></param>
    public OnSceneAssetBundles(string sceneName)
    {
        this.sceneName = sceneName;
        nameBundleDict = new Dictionary<string, AssetBundleRelation>();
    }

    public IEnumerator Load(string bundleName)
    {
        while (!AssetBundleManifestLoader.Instance.IsFinish)
        {
            yield return null;
        }
        AssetBundleRelation assetBundleRelation = nameBundleDict[bundleName];
        //先获取所有包的依赖关系
        string[] dependenceBundles=AssetBundleManifestLoader.Instance.GetDependencies(bundleName);

        foreach (var dependBundleName in dependenceBundles)
        {
            assetBundleRelation.AddDependence(dependBundleName);
            yield return LoadDependence(dependBundleName, bundleName, assetBundleRelation.LoadProgress);
        }
        //开始加载这个包
        yield return assetBundleRelation.Load();
    }
    private IEnumerator LoadDependence(string bundleName, string referenceBundleName, System.Action<string, float> loadProgress)
    {
        if (nameBundleDict.ContainsKey(bundleName))
        {
            AssetBundleRelation assetBundleRelation = nameBundleDict[bundleName];
            //添加这个包的被依赖关系
            assetBundleRelation.AddReference(referenceBundleName);
        }
        else
        {
            //没有加载过 就创建一个新的
            AssetBundleRelation assetBundleRelation = new AssetBundleRelation(bundleName,loadProgress);
            nameBundleDict.Add(bundleName, assetBundleRelation);
            assetBundleRelation.AddReference(bundleName);
            yield return Load(bundleName);
        }
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <returns></returns>
    public Object[] LoadAllAsset(string bundleName)
    {
        if (nameBundleDict.ContainsKey(bundleName))
        {
            return null;
        }
        else
        {
            return nameBundleDict[bundleName].LoadAllAsset();
        }
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <param name="assetName">资源名字</param>
    /// <returns></returns>
    public Object[] LoadAssetWithSubAssets(string bundleName)
    {
        if (nameBundleDict.ContainsKey(bundleName))
        {
            return nameBundleDict[bundleName].LoadAssetWithSubAssets(bundleName);
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="asset"></param>
    public void UnLoadAsset(string bundleName, Object asset)
    {
        if (!nameBundleDict.ContainsKey(bundleName)) return;
        nameBundleDict[bundleName].UnLoadAsset(asset);
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose(string bundleName)
    {
        if (nameBundleDict.ContainsKey(bundleName))
        {
            nameBundleDict[bundleName].Dispose();
        }
        nameBundleDict[bundleName] = null;
        nameBundleDict.Remove(bundleName);
    }
}
