using System.Collections;
using System.Collections.Generic;

namespace Games
{
	public partial class ConfigManager 
	{

		public void Load()
		{
			Loger.Log ("ConfigManager Load Begin");
			int count = readerList.Count;
			for(int i = 0; i < count; i ++)
			{
				readerList[i].Load();
			}

			OnGameConfigLoaded();
			Loger.Log ("ConfigManager Load End");
		}

		public void Reload()
		{
			int count = readerList.Count;
			for(int i = 0; i < count; i ++)
			{
				readerList[i].Reload();
			}

			OnGameConfigLoaded();
		}

		public void OnGameConfigLoaded()
		{
			int count = readerList.Count;
			for(int i = 0; i < count; i ++)
			{
				readerList[i].OnGameConfigLoaded();
			}
		}
	}
}