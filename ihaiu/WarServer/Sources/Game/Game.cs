using Games;
using System;
using System.Collections.Generic;
using WarServers;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/28/2018 5:35:20 PM
*  @Description:    
* ==============================================================================
*/

public class Game
{
    public static ProgramSetting setting;
    public static ConfigManager config;

    public static void Install(ProgramSetting setting)
    {
        Game.setting = setting;

        config = new ConfigManager();
        config.Load();
    }

}
