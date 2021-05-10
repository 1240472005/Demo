using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneAssetBundles 
{
    private Dictionary<string, AssetBundleRelation> nameBundleDict;

    private Dictionary<string, AssetCaching> nameCacheDict;
    private string sceneName;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sceneName"></param>
    public OnSceneAssetBundles(string sceneName)
    {
        this.sceneName = sceneName;
        nameBundleDict = new Dictionary<string, AssetBundleRelation>();
        nameCacheDict = new Dictionary<string, AssetCaching>();
    }

    /// <summary>
    /// 加载资源包
    /// </summary>
    public void LoadAssetBundle(string bundleName,System.Action<string,float> loadProgress,System.Action<string,string> loadCB)
    {
        if (nameBundleDict.ContainsKey(bundleName))
        {
            Debug.LogWarning("此包已经加载了：" + bundleName);
            return;
        }
        else
        {
            //没有被加载
            AssetBundleRelation assetBundleRelation = new AssetBundleRelation(bundleName, loadProgress);
            //保存到字典里面
            nameBundleDict.Add(bundleName, assetBundleRelation);
            loadCB(sceneName, bundleName);
        }
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
    /// 获取单个资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名字</param>
    /// <returns>特定的资源</returns>
    public T LoadAsset<T>(string bundleName,string assetName) where T : UnityEngine.Object
    {
        if (nameCacheDict.ContainsKey(bundleName))
        {
            UnityEngine.Object[] assets = nameCacheDict[bundleName].GetAsset(assetName);
            if (assets != null)
            {
                return assets[0] as T;
            }
            else
            {
                
            }
        }
        if (nameBundleDict.ContainsKey(bundleName))
        {
            Object asset = nameBundleDict[bundleName].LoadAsset<T>(assetName);
            TempObject tempObject = new TempObject(asset);
            //有这个缓存层 但是 这次获取的资源以前没有做缓存
            if (nameCacheDict.ContainsKey(bundleName))
            {
                nameCacheDict[bundleName].AddAsset(assetName, tempObject);
            }
            else
            { 
            AssetCaching caching = new AssetCaching();
            //保存到字典里面 方便下次使用
            caching.AddAsset(assetName, tempObject);
            }



            return asset as T;
        }
        return null;

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
    public bool Loading(string bundleName)
    {
        return nameBundleDict.ContainsKey(bundleName);
    }
    public bool Finsh(string bundleName)
    {
        if (Loading(bundleName))
        {
            return nameBundleDict[bundleName].IsFinish;
        }
        return false;
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <param name="assetName">资源名字</param>
    /// <returns></returns>
    public Object[] LoadAssetWithSubAssets(string bundleName,string assetName)
    {
        //if (nameBundleDict.ContainsKey(bundleName))
        //{
        //    return nameBundleDict[bundleName].LoadAssetWithSubAssets(assetName);
        //}
        //else
        //{
        //    return null;
        //}
        if (nameCacheDict.ContainsKey(bundleName))
        {
            UnityEngine.Object[] assets = nameCacheDict[bundleName].GetAsset(assetName);
            if (assets != null)
            {
                return assets;
            }
            else
            {

            }
        }
        if (nameBundleDict.ContainsKey(bundleName))
        {
            Object[] asset = nameBundleDict[bundleName].LoadAssetWithSubAssets(assetName);
            TempObject tempObject = new TempObject(asset);
            //有这个缓存层 但是 这次获取的资源以前没有做缓存
            if (nameCacheDict.ContainsKey(bundleName))
            {
                nameCacheDict[bundleName].AddAsset(assetName, tempObject);
            }
            else
            {
                AssetCaching caching = new AssetCaching();
                //保存到字典里面 方便下次使用
                caching.AddAsset(assetName, tempObject);
            }

            return asset ;
        }
        return null;
    }
    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="asset"></param>
    public void UnLoadAsset(string bundleName, Object asset)
    {
        if (!nameBundleDict.ContainsKey(bundleName)) return;
        nameBundleDict[bundleName].UnLoadAsset(asset);
        nameCacheDict.Remove(bundleName);
        Resources.UnloadUnusedAssets();
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose(string bundleName)
    {
        if (!nameBundleDict.ContainsKey(bundleName))
        {
            return;
        }

        AssetBundleRelation assetBundleRelation = nameBundleDict[bundleName];
        //获取当前包的依赖
        string[] allDependences = assetBundleRelation.GetAllDependence();
        foreach (string dependenceBundleName in allDependences)
        {
            //首先移除 依赖包里面的被依赖关系
            AssetBundleRelation bundleRelation = nameBundleDict[dependenceBundleName];
            if (bundleRelation != null)
            {
                if (bundleRelation.RemoveReference(bundleName))
                {
                    //递归释放
                    Dispose(bundleRelation.BundleName);
                }
            }
        }
        //才开是卸载当前包
        if (assetBundleRelation.GetAllReference().Length <= 0)
        {
          nameBundleDict[bundleName].Dispose();
          nameBundleDict.Remove(bundleName);
        }

    }
    /// <summary>
    /// 卸载所有的包
    /// </summary>
    public void DisposeAll()
    {
        foreach (var item in nameBundleDict.Keys)
        {
            Dispose(item);
        }
    }
    
}
