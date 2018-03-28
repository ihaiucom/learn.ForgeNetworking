using com.ihaiu;
using System;
using System.Collections.Generic;
namespace Games
{
    [ConfigCsv("Config/AIHero", true, false)]
    public class AIHeroConfigReader : ConfigReader<AIHeroConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            AIHeroConfig config = new AIHeroConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.timeCD = csv.GetInt32(GetHeadIndex("time")) / 1000.0F;
            config.defaultState = GetHeroAiEnum(csv.GetString(GetHeadIndex("default_value")));
            config.distanceList = new Dictionary<int, int>();
            for (int i = 1; i < 7; i++)
            {
                config.distanceList.Add(i, csv.GetInt32(GetHeadIndex("distance" + i)));
            }
            config.eventDic = new Dictionary<int, Dictionary<int, Wars.UnitHeroAI.HeroAiEnum>>();
            for (int i = 1; i < 7; i++)
            {
                string disData = csv.GetString(GetHeadIndex("distance" + i + "_data"));
                config.eventDic.Add(i, GetDic(disData));
            }

            configs.Add(config.id, config);
        }

        public AIHeroConfig AIGetConfig(int aiId)
        {
            AIHeroConfig config = GetConfig(aiId);
            if (config == null)
            {
                Loger.LogErrorFormat("不存在配置 AIHeroConfig Id={0}", aiId);
            }
            return config;
        }

        /// <summary>
        /// 获取事件列表
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        Dictionary<int, Wars.UnitHeroAI.HeroAiEnum> GetDic(string src)
        {
            Dictionary<int,Wars.UnitHeroAI.HeroAiEnum> result = new Dictionary<int,Wars.UnitHeroAI.HeroAiEnum>();
            src = src.Replace(';', ',');
            string[] strArr = src.Split(',');
            int index = 0;
            int length = strArr.Length;
            if (length % 2 != 0) return null;
            int numLength = length / 2;
            Wars.UnitHeroAI.HeroAiEnum[] enumList = new Wars.UnitHeroAI.HeroAiEnum[numLength];
            int enumListIndex = 0;
            int[] floatList = new int[numLength];
            int floatListIndex = 0;
            int AllCount = 0;
            for (int i = 0; i < length; i++)
            {
                if (index % 2 == 0)
                {
                    enumList[enumListIndex] = GetHeroAiEnum(strArr[i]);
                    enumListIndex++;
                }
                else
                {
                    AllCount += strArr[i].ToInt32();
                    floatList[floatListIndex] = AllCount;
                    floatListIndex++;
                }
                index++;
            }
            for (int i = 0; i < numLength; i++)
            {
                result.Add(floatList[i], enumList[i]);
            }

            return result;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        Wars.UnitHeroAI.HeroAiEnum GetHeroAiEnum(string str)
        {
            Wars.UnitHeroAI.HeroAiEnum result = Wars.UnitHeroAI.HeroAiEnum.Idle;
            string sttr = str.ToLower();
            switch (sttr)
            {
                case "move":
                    result = Wars.UnitHeroAI.HeroAiEnum.Move;
                    break;
                case "movefoward":
                    result = Wars.UnitHeroAI.HeroAiEnum.MoveFoward;
                    break;
                case "moveback":
                    result = Wars.UnitHeroAI.HeroAiEnum.MoveBack;
                    break;
                case "moveright":
                    result = Wars.UnitHeroAI.HeroAiEnum.MoveRight;
                    break;
                case "moveleft":
                    result = Wars.UnitHeroAI.HeroAiEnum.MoveLeft;
                    break;
                case "attack":
                    result = Wars.UnitHeroAI.HeroAiEnum.Attack;
                    break;
                case "attackfoward":
                    result = Wars.UnitHeroAI.HeroAiEnum.AttackFoward;
                    break;
                case "attackback":
                    result = Wars.UnitHeroAI.HeroAiEnum.AttackBack;
                    break;
                case "attackright":
                    result = Wars.UnitHeroAI.HeroAiEnum.AttackRight;
                    break;
                case "attackleft":
                    result = Wars.UnitHeroAI.HeroAiEnum.AttackLeft;
                    break;
                case "skill1":
                    result = Wars.UnitHeroAI.HeroAiEnum.Skill1;
                    break;
                case "skill2":
                    result = Wars.UnitHeroAI.HeroAiEnum.Skill2;
                    break;
                case "skill3":
                    result = Wars.UnitHeroAI.HeroAiEnum.Skill3;
                    break;
                case "skill4":
                    result = Wars.UnitHeroAI.HeroAiEnum.Skill4;
                    break;
            }
            return result;
        }
    }
}
