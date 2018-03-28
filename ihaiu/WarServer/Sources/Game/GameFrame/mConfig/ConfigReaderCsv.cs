using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace com.ihaiu
{
    public class ConfigReader<T> : AbstractParseCsv, IConfigReader
    {

		public char 			delimiter = '\t';

        public Dictionary<int, T> configs = new Dictionary<int, T>();
		protected ConfigCsvAttribute arrtr;



        virtual public void Load()
        {
			Type t = this.GetType();
            arrtr = t.GetCustomAttributes(typeof(ConfigCsvAttribute), false)[0] as ConfigCsvAttribute;
            ConfigSetting.Load(arrtr.assetName, ParseAsset);
        }

		virtual public void ParseAsset(string path, string txt)
        {
			if(txt == null)
			{
				Loger.LogErrorFormat("{0}: txt={1}, path={2}", this, txt, path);
				return;
			}
            StringReader sr = new StringReader(txt);

            string      line;
            string[]    csv;

			if (arrtr.hasHeadType) 
			{
				line = sr.ReadLine();
				csv = line.Split(delimiter);
				ParseHeadTypes(csv);
			}

            line = sr.ReadLine();
			csv = line.Split(delimiter);
            ParseHeadKeyCN(csv);

            line = sr.ReadLine();
			csv = line.Split(delimiter);
            ParseHeadKeyEN(csv);

            if (arrtr.hasHeadPropId)
            {
                line = sr.ReadLine();
				csv = line.Split(delimiter);
                ParseHeadPropId(csv);
            }

            while (true)
            {
                line = sr.ReadLine();
                if (line == null)
                {
                    break;
                }

				csv = line.Split(delimiter);
                if (csv.Length != 0 && !string.IsNullOrEmpty(csv[0]))
                {
                    ParseCsv(csv);
                }
            }

			sr.Dispose ();
        }

        override public void Reload()
        {
            base.Reload();
            configs.Clear();
            Load();
        }

		virtual public void OnGameConfigLoaded()
		{
		}

        public T GetConfig(int id)
        {
            if (configs.ContainsKey(id))
            {
                return configs[id];
            }

            Loger.LogErrorFormat("没有找到配置{0}, id={1}", typeof(T), id);

            return default(T);
        }


        /// <summary>
        /// 保存CSV
        /// </summary>
        virtual public void SaveCsv()
        {
#if UNITY_EDITOR
            if (arrtr == null)
            {
                Type t = this.GetType();
                arrtr = t.GetCustomAttributes(typeof(ConfigCsvAttribute), false)[0] as ConfigCsvAttribute;
            }

            string filename = AssetManagerSetting.EditorGetConfigPath(arrtr.assetName);
            SaveCsv(filename, delimiter);
#endif
        }


        virtual public void SaveCsv(string path, char delimiter)
        {
            StringWriter sw = ToCsv(delimiter);
            File.WriteAllText(path, sw.ToString());
        }

        virtual public StringWriter ToCsv(char delimiter = '\t', StringWriter sw = null)
        {
            if (sw == null)
            {
                sw = new StringWriter();
            }

            sw.WriteLine(headTypes.ToStr<string>(delimiter.ToString(), "", ""));
            sw.WriteLine(headKeyCns.ToStr<string>(delimiter.ToString(), "", ""));
            sw.WriteLine(headKeyFields.ToStr<string>(delimiter.ToString(), "", ""));


            Dictionary<int, T> dict = configs.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            foreach(var kvp in dict)
            {
                IToCsv item = kvp.Value as IToCsv;
                sw.WriteLine(item.ToCsv(delimiter));
            }

            return sw;
        }

    }
}
