﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateBundles 
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string outPath = PathUtil.GetAssetBundelOutPath();
        if (!System.IO.Directory.Exists(outPath))
        {
            System.IO.Directory.CreateDirectory(outPath); 
        }
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);
    }
}
