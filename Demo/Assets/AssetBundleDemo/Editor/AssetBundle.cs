using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundle 
{
    [MenuItem("AssetBundle/SetAssetBundleLabels")]
    public static void SetAssetBundleLables()
    {
        //移除所有没有使用标记的标记
        AssetDatabase.RemoveUnusedAssetBundleNames();
        //路径
        string path = Application.streamingAssetsPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        foreach (var v in directoryInfos)
        {
            
        }
    }
}
