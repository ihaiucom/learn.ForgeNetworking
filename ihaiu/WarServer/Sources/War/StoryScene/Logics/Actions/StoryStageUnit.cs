using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情触发
    /// </summary>
    public class StoryActionUnit : StoryAction
    {
        private StoryActionUnitConfig actionConfig;
        public override void SetConfig(StoryActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (StoryActionUnitConfig)config;

            endType = StoryActionEndType.Call;
            OperUnit = room.clientOperationUnit.GetUnitAgent();
            OperData = room.clientOperationUnit.GetUnitData();
        }

        private UnitAgent OperUnit;
        private UnitData OperData;

        protected override void OnSkip()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.HeroPropSet:
                    {
                        // 修改属性
                        OperData.prop.SetProp(actionConfig.propId, actionConfig.propType, actionConfig.propValue);
                    }
                    break;
                case StoryActionType.HeroPropAdd:
                    {
                        // 添加属性
                        OperData.prop.AddProp(actionConfig.propId, actionConfig.propType, actionConfig.propValue);
                    }
                    break;
                case StoryActionType.HeroPosition:
                    {
                        // 位置设置
                        OperUnit.position = actionConfig.position;
                        OperUnit.rotation = actionConfig.angle;
                    }
                    break;
                case StoryActionType.HeroMove:
                    {
                        // 英雄移动
                        int length = actionConfig.movePath.Length;
                        OperUnit.position = actionConfig.movePath[length - 1];
                        if (length > 1)
                        {
                            Vector3 nearestDir = actionConfig.movePath[length - 1] - actionConfig.movePath[length - 2];
                            OperUnit.AnchorRotation.rotation = Quaternion.LookRotation(nearestDir);
                        }
                    }
                    break;
            }
            End();
        }

        // 更新物件
        protected override void OnStart()
        {

            switch (actionConfig.actionType)
            {
                case StoryActionType.HeroPlayAnimation:
                    {
                        // 英雄做动作
                        OperUnit.aniManager.ActionAni(actionConfig.animatorState);
                        End();
                    }
                    break;
                case StoryActionType.HeroPropSet:
                    {
                        // 修改属性
                        OperData.prop.SetProp(actionConfig.propId, actionConfig.propType, actionConfig.propValue);
                        End();
                    }
                    break;
                case StoryActionType.HeroPropAdd:
                    {
                        // 添加属性
                        OperData.prop.AddProp(actionConfig.propId, actionConfig.propType, actionConfig.propValue);
                        End();
                    }
                    break;
                case StoryActionType.HeroPosition:
                    {
                        // 位置设置
                        OperUnit.position = actionConfig.position;
                        OperUnit.rotation = actionConfig.angle;
                        End();
                    }
                    break;
                case StoryActionType.HeroMove:
                    {
                        // 英雄移动
                        OperUnit.aniManager.ActionAni(AnimatorState.Run);
                        OperUnit.rotation = Vector3.zero;
                        StoryStageMove.Move(OperUnit.transform, actionConfig.movePath, actionConfig.moveDuration, true, End);
                    }
                    break;
            }
        }
    }
}