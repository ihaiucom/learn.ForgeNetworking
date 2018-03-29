using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 7:06:42 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 光环容器
    /// </summary>
    public class HaoleContainer
    {
        public UnitData unit;
        public Dictionary<string, Halo> haoleDict = new Dictionary<string, Halo>();

        public void Add(Halo haole)
        {
            if (haoleDict.ContainsKey(haole.identifier))
            {
                Remove(haole.identifier);
            }

            haoleDict.Add(haole.identifier, haole);
            haole.sOnStopCallback += OnHaoleStop;
        }


        public void Remove(Halo haole)
        {
            Remove(haole.identifier);
        }

        public void Remove(string id)
        {
            Halo haole;
            if (haoleDict.TryGetValue(id, out haole))
            {

                haoleDict.Remove(id);
                haole.sOnStopCallback = null;
                haole.Stop();


            }

        }


        public void RemoveAll()
        {
            List<Halo> list = new List<Halo>();
            foreach (var kvp in haoleDict)
            {
                list.Add(kvp.Value);
            }

            for (int i = 0; i < list.Count; i++)
            {
                Remove(list[i]);
            }

            list.Clear();
            haoleDict.Clear();
        }

        public void OnHaoleStop(Halo haole)
        {
            if (haoleDict.ContainsKey(haole.identifier))
            {
                haoleDict.Remove(haole.identifier);
                haole.sOnStopCallback -= OnHaoleStop;
            }
        }

    }
}
