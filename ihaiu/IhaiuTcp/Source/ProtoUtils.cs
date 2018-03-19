using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/19/2018 9:38:41 AM
*  @Description:    
* ==============================================================================
*/

public static class ProtoUtils
{
    public static string ToStr(this byte[] bytes)
    {
        string str = "[";
        string gap = "";
        foreach (byte item in bytes)
        {
            str += gap + item;

            gap = ", ";
        }

        str += "]";
        return str;
    }
    public static string ToStr0x(this byte[] bytes)
    {
        string str = "[";
        string gap = "";
        foreach (byte item in bytes)
        {
            str += gap + item.ToString("X2");

            gap = ", ";
        }

        str += "]";
        return str;
    }


}