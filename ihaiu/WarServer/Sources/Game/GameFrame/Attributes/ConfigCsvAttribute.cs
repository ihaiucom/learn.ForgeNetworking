using System.Collections;
using System;

namespace com.ihaiu
{
    public class ConfigCsvAttribute : Attribute
    {
		public string       assetName;
		public bool         hasHeadType;
        public bool         hasHeadPropId;
		public ConfigCsvAttribute(string path, bool hasHeadType, bool hasHeadPropId)
        {
            this.assetName 			= path;
			this.hasHeadType 		= hasHeadType;
            this.hasHeadPropId 		= hasHeadPropId;
        }

    }
}