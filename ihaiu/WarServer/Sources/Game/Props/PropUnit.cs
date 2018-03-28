using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/5/2017 4:10:45 PM
*  @Description:    单位属性容器
* ==============================================================================
*/
namespace Games
{
    //#if UNITY_EDITRO
    //    [Serializable]
    //#endif
    public partial class PropUnit
    {
        /** 基本属性 */
        public float[] bases                = new float[PropId.MAX];

        /** 附加具体值, 相对基础值 */
        public float[] basesadds            = new float[PropId.MAX];

        /** 附加百分比, 相对基础值 */
        public float[] basepers             = new float[PropId.MAX];

        /** 附加百分比，相对上限值，比如血量 */
        public float[] maxpers              = new float[PropId.MAX];

        /** 附加具体值 */
        public float[] adds                 = new float[PropId.MAX];

        /** 附加百分比 */
        public float[] pers                 = new float[PropId.MAX];

        /** 终于属性 */
        public float[] finals               = new float[PropId.MAX];

        /** 附加属性数据字典 */
        public Dictionary<int, PropAttachData> attachProps = new Dictionary<int, PropAttachData>();

        /** 初始化 */
        public void Init()
        {
            foreach (PropIdGroup item in PropId.NonrevertGroup)
            {
                if (finals[item.id] != 0)
                    finals[item.id] = bases[item.id];
            }
        }

        /** 获取属性值 */
        public float GetProp(int propTypeId)
        {
            return GetProp(PropId.PropTypeid2Id(propTypeId), PropId.PropTypeid2Type(propTypeId));
        }

        public float GetProp(int propId, PropType propType)
        {
            switch (propType)
            {
                case PropType.Base:
                    return bases[propId];
                case PropType.Basesadd:
                    return bases[propId];
                case PropType.Baseper:
                    return basepers[propId];
                case PropType.Maxper:
                    return maxpers[propId];
                case PropType.Add:
                    return adds[propId];
                case PropType.Per:
                    return pers[propId];
                case PropType.Final:
                    return finals[propId];
            }
            return 0;
        }


        /** 设置属性值 */
        public void SetProp(int propTypeId, float propVal)
        {
            SetProp(PropId.PropTypeid2Id(propTypeId), PropId.PropTypeid2Type(propTypeId), propVal);
        }

        public void SetProp(int propId, PropType propType, float propVal)
        {
            switch(propType)
            {
                case PropType.Base:
                    bases[propId] = propVal;
                    break;
                case PropType.Basesadd:
                    bases[propId] = propVal;
                    break;
                case PropType.Baseper:
                    basepers[propId] = propVal;
                    break;
                case PropType.Maxper:
                    maxpers[propId] = propVal;
                    break;
                case PropType.Add:
                    adds[propId] = propVal;
                    break;
                case PropType.Per:
                    pers[propId] = propVal;
                    break;
                case PropType.Final:
                    finals[propId] = propVal;
                    break;
            }
        }

        /** 减属性 */
        public void SubProp(Prop prop)
        {
            AddProp(prop.Id, prop.PropType, - prop.Val);
            //if(prop.GroupType == PropGroupType.State && GetProp(prop.Id, prop.PropType) < 0)
            //{
            //    Loger.LogErrorFormat("减属性的状态 小于0， 有可能减的次数 多与加的次数");
            //    SetProp(prop.Id, prop.PropType, 0);
            //}
        }

        /** 加属性值 */
        public void AddProp(Prop prop)
        {
            AddProp(prop.Id, prop.PropType, prop.Val);
        }

        public void AddProp(int propTypeId, float propVal)
        {
            AddProp(PropId.PropTypeid2Id(propTypeId), PropId.PropTypeid2Type(propTypeId), propVal);
        }

        public void AddProp(int propId, PropType propType, float propVal)
        {
            switch (propType)
            {
                case PropType.Base:
                    bases[propId] += propVal;
                    break;
                case PropType.Basesadd:
                    bases[propId] += propVal;
                    break;
                case PropType.Baseper:
                    basepers[propId] += propVal;
                    break;
                case PropType.Maxper:
                    maxpers[propId] += propVal;
                    break;
                case PropType.Add:
                    adds[propId] += propVal;
                    break;
                case PropType.Per:
                    pers[propId] += propVal;
                    break;
                case PropType.Final:
                    finals[propId] += propVal;
                    break;
            }
        }

