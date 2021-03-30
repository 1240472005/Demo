using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RotaryItem:MonoBehaviour
{
    RotaryController.SubItemPosData data;
    public int PosID;


    private RectTransform _rect;

    private RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();

            }
             return _rect;
        }
    }



    public void SetSprite(Sprite s)
    {
        GetComponent<Image>().sprite = s;
    }

    public void SetPosData(RotaryController.SubItemPosData posdata)
    {
        Rect.anchoredPosition = Vector3.right*posdata.x;
        Rect.localScale = Vector3.one * posdata.scale;
    }
}