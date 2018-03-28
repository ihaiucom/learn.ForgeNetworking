using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/5/2017 5:41:38 PM
*  @Description:    属性ID 组
* ==============================================================================
*/
namespace Games
{
    public class PropIdGroup
    {
        public int  id;
        public int  maxId;

        public PropIdGroup(int id, int maxId)
        {
            this.id         = id;
            this.maxId      = maxId;
        }
    }

    public partial class PropId
    {
        // 最大值 
        public static int TYPE_ID_MASK = 100;

        /** 获取propId, 根据 propTypeId */
        public static int PropTypeid2Id(int propTypeId)
        {
            return propTypeId % TYPE_ID_MASK;
        }


        /** 获取propType, 根据 propTypeId */
        public static PropType PropTypeid2Type(int propTypeId)
        {
            return (PropType) Mathf.FloorToInt(propTypeId / TYPE_ID_MASK);
        }

        /** 获取propTypeId, 根据 propId和propType */
        public static int GetTypeId(int propId, PropType propType)
        {
            return (int)propType * TYPE_ID_MASK + propId;
        }




        /** 不可逆, HP、Energy */
        private static List<PropIdGroup> nonrevertGroup ;

        /** 可回滚 */
        private static List<int> revertGroup    ;

        /** 状态 */
        private static List<int> stateGroup     ;


        /** 不可逆, HP、Energy */
        public static List<PropIdGroup> NonrevertGroup
        {
            get
            {
                if(nonrevertGroup == null)
                {

                    List<PropIdGroup> list = new List<PropIdGroup>();
                    list.Add( new PropIdGroup(Hp        , HpMax         ) );
                    list.Add( new PropIdGroup(Energy    , EnergyMax     ) );
                    nonrevertGroup = list;
                }
                return nonrevertGroup;
            }
        }


        /** 可回滚 */
        public static List<int> RevertGroup
        {
            get
            {
                if(revertGroup == null)
                {

                    List<int> list = new List<int>();
                    list.Add(EnergyMax);
                    list.Add(HpMax);
                    list.Add(Shield);
                    list.Add(PhysicalAttack);
                    list.Add(MagicAttack);
                    list.Add(PhysicalDefence);
                    list.Add(MagicDefence);
                    list.Add(RadarRadius);
                    list.Add(AttackRadius);
                    list.Add(AttackSpeed);
                    list.Add(MovementSpeed);
                    list.Add(Damage);
                    list.Add(HpRecover);
					list.Add(EnergyRecover);
					list.Add(AttackInterval);

                    revertGroup = list;
                }
                return revertGroup;
            }
        }



        /** 状态 */
        public static List<int> StateGroup
        {
            get
            {
                if (stateGroup == null)
                {

                    List<int> list = new List<int>();
                    list.Add(StateBurn);
                    list.Add(StateFreezed);
                    list.Add(StateSilence);
                    list.Add(StateVertigo);
                    list.Add(StatePosion);
                    stateGroup = list;
                }
                return stateGroup;
            }
        }


    }
}
