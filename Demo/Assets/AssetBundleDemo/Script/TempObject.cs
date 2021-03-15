using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject 
{
    private List<Object> assetList;

    public List<Object> AssetList => assetList;

    public TempObject(params Object[] args)
    {
        assetList = new List<Object>(args);
    }

    public void UnLoadAsset()
    {
        foreach (var v in assetList)
        {
            Resources.UnloadAsset(v);
        }
        assetList.Clear();
    }

}
