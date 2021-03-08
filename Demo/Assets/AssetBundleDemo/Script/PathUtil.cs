using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// 路径
/// </summary>
public class PathUtil 
{

    public static string GetAssetBundelOutPath()
    {
        string outPath = GetPlatformPath();
        if(!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        return outPath;
    }


    /// <summary>
    /// 自动获取平台路径
    /// </summary>
    /// <returns></returns>
    private static string GetPlatformPath()
    {
        switch (Application.platform)
        {
           
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return Application.streamingAssetsPath;
            case RuntimePlatform.Android:
                return Application.persistentDataPath;
            //case RuntimePlatform.IPhonePlayer:
            //    break;
            default:
                return null;
        }
    }

    public static string GetWWWAeestBundelPath()
    {
        switch (Application.platform)
        {

            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "file:///"+GetAssetBundelOutPath();
            case RuntimePlatform.Android:
                return "jar:file://"+GetAssetBundelOutPath();
            //case RuntimePlatform.IPhonePlayer:
            //    break;
            default:
                return null;
        }
    }
}
