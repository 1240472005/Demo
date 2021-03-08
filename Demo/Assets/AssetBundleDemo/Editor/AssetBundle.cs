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
        string path = Application.dataPath+"/Resources/";
        
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        foreach (var tempDirectoryInfo in directoryInfos)
        {
            //路径和名字要区分开来
            string sceneDirectory = path  + tempDirectoryInfo.Name;
            DirectoryInfo sceneDirectoryInfo = new DirectoryInfo(sceneDirectory);
            if (sceneDirectory == null)
            {
                return;
            }
            else
            {
                //遍历场景文件夹里面的所有文件系统
                int index = sceneDirectory.LastIndexOf("/");
                string fileName = sceneDirectory.Substring(index + 1);
                TraverseSceneFileSystemInfo(sceneDirectoryInfo, fileName);
            }
        }
    }
    /// <summary>
    /// 遍历场景文件夹里面的所有系统
    /// </summary>
    /// <param name="fileSystemInfo"></param>
    /// <param name="sceneName"></param>
    private static void TraverseSceneFileSystemInfo(FileSystemInfo fileSystemInfo, string fileName)
    {
        if (!fileSystemInfo.Exists)
        {
            Debug.LogError(fileSystemInfo.FullName + "不存在");
            return;
        }
        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (var tempFileSystem in fileSystemInfos)
        {
            FileInfo fileInfo = tempFileSystem as FileInfo;
            if (fileInfo == null)
            {
                //代表强转失败，不是文件，就是文件夹
                //如果访问的是文件夹；再继续访问里面的所有文件系统。直到找到文件
                TraverseSceneFileSystemInfo(tempFileSystem, fileName);
            }
            else
            {
                //就是文件
                //找到文件 修改他的assetbudle labels
                SetLabels(fileInfo, fileName);
            }
        }
    }
    /// <summary>
    /// 设置标签
    /// </summary>
    /// <param name="fileinfo">文件类型</param>
    /// <param name="sceneName">场景文件名</param>
    private static void SetLabels(FileInfo fileinfo, string sceneName)
    {
        if (fileinfo.Extension == ".meta")
        {
            return;
        }
        string budleName = GetBundleName(fileinfo, sceneName);
        //获取资源在Unity下面的路径
        int index = fileinfo.FullName.IndexOf("Assets");
        string assetpath = fileinfo.FullName.Substring(index);
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetpath);
        assetImporter.assetBundleName = budleName.ToLower();
        if (fileinfo.Extension == ".unity")
            assetImporter.assetBundleVariant = "u3d";
        else
            assetImporter.assetBundleVariant = "bytes";
    }
    private static string GetBundleName(FileInfo fileInfo, string fileName)
    {
        string windowPath = fileInfo.FullName;
        //转换Unity可识别路径
        string unityPath = windowPath.Replace(@"\", "/");

        int index = unityPath.IndexOf(fileName);
        //Cube.Prefab
        string bundlePath = unityPath.Substring(index).Split('.')[0];
        return  bundlePath;
       
       
    }
}
