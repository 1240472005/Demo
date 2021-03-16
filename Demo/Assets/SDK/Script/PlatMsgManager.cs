using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using LitJson;
/// <summary>
/// 平台消息类
/// </summary>
public class PlatMsgManager : MonoBehaviour
{
    private static PlatMsgManager m_Instance;
    public static PlatMsgManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
    GameObject m_PlatformObject = null;
    public PlatformMsgHandle m_PlatformMsgHandle;


#if UNITY_ANDROID && !UNITY_EDITOR
    AndroidJavaClass m_GameHelperJavaClass = null;
#endif

    private void Awake()
    {
        m_Instance = this;
        if (m_PlatformObject == null)
        {
            //因为要一直处理msg消息
            m_PlatformObject = new GameObject("PlatformObject");
            m_PlatformObject.hideFlags = HideFlags.HideAndDontSave;
            m_PlatformMsgHandle = m_PlatformObject.GetComponent<PlatformMsgHandle>() ?? m_PlatformObject.AddComponent<PlatformMsgHandle>();
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            GameObject.DontDestroyOnLoad(m_PlatformObject);


            Debug.Log(GetWindowAvailMemory()/(1024*1024.0f));
            Debug.Log(GetWindowTotalMemory() / (1024 * 1024.0f));
            Debug.Log(GetWindowUseMemory() / (1024 * 1024.0f));
        }


        //包名加类名 实现Java里面的类
#if UNITY_ANDROID && !UNITY_EDITOR
      m_GameHelperJavaClass = new AndroidJavaClass("com.Company.QC.GameHelper");
        Debug.LogError("com.Company.QC.GameHelper is Init");
#endif
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    /// <summary>
    /// 从Unity发送消息到平台
    /// </summary>
    /// <param name="args"></param>
    public void SendUnityMessageToPlatform(params object[] args)
    {
        if (m_GameHelperJavaClass == null)
        {
            return;
        }
        m_GameHelperJavaClass.CallStatic("SendUnityMessageToPlatform", args);
    }
    /// <summary>
    /// 从平台获取整形数据
    /// </summary>
    /// <returns></returns>
    public int GetIntFromPlatform(int type)
    {
        if (m_GameHelperJavaClass == null) return 0;
        return m_GameHelperJavaClass.CallStatic<int>("GetIntFromPlatform", type);
    }
#endif
    public const int UNITY_GET_LONG_AVAILABLEMEMORY = 1;//可用内存大小
    public const int UNITY_GET_LONG_TOTALEMEMORY = 2;//总共内存大小
    public const int UNITY_GET_LONG_USEDEMEMORY = 3;//可运行内存大小
    /// <summary>
    /// 获取windows的内存
    /// </summary>
    [StructLayout(LayoutKind.Sequential,Pack =1)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }
    [DllImport("kernel32.dll")]
    protected static extern void GlobalMemoryStatus(ref MEMORYSTATUSEX laBuff);

    protected ulong GetWindowAvailMemory()
    {
        MEMORYSTATUSEX ms = new MEMORYSTATUSEX();
        ms.dwLength = 64;
        GlobalMemoryStatus(ref ms);
        return ms.ullAvailPhys;
    }
    protected ulong GetWindowTotalMemory()
    {
        MEMORYSTATUSEX ms = new MEMORYSTATUSEX();
        ms.dwLength = 64;
        GlobalMemoryStatus(ref ms);
        return ms.ullTotalPhys;
    }
    protected long GetWindowUseMemory()
    {
        return UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
    }
}
