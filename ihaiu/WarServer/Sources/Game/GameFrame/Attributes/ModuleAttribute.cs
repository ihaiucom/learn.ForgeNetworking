using System.Collections;
using System;

namespace Games
{
    public class ModuleAttribute : Attribute
    {
		public int menuId;
		public ModuleAttribute(int menuId)
        {
			this.menuId = menuId;
        }

    }
}