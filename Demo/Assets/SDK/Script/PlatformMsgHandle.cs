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
        PlatformMsg msg = new PlatformMsg
        {
            iMsgId = (int) jd["iMsgId"],
            iPararm1 = (int) jd["iPararm1"],
            iPararm2 = (int) jd["iPararm2"],
            iPararm3 = (int) jd["iPararm3"],
            strPararm1 = (string) jd["strPararm1"],
            strPararm2 = (string) jd["strPararm2"],
            strPararm3 = (string) jd["strPararm3"]
        };

        msgQueue.Enqueue(msg);
    }
}
#endif