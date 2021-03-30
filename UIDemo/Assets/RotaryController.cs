using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
/// <summary>
/// 轮转控制器 用以实现轮转效果
/// </summary>
public class RotaryController
{
    /// <summary>
    /// 子项物体的大小
    /// </summary>
    private Vector2 m_subItemSize;
    /// <summary>
    /// 父物体
    /// </summary>
    private Transform parent;
    /// <summary>
    /// 子物体
    /// </summary>
    private List<RotaryItem> subItemList;
    private List<SubItemPosData> _posdata;
    /// <summary>
    /// 当前
    /// </summary>
    public float scaleMax;//缩放最大值
    public float scaleMin;//缩放最小值

    private float subItemOffset;//子物体之间的间距
    //按钮监听事件
    private Action m_actionListener = null;

    public void Init(Transform parent, Vector2 subItemSize, List<Sprite> sprites, Action action = null)
    {

    }
    /// <summary>
    /// 创建滑动模板
    /// </summary>
    /// <returns></returns>
    private GameObject CreateTemplate()
    {
        GameObject item = new GameObject("Template");
        item.AddComponent<RectTransform>().sizeDelta = m_subItemSize;
        item.AddComponent<Button>();
        item.AddComponent<RotaryItem>();
        return item;
    }
    /// <summary>
    /// 创建子物体 
    /// </summary>
    private void CreateSubItem(List<Sprite> s, Transform parent, Action action)
    {
        GameObject templete = CreateTemplate();
        RotaryItem rotaryTempItem = null;
        int count = s.Count;
        for (int i = 0; i < count; i++)
        {
            rotaryTempItem = GameObject.Instantiate(templete).GetComponent<RotaryItem>();
            rotaryTempItem.transform.SetParent(parent);
        }
    }
    private void CalulateData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        int subItemCount = subItemList.Count;
        //计算周长
        float length = (m_subItemSize.x + subItemOffset) * subItemCount;
        //每个物体分配的比例
        float radioOffset = 1.0f / subItemCount;
        float radio = 0;
        for (int i = 0; i < subItemCount; i++)
        {
            ItemData iData = new ItemData();
            iData.PosId = i;
            itemDatas.Add(iData);
            subItemList[i].PosID = i;


            SubItemPosData data = new SubItemPosData();
            data.x = GetX(radio, length);
            data.scale = GetScale(radio, scaleMax, scaleMin);
            radio += radioOffset;
            _posdata.Add(data);
        }
        //排序
        itemDatas.Sort((a, b) =>
        {
            if (_posdata[b.PosId].scale > _posdata[a.PosId].scale)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        });

        for (int i = 0; i < itemDatas.Count; i++)
        {
            _posdata[itemDatas[i].PosId].order = i;
        }
    }
    private void SetItemData()
    {
        for (int i = 0; i < _posdata.Count; i++)
        {
            subItemList[i].SetPosData(_posdata[i]);
        }
    }

    private float GetX(float radio, float length)
    {
        if (radio > 1 || radio < 0)
        {
            return 0;
        }
        if (radio >= 0 && radio < 0.25f)
        {
            return length * radio;
        }
        else if (radio >= 0.25f && radio < 0.75f)
        {
            return length * (0.5f - radio);
        }
        else
        {
            return length * (radio - 1);
        }
    }
    /// <summary>
    /// 获取缩放系数
    /// </summary>
    /// <returns></returns>
    public float GetScale(float radio, float max, float min)
    {
        if (radio > 1 || radio < 0)
        {
            return 0;
        }
        float scaleOffset = (max - min) / 0.5f;
        if (radio < 0.5f)
        {
            return max - scaleOffset * radio;
        }
        else
        {
            return max - scaleOffset * (1 - radio);
        }
    }
    /// <summary>
    /// 子物体位置数据
    /// </summary>
    public class SubItemPosData
    {
        public float x;
        public float scale;
        public int order;
    }
    public struct ItemData
    {
        public int PosId;
    }

}
