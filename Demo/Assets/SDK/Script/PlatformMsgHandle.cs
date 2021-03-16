using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
/// <summary>
/// 平台消息处理类
/// </summary>
#if !UNITY_IOS || UNITY_ANDROID
public class PlatformMsgHandle : MonoBehaviour
{
    private struct PlatformMsg
    {
        public int iMsgId;
        public int iPararm1;
        public int iPararm2;
        public int iPararm3;
        public string strPararm1;
        public string strPararm2;
        public string strPararm3;
    }
    /// <summary>
    /// 消息队列
    /// </summary>
    private Queue<PlatformMsg> msgQueue = new Queue<PlatformMsg>();
    /// <summary>
    /// 消息处理类
    /// </summary>
    /// <param name="param"></param>
    protected void OnMessage(string param)
    {
        JsonData jd= LitJson.JsonMapper.ToObject(param);
        PlatformMsg msg=new PlatformMsg();
        msg.iMsgId = (int)jd["iMsgId"];
        msg.iPararm1 = (int)jd["iPararm1"];
        msg.iPararm1 = (int)jd["iPararm2"];
        msg.iPararm1 = (int)jd["iPararm3"];
        msg.strPararm1 = (string)jd["strPararm1"];
        msg.strPararm1 = (string)jd["strPararm2"];
        msg.strPararm1 = (string)jd["strPararm3"];

        msgQueue.Enqueue(msg);
    }
}
#endif