        public void InitNonrevertFinals()
        {
            List<PropIdGroup> list = PropId.NonrevertGroup;
            int             count           = list.Count;
            PropIdGroup     group           = null;
            int             id              = 0;
            int             maxId           = 0;

            // final = (final + add) * (1 + per / 100)      +     base * baseper / 100      +     baseadd           + max.base * maxper
            for (int i = 0; i < count; i++)
            {
                group = list[i];
                id = group.id;
                maxId = group.maxId;

                finals[id] = bases[id];
            }
        }


        /** 计算 */
        public void Calculate()
        {
            CalculateNonrevert();
            CalculateRevert();
            CalculateState();
        }

        /** 计算--不可逆 */
        private void CalculateNonrevert()
        {
            List<PropIdGroup> list = PropId.NonrevertGroup;
            int             count           = list.Count;
            PropIdGroup     group           = null;
            int             id              = 0;
            int             maxId           = 0;

            // final = (final + add) * (1 + per / 100)      +     base * baseper / 100      +     baseadd           + max.base * maxper
            for (int i = 0; i < count; i++)
            {
                group   = list[i];
                id      = group.id;
                maxId   = group.maxId;

                finals[id] = (finals[id] + adds[id]) * (1 + pers[id] / 100f)                + bases[id] * basepers[id] / 100f + basesadds[id]          + bases[maxId] * maxpers[id];
                adds[id]        = 0;
                pers[id]        = 0;
                basepers[id]    = 0;
                basesadds[id]   = 0;
                maxpers[id]     = 0;
            }
        }

        /** 计算--可回滚 */
        private void CalculateRevert()
        {
            List<int> list = PropId.RevertGroup;
            int count   = list.Count;
            int id      = 0;


            // final = (base + add) * (1 + per / 100) + base * baseper / 100 + baseadd
            for(int i = 0; i < count; i ++)
            {
                id = list[i];
                finals[id] = (bases[id] + adds[id]) * (1 + pers[id] / 100f) + bases[id] * basepers[id] / 100f + basesadds[id];
            }
        }


        /** 计算--状态 */
        private void CalculateState()
        {
            List<int> list = PropId.StateGroup;
            int count = list.Count;
            int id = 0;


            // final = (final + add)
            for (int i = 0; i < count; i++)
            {
                id = list[i];
                finals[id] = finals[id] + adds[id];
                adds[id] = 0;
            }
        }


        /** 属性实体--添加附加 */
        public void AppProps(PropAttachData propAttachData, bool calculate = false)
        {
            if (propAttachData == null) return;

            if (attachProps.ContainsKey(propAttachData.uid))
            {
                RevokeProps(propAttachData.uid);
            }
            propAttachData.App(this);


            if (calculate)
            {
                Calculate();
            }
        }

        /** 属性实体--移除附加 */
        public void RevokeProps(PropAttachData attachPropData, bool calculate = false)
        {
            if (attachPropData == null) return;

            if (attachProps.ContainsKey(attachPropData.uid))
            {
                attachPropData.Revoke(this);
            }

            if (calculate)
            {
                Calculate();
            }
        }


        /** 属性实体--移除附加 */
        public void RevokeProps(int attachPropUid, bool calculate = false)
        {
            PropAttachData propAttachData;
            if (attachProps.TryGetValue(attachPropUid, out propAttachData))
            {
                RevokeProps(attachPropUid, calculate);
            }
        }


        /** 属性实体--清空 */
        public void RevokeAll()
        {
            List<PropAttachData> list = new List<PropAttachData>(attachProps.Values);
            foreach(PropAttachData item in list)
            {
                RevokeProps(item);
            }

            Calculate();
        }



        /** 清空 */
        public void Clear()
        {
            for (int i = 0; i < PropId.MAX; i++)
            {
                bases[i] = 0;
                basesadds[i] = 0;
                basepers[i] = 0;
                maxpers[i] = 0;
                adds[i] = 0;
                pers[i] = 0;
                finals[i] = 0;
            }

            attachProps.Clear();
        }






    }
}
