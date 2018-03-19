using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/19/2018 1:36:06 PM
*  @Description:    
* ==============================================================================
*/
public class TestPost
{
    public string urlRoot = "http://172.16.52.101";
    public string urlGate
    {
        get
        {
            return urlRoot + "/game_operating_platform/index.php";
        }
    }

    public string urlList
    {
        get
        {
            return urlRoot + "/list.html";
        }
    }

    public void Run()
    {
        string url = urlGate;
        //参数p
        IDictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("p", Http.UrlEncode("aaa"));
        //http请求
        System.Net.HttpWebResponse res = Http.CreatePostHttpResponse(url, parameters, 3000, null, null, null);
        if (res == null)
        {
            Loger.LogError("RequestFailed.aspx?result=出错了,可能是由于您的网络环境差、不稳定或安全软件禁止访问网络，您可在网络好时或关闭安全软件在重新访问网络。");
        }
        else
        {
            //获取返回数据转为字符串
            string mes = Http.GetResponseString(res);
            Loger.Log(mes);
        }
    }
}