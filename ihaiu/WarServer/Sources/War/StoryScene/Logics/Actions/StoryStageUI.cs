using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情触发
    /// </summary>
    public class StoryActionUI : StoryAction
    {
        private StoryActionMaskConfig actionConfig;
        public override void SetConfig(StoryActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (StoryActionMaskConfig)config;

            endType = StoryActionEndType.NextFrame;
        }

        protected override void OnSkip()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.UIMask:
                    {
                        // 遮罩开启和关闭
                        WarUI.Instance.OnBlackMask(actionConfig.isOpen);
                    }
                    break;
                case StoryActionType.UIShowHideComplete:
                    {
                        WarUI.Instance.OnShowHideUI(actionConfig.isOpen);
                    }
                    break;
            }
            End();
        }

        // 更新UI
        protected override void OnStart()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.UIMask:
                    {
                        // 遮罩开启和关闭
                        WarUI.Instance.OnBlackMask(actionConfig.isOpen);
                    }
                    break;
                case StoryActionType.UIShowHideComplete:
                    {
                        WarUI.Instance.OnShowHideUI(actionConfig.isOpen);
                    }
                    break;
            }
        }
    }
}