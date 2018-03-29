using Games.Wars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnityPathGraphSetting : MonoBehaviour
{
    [Header("单位类型")]
    // 单位类型--英雄
    public int unitHeroGraph        = 0;
    [Header("空间类型")]
    // 空间类型--地面
    public int spaceGroundGraph     = 0;
    // 空间类型--空中
    public int spaceFlyGraph        = 1;

    private static UnityPathGraphSetting    install;
    private static AstarPath                astarPath;

    private void Awake()
    {
        install = this;
        astarPath = GetComponent<AstarPath>();
    }



    public static int GetPathGraphMask(UnitType unitType, UnitSpaceType spaceType)
    {

        if (astarPath == null || astarPath.graphs.Length <= 1)
            return -1;

        if (unitType == UnitType.Hero)
        {
            return 1 << install.unitHeroGraph;
        }

        if (spaceType.IsGround())
        {
            return 1 << install.spaceGroundGraph;
        }

        if (spaceType.IsFly())
        {
            return  1 << install.spaceFlyGraph;
        }
        return -1;
    }

    public static int GetPathGraphMask(UnitType value)
    {
        if (astarPath == null || astarPath.graphs.Length <= 1)
            return -1;

        if(value == UnitType.Hero)
        {
            return 1 << install.unitHeroGraph;
        }

        return -1;
    }

    public static int GetPathGraphMask(UnitSpaceType value)
    {
        if (astarPath == null || astarPath.graphs.Length <= 1)
            return -1;

        if (value.IsGround())
        {
            return 1 << install.spaceGroundGraph;
        }

        if (value.IsFly())
        {
            return 1 << install.spaceFlyGraph;
        }
        return -1;
    }

}
