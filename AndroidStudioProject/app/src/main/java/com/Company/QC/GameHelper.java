package com.Company.QC;

import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class GameHelper {
    private static  MainActivity m_Activity = null;
    private static  String m_PlatformObject = "PlatformObject";//unity 用于通信的物体
    private static String m_MethodName="OnMessage";
    public  static  String TAG="GameHelper";

    public  static  void Init(MainActivity activity)
    {
       m_Activity = activity;
    }
    public  static  void SendPlatformMessageToUnity(int iMsgId,int iParam1,int iParam2,int iParam3,String strParam1,String strParam2,String strParam3)
    {
        String jsonString = GetJsonStr( iMsgId,iParam1,iParam2,iParam3,strParam1,strParam2,strParam3);
        UnityPlayer.UnitySendMessage(m_PlatformObject,m_MethodName,jsonString);//消息的接收对象，消息的接收函数名，消息字符串
    }
    public  static  String GetJsonStr(int iMsgId,int iParam1,int iParam2,int iParam3,String strParam1,String strParam2,String strParam3)
    {
        try {
            JSONObject jsonObject = new JSONObject();
            jsonObject.put("iMsgId",iMsgId);
            jsonObject.put("iPararm1",iParam1);
            jsonObject.put("iPararm2",iParam2);
            jsonObject.put("iPararm3",iParam3);
            jsonObject.put("strPararm1",strParam1);
            jsonObject.put("strPararm2",strParam2);
            jsonObject.put("strPararm3",strParam3);
            return jsonObject.toString();
        }catch (JSONException e)
        {
            Log.e(TAG, "JsonErr: "+ e.toString());
            return "";
        }
    }

    //Unity发送消息到平台
    public  static  String SendUnityMessageToPlatform(int iMsgId,int iParam1,int iParam2,int iParam3,int iParam4,String strParam1,String strParam2,String strParam3,String strParam4)
    {
       if(m_Activity==null)
       {
           Log.e(TAG, "my Activity is NULL");
       }
    }
}
