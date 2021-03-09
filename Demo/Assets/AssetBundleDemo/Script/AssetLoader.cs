using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : System.IDisposable
{
    private AssetBundle m_assetBundle;


    /// <summary>
    /// 设置资源包
    /// </summary>
    /// <param name="bundle"></param>
    public void SetAssetBundle(AssetBundle bundle)
    {
        this.m_assetBundle = bundle;
    }
    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名字</param>
    /// <returns>特定的资源</returns>
    public T LoadAsset<T>(string assetName) where T : Object
    {
        if (m_assetBundle != null)
        {
            if (m_assetBundle.Contains(assetName))
            {
                return m_assetBundle.LoadAsset(assetName) as T;
            }
            else
            {
                Debug.LogError(($"当前资源包不包含{0}资源", assetName));
                return null;
            }
        }
        else
        {
            Debug.LogError(($"当前资源包为空，无法获取{0}资源", assetName));
            return null;
        }
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <returns></returns>
    public Object[] LoadAllAsset()
    {
        if (m_assetBundle == null)
        {
            Debug.LogError(($"当前资源包{0}为空", m_assetBundle.name));
            return null;
        }
        else
        {
            return m_assetBundle.LoadAllAssets();
        }
    }
    /// <summary>
    /// 获取所有资源
    /// </summary>
    /// <param name="assetName">资源名字</param>
    /// <returns></returns>
    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (m_assetBundle != null)
        {
            if (m_assetBundle.Contains(assetName))
            {
                return m_assetBundle.LoadAssetWithSubAssets(assetName);
            }
            else
            {
                Debug.LogError(($"当前资源包不包含{0}资源", assetName));
                return null;
            }
        }
        else
        {
            Debug.LogError(($"当前资源包为空，无法获取{0}资源", assetName));
            return null;
        }
    }
    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="asset"></param>
    public void UnLoadAsset(Object asset)
    {
        if (this.m_assetBundle == null) return;
        Resources.UnloadAsset(asset);
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (m_assetBundle != null)
        {
            
            m_assetBundle.Unload(false);
        }
    }

    public void GetAll()
    {
        string[] names = m_assetBundle.GetAllAssetNames();
        foreach (var item in names)
        {
            Debug.Log(item);
        }
    }

    
}
