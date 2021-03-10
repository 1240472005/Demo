using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// AB包依赖关系
/// </summary>
public class AssetBundleRelation 
{
    /// <summary>
    /// 加载资源包
    /// </summary>
    private AssetBundleLoader bundleLoader;
    /// <summary>
    /// 包名
    /// </summary>
    private string bundleName;
    /// <summary>
    /// 加载进度回调
    /// </summary>
    private Action<string, float> loadProgress;

    public Action<string, float> LoadProgress => loadProgress;

    private bool isFinish;
    public bool IsFinish => isFinish;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="loadProgress"></param>
    public AssetBundleRelation(string bundleName,Action<string,float> loadProgress)
    {
        this.bundleName = bundleName;
        this.loadProgress = loadProgress; 
        isFinish = false;
        this.bundleLoader = new AssetBundleLoader(bundleName, loadProgress, OnLoadComplete);
        dependenceBundleList = new List<string>();
        referenceBundleList = new List<string>();
     }
    private void OnLoadComplete(string bundleName)
    {
        isFinish = true;
    }

    #region 依赖关系
    private List<string> dependenceBundleList;
    /// <summary>
    /// 移除依赖关系
    /// </summary>
    /// <param name="bundleName">包名</param>
    public void RemoveDependence(string bundleName)
    {
        if (dependenceBundleList.Contains(bundleName))
        {
            dependenceBundleList.Remove(bundleName);
        }
    }
    /// <summary>
    /// 添加依赖关系
    /// </summary>
    /// <param name="bundleName">包名</param>
    public void AddDependence(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
        {
            return;
        }
        if (!dependenceBundleList.Contains(bundleName))
        {
            dependenceBundleList.Add(bundleName);
        }
    }
    /// <summary>
    /// 获取所有的依赖关系
    /// </summary>
    /// <returns></returns>
    public string[] GetAllDependence()
    {
        return dependenceBundleList.ToArray();
    }
    #endregion

    #region 被依赖关系
    //被依赖数组
    private List<string> referenceBundleList;
    /// <summary>
    /// 添加被依赖关系
    ///  <param name="bundleName">包名</param>>
    /// </summary>
    public void AddReference(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
        {
            return;
        }
        if (!referenceBundleList.Contains(bundleName))
        {
            referenceBundleList.Add(bundleName);
        }
    }
    /// <summary>
    /// 移除被依赖关系
    /// <param name="bundleName">包名</param>>
    /// </summary>
    /// <return>true 代表包被释放掉</return>
    public bool RemoveReference(string bundleName)
    {
        if (bundleName.Contains(bundleName))
        {
            referenceBundleList.Remove(bundleName);
            //移除一个包的时候，我们要做一个判断
            //判断是否有其他包还依赖这个包
            //有 无所谓
            //没有就要释放掉这个AssetBundle
            if (referenceBundleList.Count <= 0)
            {
                Dispose();
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 获取所有的依赖关系
    /// </summary>
    /// <returns></returns>
    public string[] GetAllReference()
    {
        return referenceBundleList.ToArray();
    }

    /// <summary>
    /// 加载资源包
    /// </summary>
    /// <returns></returns>
    public IEnumerator Load()
    {

        yield return bundleLoader.Load();
    }
    #endregion

    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名字</param>
    /// <returns>特定的资源</returns>
    public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {
        if (bundleLoader != null)
        {
            return bundleLoader.LoadAsset<T>(assetName);
        }
        return null;

    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <returns></returns>
    public UnityEngine.Object[] LoadAllAsset()
    {
        if (bundleLoader == null)
        {

            return null;
        }
        else
        {
            return bundleLoader.LoadAllAsset();
        }
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <param name="assetName">资源名字</param>
    /// <returns></returns>
    public UnityEngine.Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (bundleLoader != null)
        {
            return bundleLoader.LoadAssetWithSubAssets(assetName);
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
    public void UnLoadAsset(UnityEngine.Object asset)
    {
        if (this.bundleLoader == null) return;
        bundleLoader.UnLoadAsset(asset);
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (bundleLoader != null)
        {
            bundleLoader.Dispose();
        }
        bundleLoader = null;
    }
}
