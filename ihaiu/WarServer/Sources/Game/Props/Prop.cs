using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 5:27:59 PM
*  @Description:    属性
* ==============================================================================
*/
namespace Games
{

    [Serializable]
    public class Prop
    {
        [SerializeField]
        public PropType type;
        [SerializeField]
        public int      id;
        [SerializeField]
        public int      typeId;
        [SerializeField]
        public float    val;
        [SerializeField]
        public  int     valid;


        public Prop()
        {

        }

        public Prop(int propTypeId, float propVal)
        {
            typeId  = propTypeId;
            id      = PropId.PropTypeid2Id(typeId);
            type    = PropId.PropTypeid2Type(typeId);
            val     = propVal;
        }


        public Prop(int propId, PropType propType, float propVal)
        {
            typeId  = PropId.GetTypeId(propId, propType);
            id      = propId;
            type    = propType;
            val     = propVal;
        }

        public Prop(string proField, float propVal)
        {
            id = PropField.GetPropId(proField);
            type = PropType.Add;
            typeId = PropId.GetTypeId(id, type);
            val = propVal;
        }


        [Newtonsoft.Json.JsonIgnore]
        public PropConfig Config
        {
            get
            {
                return Game.config.prop.GetConfig(id);
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                typeId = PropId.GetTypeId(id, type);
            }
        }


        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public PropType PropType
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                typeId = PropId.GetTypeId(id, type);
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public int PropTypeId
        {
            get
            {
                return typeId;
            }


            set
            {
                typeId = value;

                id      = PropId.PropTypeid2Id(typeId);
                type    = PropId.PropTypeid2Type(typeId);
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public float Val
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }

        public  int ValId
        {
            set
            {
                valid = value;
            }
            get
            {
                return valid;
            }
        }
        public  int valLvset
        {
            set
            {
                if (ValId > 10000)
                {
                    val = Game.config.skillValue.GetConfigs(ValId, value);
                }
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public PropGroupType GroupType
        {
            get
            {
                return Config.groupType;
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public bool EnableRevert
        {
            get
            {
                return GroupType != PropGroupType.Nonrevert;
            }
        }



        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public string Field
        {
            get
            {
                return Config.field;
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public string Name
        {
            get
            {
                return Config.cnName;
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public string Tip
        {
            get
            {
                return Config.tip;
            }
        }

        [ignoreAttibute]
        [Newtonsoft.Json.JsonIgnore]
        public string Icon
        {
            get
            {
                return Config.icon;
            }
        }




        /** 创建，用PropId */
        public static Prop Create(int propTypeId, float propVal)
        {
            return new Prop(propTypeId, propVal);
        }


        public static Prop Create(int propId, PropType propType, float propVal)
        {
            return new Prop(propId, propType, propVal);
        }


        /** 创建，用PropField */
        public static Prop Create(string propField, float propVal)
        {
            return new Prop(PropField.GetPropId(propField), propVal);
        }


        public override string ToString()
        {
            return string.Format("[Prop Id={0}, PropType={1}, Val={2}, Name={3}]", Id, PropType, Val, Name);
        }


    }
}
