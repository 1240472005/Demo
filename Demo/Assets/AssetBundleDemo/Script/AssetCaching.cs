using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 资源缓存层
/// </summary>
public class AssetCaching
{
    /// <summary>
    /// 资源名字和资源的映射
    /// </summary>
    private Dictionary<string, TempObject> nameAssetDict;

    public AssetCaching()
    {
        nameAssetDict = new Dictionary<string, TempObject>();
    }
    public void AddAsset(string assetName,TempObject asset)
    {
        if (nameAssetDict.ContainsKey(assetName))
        {
            return;
        }
        nameAssetDict.Add(assetName, asset);
    }
    public Object[] GetAsset(string assetName)
    {
        if (nameAssetDict.ContainsKey(assetName))
        {
            return nameAssetDict[assetName].AssetList.ToArray();
        }
        return null;
    }
    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="assetName">资源名字</param>
    public void UnLoadAsset(string assetName)
    {
        if (nameAssetDict.ContainsKey(assetName))
        {
            nameAssetDict[assetName].UnLoadAsset();
        }
    }
    /// <summary>
    /// 卸载所有的资源
    /// </summary>
    public void UnLoadAllAsset()
    {
        foreach (var v in nameAssetDict.Keys)
        {
            UnLoadAsset(v);
        }
    }
}

