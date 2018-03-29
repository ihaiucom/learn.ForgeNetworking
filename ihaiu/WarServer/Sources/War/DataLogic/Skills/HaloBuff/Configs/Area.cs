using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:34:20 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 区域
    /// </summary>
    public class Area
    {
        public Vector3 position;

        virtual public bool Contains(Vector3 pos)
        {
            return Contains(pos, 0);
        }

        virtual public bool Contains(Vector3 pos, float r)
        {
            return true;
        }

        virtual public bool Contains(float x, float z)
        {
            return Contains(x, z, 0);
        }

        virtual public bool Contains(float x, float z, float r)
        {
            return true;
        }
    }



    /// <summary>
    /// 圆形区域
    /// </summary>
    public class CircleArea : Area
    {
        public float radius = 5f;

        public CircleArea()
        {
        }

        public CircleArea(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }


        public override bool Contains(Vector3 pos, float r)
        {
            return Vector3.Distance(position, pos) < (radius + r);
        }

        private Vector3 _point = Vector3.zero;
        public override bool Contains(float x, float z, float r)
        {
            _point.x = x;
            _point.z = z;
            return Vector3.Distance(position, _point) < (radius + r);
        }

    }

    /// <summary>
    /// 矩形区域
    /// </summary>
    public class RectArea : Area
    {
        public Rect rect;

        public RectArea()
        {
        }

        public RectArea(Rect rect)
        {
            this.rect = rect;
        }

        public RectArea(Vector3 center, float width, float height)
        {
            rect = new Rect();
            rect.center = center.ToV2();
            rect.width = width;
            rect.height = height;
        }

        public RectArea(float x, float y, float width, float height)
        {
            rect = new Rect(x, y, width, height);
        }

        public override bool Contains(Vector3 pos, float r)
        {
            return Contains(pos.x, pos.z, r);
        }

        private Vector2 _p = Vector2.zero;
        public override bool Contains(float x, float z, float r)
        {
            _p.x = x;
            _p.y = z;
            return rect.Contains(_p);
        }
    }
}
