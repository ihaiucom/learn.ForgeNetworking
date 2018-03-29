using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情行为实现 -- 气泡文字
    /// </summary>
    public class StoryActionBubble : StoryAction
    {
        private StoryActionBubbleConfig actionConfig;
        public override void SetConfig(StoryActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (StoryActionBubbleConfig)config;

            endType = StoryActionEndType.DurationTime;
            durationTime = actionConfig.duration;
        }

        protected override void OnSkip()
        {
            End();
        }

        protected override void OnStart()
        {
            switch (actionConfig.positionType)
            {
                case StoryBubblePositionType.World:
                    {
                        WarUI.Instance.OnAddTextTip(actionConfig.duration, actionConfig.positon, null, actionConfig.msg, null);
                    }
                    break;
                case StoryBubblePositionType.Obj:
                    {
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.objName));
                        Vector3 pos = actionConfig.positon;
                        if (storySceneUnit != null)
                        {
                            pos += storySceneUnit.tForm.position;
                        }
                        WarUI.Instance.OnAddTextTip(actionConfig.duration, pos, null, actionConfig.msg, storySceneUnit.tForm);
                    }
                    break;
                case StoryBubblePositionType.Unit:
                    {
                        UnitData opUnit = null;
                        if (actionConfig.unitId <= 0)
                        {
                            opUnit = room.clientOperationUnit.GetUnitData();
                        }
                        else
                        {
                            opUnit = room.sceneData.GetUnit(actionConfig.unitId);
                        }
                        WarUI.Instance.OnAddTextTip(actionConfig.duration, actionConfig.positon, opUnit, actionConfig.msg, null);
                    }
                    break;
            }
        }

    }
}