using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 加载Assetbundle;
/// </summary>
public class AssetBundleLoader 
{
    private AssetLoader assetLoader;
    /// <summary>
    /// 通过WWW 加载
    /// </summary>
    private WWW www;
    /// <summary>
    /// 包名
    /// </summary>
    private string bundleName;
    /// <summary>
    /// 包的路径
    /// </summary>
    private string bundlePath;
    /// <summary>
    /// 进度
    /// </summary>
    private float progress;

    private Action<string, float> loadProgress;
    /// <summary>
    /// 加载完成
    /// </summary>
    private Action<string> loadComplete;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="loadProgress">加载进度</param>
    /// <param name="loadComplete">加载完成</param>
    public AssetBundleLoader(string bundleName,Action<string,float> loadProgress,Action<string> loadComplete)
    {
        this.bundleName = bundleName;
        this.loadProgress = loadProgress;
        this.loadComplete = loadComplete;
        this.bundlePath = PathUtil.GetWWWAeestBundelPath()+"/"+bundleName;
        this.progress = 0.0f;
        this.www = null;
        this.assetLoader = null;
    }
    /// <summary>
    /// 加载资源包
    /// </summary>
    /// <returns></returns>
    public IEnumerator Load()
    {
        
        www = new WWW(bundlePath);
        while (!www.isDone)
        {
            progress = www.progress;
            if (loadProgress != null)
                loadProgress(bundleName, progress);
            yield return www;
        }
        progress = www.progress;
        if (progress >= 1.0f)
        {
            assetLoader = new AssetLoader();
            assetLoader.SetAssetBundle(www.assetBundle);
            if (loadProgress != null)
                loadProgress(bundleName, progress);

            if (loadComplete != null)
            {
                loadComplete(bundleName);
            }
        }
    }

    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名字</param>
    /// <returns>特定的资源</returns>
    public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {
        if (assetLoader != null)
        {
            return  assetLoader.LoadAsset<T>(assetName);
        }
        return null;
       
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <returns></returns>
    public UnityEngine.Object[] LoadAllAsset()
    {
        if (assetLoader == null)
        {
            
            return null;
        }
        else
        {
            return assetLoader.LoadAllAsset();
        }
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <param name="assetName">资源名字</param>
    /// <returns></returns>
    public UnityEngine.Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (assetLoader != null)
        {
             return assetLoader.LoadAssetWithSubAssets(assetName);
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
        if (this.assetLoader == null) return;
        assetLoader.UnLoadAsset(asset);
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (assetLoader != null)
        {
            assetLoader.Dispose();
        }
        assetLoader = null;
    }
}
