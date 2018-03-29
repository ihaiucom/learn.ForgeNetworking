using Games.Wars;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ShowActionTarget : MonoBehaviour
{
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //DrawFan(pos + transform.position, dis, angle, dis2);
        switch (targetLocation)
        {
            case TargetLocation.LinearWave:
                {
                    if (triggerUse != null)
                    {
                        SkillTriggerUseLinearWave triggerUseLine = (SkillTriggerUseLinearWave)(triggerUse.skillTriggerLocation);
                        if (triggerUseLine.targetLocationWidth > 0)
                        {
                            Handles.DrawAAPolyLine(triggerUseLine.targetLocationWidth, new Vector3[] { tform.position, tform.position + tform.forward * triggerUseLine.targetFanRadius });
                        }
                    }
                }
                break;
            case TargetLocation.FanWave:
                {
                    SkillTriggerUseFanWave triggerUseLine = (SkillTriggerUseFanWave)(triggerUse.skillTriggerLocation);
                    DrawFan(tform, triggerUseLine.targetFanRadius, triggerUseLine.targetFanAngle, triggerUseLine.targetMinRadius);
                }
                break;
            case TargetLocation.CircularShockwave:
                {
                    SkillTriggerUseCircularShockwave triggerUseLine = (SkillTriggerUseCircularShockwave)(triggerUse.skillTriggerLocation);
                    Handles.DrawWireArc(tform.position, Vector3.up * triggerUseLine.targetFanRadius, tform.forward * triggerUseLine.targetFanRadius, 360, triggerUseLine.targetFanRadius);
                }
                break;
            case TargetLocation.TargetCircleArea:
                {
                    SkillTriggerUseTargetCircleArea triggerUseLine = (SkillTriggerUseTargetCircleArea)(triggerUse.skillTriggerLocation);
                    Vector3 center = tform.position + tform.forward * triggerUseLine.targetCircleAreaDis;
                    Vector3 from = Vector3.forward * triggerUseLine.targetFanRadius;
                    Handles.DrawWireArc(center, Vector3.up * triggerUseLine.targetFanRadius, from, 360, triggerUseLine.targetFanRadius);
                }
                break;
        }
    }
    public Transform tform;
    TargetLocation targetLocation;
    SkillTriggerUse triggerUse;
    public void SetFan(SkillTriggerUse skillTrigger)
    {
        this.targetLocation = skillTrigger.targetLocation;
        this.triggerUse = skillTrigger;
    }

    #region 扇形
    /// <summary>
    /// 绘制扇形区域
    /// </summary>
    /// <param name="pos">起点</param>
    /// <param name="dis">半径</param>
    /// <param name="angle">角度</param>
    /// <param name="dis2">内半径</param>
    void DrawFan(Transform tform, float dis, float angle, float dis2 = 0)
    {
        float mindis = dis;
        float maxdis = dis2;
        if (dis2 < dis)
        {
            mindis = dis2;
            maxdis = dis;
        }

        drawfans(tform, maxdis, angle);
        drawfans(tform, mindis, angle, true);
    }

    private static void drawfans(Transform tform, float dis, float angle, bool bMin = false)
    {
        if (!bMin)
            Handles.Label(tform.position + tform.forward * dis / 2, " 中心线 ");
        Handles.color = Color.gray;
        Handles.DrawLine(tform.position, tform.position + tform.forward * dis);
        Vector3 p1 = Quaternion.Euler(0,-angle / 2,0) * tform.forward * dis + tform.position;
        Vector3 p2 = Quaternion.Euler(0,angle / 2,0) * tform.forward * dis + tform.position;
        if (bMin)
        {
            Handles.color = Color.white;
        }
        else
        {
            Handles.color = Color.green;
        }
        Handles.DrawLine(tform.position, p1);
        if (bMin)
        {
            Handles.color = Color.white;
        }
        else
        {
            Handles.color = Color.blue;
        }
        Handles.DrawLine(tform.position, p2);
        if (bMin)
        {
            Handles.color = Color.white;
        }
        else
        {
            Handles.color = Color.red;
        }
        Handles.DrawWireArc(tform.position, Vector3.up * dis, p1 - tform.position, angle, dis);
    }
    #endregion
    
#endif
}
