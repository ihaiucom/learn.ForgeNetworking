using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/21/2017 6:46:53 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarDefaultColor
    {

        /** 白色 */
        public Color white = Color.white;
        /** 红色 */
        public Color red1 = new Color(255 / 255f, 0 / 255f, 0 / 255f, 1);
        /** 浅红色 */
        public Color red2 = new Color(255 / 255f, 0 / 255f, 127 / 255f, 1);
        /** 浅浅红色 */
        public Color red3 = new Color(255 / 255f, 127 / 255f, 127 / 255f, 1);


        /** 绿色 */
        public Color green1 = new Color(0 / 255f, 255 / 255f, 0 / 255f, 1);
        /** 浅绿色 */
        public Color green2 = new Color(141 / 255f, 255 / 255f, 137 / 255f, 1);

        /** 蓝色 */
        public Color blue1 = new Color(0 / 255f, 0 / 255f, 150 / 255f, 1);
        /** 浅蓝色 */
        public Color blue2 = new Color(0 / 255f, 234 / 255f, 255f / 255f, 1);
        /** 浅浅蓝色 */
        public Color blue3 = new Color(65 / 255f, 125 / 255f, 144 / 255f, 1);


        /** 紫红色 */
        public Color magenta = new Color(202 / 255f, 0 / 255f, 255 / 255f, 1);
        /** 黄色色 */
        public Color yellow = Color.yellow;
        /** 橙色 */
        public Color orange = new Color(255 / 255f, 127 / 255f, 0f / 255f, 1);

        private List<Color> _colorList;
        public List<Color> colorList
        {
            get
            {
                if (_colorList == null)
                {
                    _colorList = new List<Color>();

                    _colorList.Add(white);
                    _colorList.Add(red1);
                    _colorList.Add(green1);
                    _colorList.Add(blue1);

                    _colorList.Add(magenta);
                    _colorList.Add(orange);
                    _colorList.Add(green2);

                    _colorList.Add(red2);
                    _colorList.Add(yellow);
                    _colorList.Add(blue2);


                    _colorList.Add(red3);
                    _colorList.Add(blue3);
                }

                return _colorList;
            }
        }

        public Color GetColor(int index)
        {
            if (index == -1) return Color.gray;
            return colorList[index];
        }
    }
}
