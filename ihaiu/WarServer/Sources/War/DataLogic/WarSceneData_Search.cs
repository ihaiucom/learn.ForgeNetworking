using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 1:54:49 PM
*  @Description:    战场数据
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class WarSceneData
    {

        /// <summary>
        /// 搜索圆形区域内的最近距离单位
        /// </summary>
        /// <param name="UnitData">主势力的单位</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public UnitData SearchMinDistanceUnit(UnitData unit, UnitType unitType, RelationType releationType, float radius, bool containUnitRadiuse = true, UnitSpaceType spaceType = UnitSpaceType.All)
        {

            List<UnitData> list = SearchUnitList(unit, unitType, releationType, spaceType, radius, containUnitRadiuse);
            //if(list.Count > 0)
            //{
            //    list.SortByDistance(unit.position);
            //    return list[0];
            //}
            //return null;
            if (list.Count > 0)
            {
                list.SortByDistance(unit.position);
                return list[list.Count - 1];
            }
            return null;
            //return SearchMinUnitWithList(unit, list);
        }

        #region 搜索圆环区域内的指定规则单位
        /// <summary>
        /// 搜索圆环区域内的指定规则单位
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="attackRule"></param>
        /// <param name="minRadius"></param>
        /// <param name="radius"></param>
        /// <param name="getCount"></param>
        /// <param name="count"></param>
        /// <param name="containUnitRadiuse"></param>
        /// <returns></returns>
        public List<UnitData> SearchMinDistanceUnit(UnitData unit, AttackRule attackRule, float minRadius, float radius, int getCount, out int count, bool containUnitRadiuse = false)
        {
            // 大圆范围
            List<UnitData> list = SearchUnitList(unit, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, radius, containUnitRadiuse);
            if (minRadius > 0)
            {
                // 小圆范围
                List<UnitData> list2 = SearchUnitList(unit, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, minRadius, containUnitRadiuse);
                // 去除小圆内的单位
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list2.Find(m => m.uid == list[i].uid) != null)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
            {
                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
            }

            // 提取规则
            if (getCount < list.Count && list.Count > 1)
            {
                SearchGetSkillRule(unit.position, attackRule, ref list);
                List<UnitData> result = list.GetRange(0, getCount);
                count = result.Count;
                return result;
            }
            count = list.Count;
            return list;
        }
        /// <summary>
        /// 提取规则
        /// </summary>
        /// <param name="attackRule"></param>
        /// <param name="list"></param>
        private static void SearchGetSkillRule(Vector3 pos, AttackRule attackRule, ref List<UnitData> list)
        {
            switch (attackRule.warSkillRule)
            {
                case WarSkillRule.HpHighToLower:
                    {
                        list.Sort(delegate (UnitData x, UnitData y)
                        {
                            return y.prop.Hp.CompareTo(x.prop.Hp);
                        });
                    }
                    break;
                case WarSkillRule.HpLowerToHigh:
                    {
                        list.Sort(delegate (UnitData x, UnitData y)
                        {
                            return x.prop.Hp.CompareTo(y.prop.Hp);
                        });
                    }
                    break;
                case WarSkillRule.HeroToBuild:
                    {
                        list.Sort((UnitData a, UnitData b) =>
                        {
                            if (a.unitType == UnitType.Hero && b.unitType == UnitType.Build)
                            {
                                return -1;
                            }
                            else if (a.unitType == UnitType.Build && b.unitType == UnitType.Hero)
                            {
                                return 1;
                            }
                            return 0;
                        });
                    }
                    break;
                case WarSkillRule.RandomRange:
                case WarSkillRule.FromNearToFar:
                    {
                        list.Sort((UnitData a, UnitData b) =>
                        {
                            float aa = Vector3.Distance(pos, a.position);
                            float bb = Vector3.Distance(pos, b.position);
                            if (aa > bb)
                            {
                                return 1;
                            }
                            else if (aa < bb)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    break;
                case WarSkillRule.FromFarToNear:
                    {
                        list.Sort((UnitData a, UnitData b) =>
                        {
                            float aa = Vector3.Distance(pos, a.position);
                            float bb = Vector3.Distance(pos, b.position);
                            if (aa > bb)
                            {
                                return -1;
                            }
                            else if (aa < bb)
                            {
                                return 1;
                            }
                            return 0;
                        });
                    }
                    break;
                case WarSkillRule.BossToSolider:
                    {
                        list.Sort((UnitData a, UnitData b) =>
                        {
                            if (a.soliderType == UnitSoliderType.Boss && b.soliderType != UnitSoliderType.Boss)
                            {
                                return -1;
                            }
                            else if (a.soliderType != UnitSoliderType.Boss && b.soliderType == UnitSoliderType.Boss)
                            {
                                return 1;
                            }
                            return 0;
                        });
                    }
                    break;
                case WarSkillRule.SoliderToBoss:
                    {
                        list.Sort((UnitData a, UnitData b) =>
                        {
                            if (a.soliderType == UnitSoliderType.Boss && b.soliderType != UnitSoliderType.Boss)
                            {
                                return 1;
                            }
                            else if (a.soliderType != UnitSoliderType.Boss && b.soliderType == UnitSoliderType.Boss)
                            {
                                return -1;
                            }
                            return 0;
                        });
                    }
                    break;

            }
        }
        #endregion

        #region 搜索目标圆形区域
        public List<UnitData> SearchMinDistanceUnit(int legionId, Vector3 point, AttackRule attackRule, float radius, int getCount, out int count, bool containUnitRadiuse = false)
        {
            List<UnitData> list = SearchUnit(legionId, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, point, radius, containUnitRadiuse);
            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
            {
                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
            }
            // 提取规则
            if (getCount < list.Count && list.Count > 1)
            {
                SearchGetSkillRule(point, attackRule, ref list);
                List<UnitData> result = list.GetRange(0, getCount);
                count = result.Count;
                return result;
            }
            count = list.Count;
            return list;
        }
        #endregion

        #region 搜索扇形范围内的目标
        public List<UnitData> SearchFanUnit(Quaternion rotation, Vector3 position, float targetFanAngle, int legionId, Vector3 point, AttackRule attackRule, float radius, int getCount, out int count, bool containUnitRadiuse = false)
        {
            // 先搜圆形内目标
            List<UnitData> list2 = SearchUnit(legionId, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, point, radius, containUnitRadiuse);
            List<UnitData> list = new List<UnitData>();
            for (int i = list2.Count - 1; i >= 0; i--)
            {
                float distance = Vector3.Distance(position, list2[i].position);//距离
                Vector3 norVec = rotation * Vector3.forward;
                Vector3 temVec = list2[i].position - position;
                float angle = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;
                if (distance < radius)
                {
                    if (angle <= targetFanAngle * 0.5f)
                    {
                        list.Add(list2[i]);
                    }
                }
            }
            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
            {
                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
            }
            // 提取规则
            if (getCount < list.Count && list.Count > 1)
            {
                SearchGetSkillRule(position, attackRule, ref list);
                List<UnitData> result = list.GetRange(0, getCount);
                count = result.Count;
                return result;
            }
            count = list.Count;
            return list;
        }
        #endregion

        #region 搜索矩形范围内的目标
        public List<UnitData> SearchFanUnit(Vector3 forward, Quaternion rotation, Vector3 position, float Distance, float targetMinRadius, float targetFanRadius, int legionId, Vector3 point, AttackRule attackRule, int getCount, out int count, bool containUnitRadiuse = false)
        {
            // 先搜圆形内目标，半径为矩形最大半径 * 2
            List<UnitData> list2 = SearchUnit(legionId, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, point, targetFanRadius * 2, containUnitRadiuse);
            List<UnitData> list = new List<UnitData>();
            for (int i = list2.Count - 1; i >= 0; i--)
            {
                Vector3 l = position.Clone().SetY(0) + (rotation * Vector3.left) * Distance;
                Vector3 le = l + (rotation * Vector3.forward) * Distance;
                Vector3 r = position.Clone().SetY(0) + (rotation * Vector3.right) * Distance;
                Vector3 re = r + (rotation * Vector3.forward) * Distance;
                Vector3 f = position.Clone().SetY(0) + (rotation * Vector3.forward) * Distance;

                Vector3 toOther = list2[i].position - position.Clone().SetY(0);
                if (Vector3.Dot(forward.Clone().SetY(0), toOther) >= 0)
                {
                    Vector3 e = list2[i].position;
                    Vector3 l0e = e - l; //e到l的向量
                    Vector3 r0e = e - r;//e到r的向量
                    Vector3 f0e = e - f;//e到f的向量

                    Vector3 h0l = position.Clone().SetY(0) - l;//h到l的向量
                    Vector3 h0r = position.Clone().SetY(0) - r;//h到r的向量
                    Vector3 h0f = position.Clone().SetY(0) - f;//h到f的向量
                    if (Vector3.Angle(l0e, h0l) <= 90 && Vector3.Angle(r0e, h0r) <= 90 && Vector3.Angle(f0e, h0f) <= 90)
                    {
                        list.Add(list2[i]);
                    }
                }
            }
            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
            {
                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
            }
            // 提取规则
            if (getCount < list.Count && list.Count > 1)
            {
                SearchGetSkillRule(position, attackRule, ref list);
                List<UnitData> result = list.GetRange(0, getCount);
                count = result.Count;
                return result;
            }
            count = list.Count;
            return list;
        }

        public List<UnitData> SearchFanUnit2(Vector3 point, float length, float x, int legionId, AttackRule attackRule, int getCount, out int count, bool containUnitRadiuse = false)
        {

            Rect rect = new Rect(x,length,0,0);
            // 先搜圆形内目标，半径为矩形最大半径 * 2
            List<UnitData> list = SearchFanUnit(legionId, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, point, rect, containUnitRadiuse);

            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
            {
                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
            }

            // 提取规则
            if (getCount < list.Count && list.Count > 1)
            {
                SearchGetSkillRule(point, attackRule, ref list);
                List<UnitData> result = list.GetRange(0, getCount);
                count = result.Count;
                return result;
            }
            count = list.Count;
            return list;
        }
        #endregion

        public UnitData SearchMinUnitWithList(UnitData unit, List<UnitData> list)
        {
            if (list.Count > 0)
            {
                list.SortByDistance(unit.position);
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 搜索圆形区域内的单位
        /// </summary>
        /// <param name="UnitData">主势力的单位</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public List<UnitData> SearchUnitList(UnitData unit, UnitType unitType, RelationType releationType, UnitSpaceType spaceType, float radius, bool containUnitRadiuse = true)
        {
            if (unit.unitId == 1001)
            {
                spaceType = UnitSpaceType.Ground;
            }

            if (unit.unitId == 3003)
            {
                spaceType = UnitSpaceType.Fly;
            }
            if (radius <= 0) radius = 10;
            return SearchUnit(unit.ascriptionLegionId, unitType, releationType, spaceType, unit.position, radius, containUnitRadiuse);
        }
        /// <summary>
        /// 依据规则搜索指定数量的单位，count为0表示所有单位全部返回
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="unitType"></param>
        /// <param name="releationType"></param>
        /// <param name="spaceType"></param>
        /// <param name="radius"></param>
        /// <param name="containUnitRadiuse"></param>
        /// <param name="warSkillRule"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UnitData> SearchUnitListInRule(UnitData unit, UnitType unitType, RelationType releationType, UnitSpaceType spaceType, float radius, Vector3 point, out int _count, bool containUnitRadiuse = true, WarSkillRule warSkillRule = WarSkillRule.RandomRange, int count = 0)
        {
            List<UnitData> list = SearchUnit(unit.ascriptionLegionId, unitType, releationType, spaceType, point, radius, containUnitRadiuse);
            if (count > 0 && count < list.Count)
            {
                switch (warSkillRule)
                {
                    case WarSkillRule.HpHighToLower:
                        {
                            list.Sort(delegate (UnitData x, UnitData y)
                            {
                                return y.prop.Hp.CompareTo(x.prop.Hp);
                            });
                        }
                        break;
                    case WarSkillRule.HpLowerToHigh:
                        {
                            list.Sort(delegate (UnitData x, UnitData y)
                            {
                                return x.prop.Hp.CompareTo(y.prop.Hp);
                            });
                        }
                        break;
                    default:
                        {
                            list.Sort((UnitData a, UnitData b) =>
                            {
                                float aa = Vector3.Distance(unit.position, a.position);
                                float bb = Vector3.Distance(unit.position, b.position);
                                if (aa > bb)
                                {
                                    return 1;
                                }
                                else if (aa < bb)
                                {
                                    return -1;
                                }
                                return 0;
                            });

                        }
                        break;
                }
                List < UnitData > result = list.GetRange(0, count);
                _count = result.Count;
                return result;
            }
            _count = list.Count;
            return list;
        }

        /// <summary>
        /// 搜索圆形区域内的单位
        /// </summary>
        /// <param name="legionId">主势力ID</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public List<UnitData> SearchUnit(int legionId, UnitType unitType, RelationType releationType, UnitSpaceType spaceType, Vector3 point, float radius, bool containUnitRadiuse = true)
        {
            List<UnitData> list = new List<UnitData>();
            int count = unitList.Count;
            UnitData unit;
            for (int i = 0; i < count; i++)
            {
                unit = unitList[i];
                if (!unitType.UContain(unit.unitType)) continue;
                if (!releationType.RContain(unit.GetRelationType(legionId))) continue;
                if (spaceType == UnitSpaceType.Fly && unit.spaceType != UnitSpaceType.Fly) continue;
                if (spaceType == UnitSpaceType.Ground && unit.spaceType != UnitSpaceType.Ground) continue;
                if (!unit.isLive) continue;
                if (!unit.isInScene) continue;
                if (unit.prop.Hp <= 0) continue;
                if (CheckCircle(unit, point, radius, containUnitRadiuse))
                {
                    list.Add(unit);
                }
            }

            return list;
        }

        // 搜索矩形范围内的单位
        public List<UnitData> SearchFanUnit(int legionId, UnitType unitType, RelationType releationType, UnitSpaceType spaceType, Vector3 point, Rect rect, bool containUnitRadiuse = true)
        {
            List<UnitData> list = new List<UnitData>();
            int count = unitList.Count;
            UnitData unit;
            for (int i = 0; i < count; i++)
            {
                unit = unitList[i];
                if (!unitType.UContain(unit.unitType)) continue;
                if (!releationType.RContain(unit.GetRelationType(legionId))) continue;
                if (spaceType == UnitSpaceType.Fly && unit.spaceType != UnitSpaceType.Fly) continue;
                if (spaceType == UnitSpaceType.Ground && unit.spaceType != UnitSpaceType.Ground) continue;
                if (!unit.isLive) continue;
                if (!unit.isInScene) continue;
                if (unit.prop.Hp <= 0) continue;
                if (CheckFanUnit(unit, point, rect, containUnitRadiuse))
                {
                    list.Add(unit);
                }
            }

            return list;
        }


        /// <summary>
        /// 技能搜索圆形区域内的单位
        /// </summary>
        /// <param name="legionId">主势力ID</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="buildType">buildType</param>
        /// <param name="soliderType">soliderType</param>
        /// <param name="spaceType">spaceType</param>
        /// <param name="spaceType">spaceType</param
        /// <param name="professionType">professionType</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public UnitData SearchUnit(SkillConfig skillConfig, UnitData unitData, bool containUnitRadiuse = true)
        {
            float radarRadius = unitData.prop.RadarRadius;
            List<UnitData> list = SearchUnitList(unitData.ascriptionLegionId, skillConfig.targetRelation, skillConfig.targetUnitType, skillConfig.targetBuildType, skillConfig.targetSoliderType, skillConfig.targetSpaceType, skillConfig.targetProfessionType, unitData.position, radarRadius, containUnitRadiuse);
            int count = list.Count;
            float maxScore = 0;
            UnitData maxEnumy = null;
            UnitData enumy;
            float scoreDistance = 0;
            float scoreHatred = 0;
            float scoreType = 0;
            for (int i = 0; i < list.Count; i++)
            {
                enumy = list[i];
                scoreDistance = unitData.Distance(enumy);
                scoreDistance = Mathf.Clamp(1 - scoreDistance / radarRadius, 0, 2) * skillConfig.aiConfigDistance.GetVal(enumy);
                scoreType = skillConfig.aiConfigType.GetVal(enumy);
                scoreHatred = unitData.GetHatred(enumy.uid);

                scoreHatred *= skillConfig.aiConfigWeight.weightHatred;
                scoreType *= skillConfig.aiConfigWeight.weightType;
                scoreDistance *= skillConfig.aiConfigWeight.weightDistance;


                enumy.aiScore = scoreHatred + scoreType + scoreDistance;
                if (enumy.aiScore > maxScore)
                {
                    maxScore = enumy.aiScore;
                    maxEnumy = enumy;
                }
            }
            return maxEnumy;
        }


        /// <summary>
        /// 搜索圆形区域内的单位列表
        /// </summary>
        /// <param name="legionId">主势力ID</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="buildType">buildType</param>
        /// <param name="soliderType">soliderType</param>
        /// <param name="spaceType">spaceType</param>
        /// <param name="spaceType">spaceType</param
        /// <param name="professionType">professionType</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public List<UnitData> SearchUnitList(UnitData unit, RelationType releationType, UnitType unitType, UnitBuildType buildType, UnitSoliderType soliderType, UnitSpaceType spaceType, UnitProfessionType professionType, float radius, bool containUnitRadiuse = true)
        {
            return SearchUnitList(unit.ascriptionLegionId, releationType, unitType, buildType, soliderType, spaceType, professionType, unit.position, radius, containUnitRadiuse);
        }

        /// <summary>
        /// 搜索圆形区域内的单位列表
        /// </summary>
        /// <param name="legionId">主势力ID</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="buildType">buildType</param>
        /// <param name="soliderType">soliderType</param>
        /// <param name="spaceType">spaceType</param>
        /// <param name="spaceType">spaceType</param
        /// <param name="professionType">professionType</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public List<UnitData> SearchUnitList(int legionId, RelationType releationType, UnitType unitType, UnitBuildType buildType, UnitSoliderType soliderType, UnitSpaceType spaceType, UnitProfessionType professionType, Vector3 point, float radius, bool containUnitRadiuse = true)
        {
            List<UnitData> list = new List<UnitData>();
            int count = unitList.Count;
            UnitData unit;
            for (int i = 0; i < count; i++)
            {
                unit = unitList[i];
                if (!unitType.UContain(unit.unitType)) continue;
                if (!spaceType.UContain(unit.spaceType)) continue;
                if (unit.buildType != UnitBuildType.None && !buildType.UContain(unit.buildType)) continue;
                if (unit.soliderType != UnitSoliderType.None && !soliderType.UContain(unit.soliderType)) continue;
                if (unit.professionType != UnitProfessionType.None && !professionType.UContain(unit.professionType)) continue;
                if (!releationType.RContain(unit.GetRelationType(legionId))) continue;
                if (!unit.isLive) continue;
                if (!unit.isInScene) continue;

                if (unit.prop.Hp <= 0) continue;
                if (CheckCircle(unit, point, radius, containUnitRadiuse))
                {
                    list.Add(unit);
                }
            }

            return list;
        }



        /// <summary>
        /// 检测单位是否在圆形区域内
        /// </summary>
        /// <param name="unit">单位</param>
        /// <param name="point">圆心</param>
        /// <param name="radius">圆半径</param>
        /// <param name="containUnitRadiuse">是否包含单位半径 true需要单位一半以上身体在区域内</param>
        /// <returns></returns>
        public bool CheckCircle(UnitData unit, Vector3 point, float radius, bool containUnitRadiuse = true)
        {
            bool result = false;
            float distance = Vector3.Distance(point.Clone().SetY(0), unit.position.Clone().SetY(0));
            if (containUnitRadiuse)
            {
                distance -= unit.unitRadius;
                if (distance < 0)
                    distance = 0;
            }

            if (distance <= radius)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检测单位是否在矩形区域内
        /// </summary>
        /// <param name="unit">单位</param>
        /// <param name="point">矩形中心</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="containUnitRadiuse"></param>
        /// <returns></returns>
        public bool CheckFanUnit(UnitData unit, Vector3 point, Rect rect, bool containUnitRadiuse = false)
        {
            float width = rect.width * 0.5F;
            float height = rect.height * 0.5F;
            Vector2 p1 = new Vector2(point.x - width,point.z + height);
            Vector2 p2 = new Vector2(point.x - width,point.z - height);
            Vector2 p3 = new Vector2(point.x + width,point.z - height);
            Vector2 p4 = new Vector2(point.x + width,point.z + height);
            Vector2 p = new Vector2(unit.position.x,unit.position.z);
            float dis = 0;
            if (containUnitRadiuse)
            {
                dis = unit.unitRadius < 0 ? 0 : unit.unitRadius;
            }
            return GetCross(p1, p2, p) * GetCross(p3, p4, p) >= dis && GetCross(p2, p3, p) * GetCross(p4, p1, p) >= dis;


        }

        float GetCross(Vector2 p1, Vector2 p2, Vector2 p)
        {
            return (p2.x - p1.x) * (p.y - p1.y) - (p.x - p1.x) * (p2.y - p1.y);
        }

        /// <summary>
        /// 搜索全图的单位
        /// </summary>
        /// <param name="legionId">主势力ID</param>
        /// <param name="unitType">搜索的单位类型</param>
        /// <param name="releationType">搜索的关系类型</param>
        /// <returns></returns>
        public List<UnitData> SearchUnit(int legionId, UnitType unitType, RelationType releationType)
        {
            List<UnitData> list = new List<UnitData>();
            int count = unitList.Count;
            UnitData unit;
            for (int i = 0; i < count; i++)
            {
                unit = unitList[i];
                if (!unitType.UContain(unit.unitType)) continue;
                if (!releationType.RContain(unit.GetRelationType(legionId))) continue;
                if (!unit.isLive) continue;
                if (!unit.isInScene) continue;
                list.Add(unit);
            }

            return list;
        }
    }
}
