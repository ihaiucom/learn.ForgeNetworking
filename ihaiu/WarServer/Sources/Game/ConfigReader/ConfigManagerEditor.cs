#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;
using com.ihaiu;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Games
{
	public class ConfigManagerEditor 
	{

		public static void Generate()
		{
			List<Type> list = new List<Type>();

			Type 			tie 		= typeof(IConfigReader);
			List<Type> 		ignore 		= new List<Type>{
				typeof(IConfigReader), 
				typeof(ConfigReader<>)
			};


			Assembly[] ass = System.AppDomain.CurrentDomain.GetAssemblies();
			foreach (var oas in ass) 
			{
				Type[] t = oas.GetTypes();
				foreach (var typ in t)
				{
					if (tie.IsAssignableFrom(typ))
					{
						if(ignore.Contains(typ)) continue;

						list.Add(typ);
					}
				}
			}

			list.Sort(SortHanlde);



			//generate s file
			var nsc = list.Select(t => t.Namespace).Distinct();


			using(StreamWriter sw = new StreamWriter(Application.dataPath + "/Game/Scripts/ConfigReader/ConfigManager_List.cs",false))
			{
				sw.WriteLine("using System;");
				sw.WriteLine("using System.Collections;");
				sw.WriteLine("using System.Collections.Generic;");
				sw.WriteLine("using com.ihaiu;");
				foreach (var ns in nsc)
				{
					sw.WriteLine("using " + ns + ";");


					sw.WriteLine("namespace Games\n{");

					sw.WriteLine("\tpublic partial class ConfigManager\n\t{");

					sw.WriteLine("");


					StringWriter _l = new StringWriter();
					foreach(Type type in list)
					{
						string className = type.Name;
						string filedName = className.Replace("ConfigReader", "").FirstLower();

						sw.WriteLine("\t\tpublic {0}	{1}	= new {0}();", className, filedName);

						ignoreAttibute[] atts = type.GetCustomAttributes(typeof(ignoreAttibute), false) as ignoreAttibute[];
						if(atts.Length > 0)
						{
							_l.WriteLine("\t\t\t\t\t//_l.Add(" + filedName + ");");
						}
						else
						{
							_l.WriteLine("\t\t\t\t\t_l.Add(" + filedName + ");");
						}

					}


					sw.WriteLine("");
					sw.WriteLine("");
					sw.WriteLine("\t\tprivate List<IConfigReader> _l;");
					sw.WriteLine("\t\tpublic List<IConfigReader> readerList\n\t\t{");
					sw.WriteLine("\t\t\tget\n\t\t\t{");
					sw.WriteLine("\t\t\t\tif(_l == null)\n\t\t\t\t{");
					sw.WriteLine("\t\t\t\t\t_l = new List<IConfigReader>();");
					sw.WriteLine(_l.ToString());
					sw.WriteLine("\t\t\t\t}");
					sw.WriteLine("\t\t\t\treturn _l;");
					sw.WriteLine("\t\t\t}");
					sw.WriteLine("\t\t}");

					// class
					sw.WriteLine("\t}");

					// namespace
					sw.WriteLine("}");
				}
			}

		}

		static int SortHanlde(Type a, Type b)
		{
			SortAttribute[] attsA = a.GetCustomAttributes(typeof(SortAttribute), false) as SortAttribute[];
			SortAttribute[] attsB = b.GetCustomAttributes(typeof(SortAttribute), false) as SortAttribute[];

			int valA = attsA != null && attsA.Length > 0 ? attsA[0].val : 0;
			int valB = attsB != null && attsB.Length > 0 ? attsB[0].val : 0;

			return valA - valB;
		}
	}
}
#endif