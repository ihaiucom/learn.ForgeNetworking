using UnityEngine;
using System.Collections;


namespace com.ihaiu
{
    public interface IConfigReader 
    {
        void Load();
        void Reload();

		void OnGameConfigLoaded();
    }
}
