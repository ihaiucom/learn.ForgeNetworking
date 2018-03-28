using Assets.Scripts.Common;
using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/5/2017 8:21:43 PM
*  @Description:    属性附加数据
* ==============================================================================
*/
namespace Games
{
    public class PropAttachData : PooledClassObject
    {
        private static int UID = 10000;

        public int                  uid;
        public List<Prop>           props;
        public List<BuffProp>       buffProps;
        public int                  haloBuffId;

        public PropAttachData(int haloBuffId, List<BuffProp> buffProps)
        {
            Construction(0, haloBuffId, buffProps);
        }

        public PropAttachData(List<Prop> props)
        {
            Construction(0, props);
        }

        public PropAttachData(int uid, List<Prop> props)
        {
            Construction(uid, props);
        }

        private void Construction(int uid, List<Prop> props)
        {
            if(uid == 0)
            {
                uid = UID++;
            }

            this.uid = uid;
            this.props = props;
        }
        private void Construction(int uid, int haloBuffId, List<BuffProp> buffProps)
        {
            if(uid == 0)
            {
                uid = UID++;
            }

            this.uid = uid;
            this.haloBuffId = haloBuffId;
            this.buffProps = buffProps;
        }

        /** 将属性数据附加给单位 */
        public void App(PropUnit unit)
        {
            int revertCount = 0;
            if (buffProps != null && buffProps.Count > 0)
            {
                foreach (BuffProp buffProp in buffProps)
                {
                    if (float.IsNaN(buffProp.valValue))
                    {
                        Loger.LogErrorFormat("PropAttachData App  Val IsNaN  {0}", buffProp);
                    }
                    float refVal = unit.GetProp(buffProp.refAimId,buffProp.refPropType);
                    float refRatio = refVal * buffProp.ratio + buffProp.valValue;

                    Prop prop = Prop.Create(buffProp.aimId, buffProp.propType, refRatio);
                    if (props == null)
                    {
                        props = new List<Prop>();
                    }
                    props.Add(prop);
                }
            }
            foreach (Prop prop in props)
            {
                if(float.IsNaN(prop.Val))
                {
                    Loger.LogErrorFormat("PropAttachData App  Val IsNaN  {0}", prop);
                }
                unit.AddProp(prop);

                if(prop.EnableRevert)
                {
                    revertCount++;
                }
            }

            if (revertCount > 0)
            {
                unit.attachProps.Add(uid, this);
            }
        }

        /** 将属性数据从单位回滚 */
        public void Revoke(PropUnit unit)
        {
            if (!unit.attachProps.ContainsKey(uid))
                return;

            foreach (Prop prop in props)
            {
                if (prop.EnableRevert)
                {
                    unit.SubProp(prop);
                }
            }

            unit.attachProps.Remove(uid);
        }


        public override string ToString()
        {
            return string.Format("PropAttachData uid={0},    {1}", uid, props.ToStr<Prop>());
        }

    }
}
