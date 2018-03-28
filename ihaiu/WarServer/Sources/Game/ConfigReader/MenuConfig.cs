using UnityEngine;
using System.Collections;
using com.ihaiu;
using System;

namespace Games
{
	public class MenuConfig : IToCsv
	{
		/** id */
		public int 		            id;
        /** 名称 */
        public string 	            name;
        /** 模块名称 */
        public string               moduleName;
        /** 预设路径 或者 场景名称 */
        public string               path;
		/** 类型 */
        public MenuType             menuType;
		/** UI层级 */
        public UILayer.Layer        layer;
		/** 布局方式 */
        public MenuLayout           layout;
		/** 关闭其他面板包含哪些 */
        public MenuCloseOtherType   closeOtherType;
		/** 关闭面板后缓存多长时间销毁（-1永久不销毁 0下一帧销毁 大于0会缓存时间秒） */
        public float                cacheTime = -1;
        /** LoadId */
        public int                  loaderType = LoadId.None;
        /** 是否关闭主界面 */
        public bool                 dontCloseMainWindow = false;

        public MenuConfig Clone()
        {
            MenuConfig c = new MenuConfig();
            c.id                = id;
            c.name              = name;
            c.moduleName        = moduleName;
            c.path              = path;
            c.menuType          = menuType;
            c.layer             = layer;
            c.layout            = layout;
            c.closeOtherType    = closeOtherType;
            c.cacheTime         = cacheTime;
            c.loaderType        = loaderType;
            c.dontCloseMainWindow = dontCloseMainWindow;
            return c;
        }

        public void CopyValTo(MenuConfig c)
        {
            c.id = id;
            c.name = name;
            c.moduleName = moduleName;
            c.path = path;
            c.menuType = menuType;
            c.layer = layer;
            c.layout = layout;
            c.closeOtherType = closeOtherType;
            c.cacheTime = cacheTime;
            c.loaderType = loaderType;
            c.dontCloseMainWindow = dontCloseMainWindow;
        }

        public string ToCsv(char delimiter)
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}", delimiter,
                id, name, moduleName, path, (int)menuType, (int)layer, (int)layout, (int)closeOtherType, cacheTime, loaderType, dontCloseMainWindow ? 1 : 0
                );
        }
    }
}
