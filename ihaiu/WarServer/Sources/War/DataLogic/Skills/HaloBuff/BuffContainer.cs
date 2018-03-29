using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:12:39 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// Buff容器
    /// </summary>
    public class BuffContainer
    {
        /// <summary>
        /// 主人
        /// </summary>
        public UnitData unit;

        /// <summary>
        /// 现有的Buff字典
        /// </summary>
        public Dictionary<BuffType, List<Buff>> buffDict = new Dictionary<BuffType, List<Buff>>();

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (buffDict.Count == 0)
                return;

            List<BuffType> ids = new List<BuffType>(buffDict.Keys);
            ids.Sort();


            Buff buff;
            foreach (BuffType id in ids)
            {
                List<Buff> buffs = buffDict[id];

                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    buff = buffs[i];
                    buff.Update();
                }

                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    buff = buffs[i];

                    if (buff.willRemove)
                        RemoveBuff(buff);
                }
            }
        }

        /// <summary>
        /// 添加BUFF
        /// </summary>
        /// <param name="buff"></param>
        public void AddBuff(Buff buff)
        {
            List<Buff> buffs;

            if (!buffDict.TryGetValue(buff.buffType, out buffs))
            {
                buffs = new List<Buff>();
                buffDict.Add(buff.buffType, buffs);
            }

            //if (buffs.Count > 0)
            //{
            //    Buff[] rmlist = buffs.ToArray();
            //    foreach (Buff rmbuff in rmlist)
            //    {
            //        RemoveBuff(rmbuff);
            //    }
            //}

            buffs.Add(buff);
            buff.OnContainerAdd(this);

            if (buffs.Count == 1)
                buff.FirstAdd();
        }

        /// <summary>
        /// 移除BUFF
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(Buff buff)
        {
            buff.willRemove = false;
            List<Buff> buffs;

            if (!buffDict.TryGetValue(buff.buffType, out buffs))
                throw new System.Exception("NonRegisteredBuff");

            buffs.Remove(buff);

            buff.OnContainerRemove();

            if (buffs.Count == 0)
                buff.LastRemove();
        }

        /// <summary>
        /// 移除所有BUFF
        /// </summary>
        public void RemoveAll()
        {
            foreach (List<Buff> buffs in buffDict.Values)
            {
                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    Buff buff = buffs[i];
                    RemoveBuff(buff);
                }
            }

            buffDict.Clear();
        }

        /// <summary>
        /// 移除某种类型的BUFF
        /// </summary>
        /// <param name="buffType"></param>
        public void RemoveType(BuffType buffType)
        {
            List<Buff> buffs;
            if (buffDict.TryGetValue(buffType, out buffs))
            {
                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    Buff buff = buffs[i];
                    RemoveBuff(buff);
                }
            }
        }


        /// <summary>
        /// 移除BUFF,根据BUFF ID
        /// </summary>
        /// <param name="buffType"></param>
        /// <param name="id"></param>
        public void RemoveID(BuffType buffType, string id)
        {
            List<Buff> buffs;
            if (buffDict.TryGetValue(buffType, out buffs))
            {
                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    Buff buff = buffs[i];
                    if (buff.identifier == id)
                    {
                        RemoveBuff(buff);
                    }
                }
            }
        }

        /// <summary>
        /// 获取BUFF
        /// </summary>
        /// <param name="buffType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Buff GetBuff(BuffType buffType, string id)
        {
            List<Buff> buffs;
            if (buffDict.TryGetValue(buffType, out buffs))
            {
                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    Buff buff = buffs[i];
                    if (buff.identifier == id)
                    {
                        return buff;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取BUFF 列表
        /// </summary>
        /// <param name="buffType"></param>
        /// <returns></returns>
        public List<Buff> GetBuffList(BuffType buffType)
        {
            List<Buff> buffs;
            if (buffDict.TryGetValue(buffType, out buffs))
            {
                return buffs;
            }

            return null;
        }


    }
}
