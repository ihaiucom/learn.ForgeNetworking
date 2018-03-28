using System;
using System.IO;
using System.Collections.Generic;

namespace com.ihaiu
{
	public class CsvData
	{
		public char 			delimiter = '\t';
		public List<string[]> 	lines = new List<string[]>();

		public CsvData (char delimiter = '\t')
		{
            this.delimiter = delimiter;

        }

		/** 行数 */
		public int LineCount
		{
			get 
			{
				return lines.Count;
			}
		}

		/** 列数*/
		public int ColumnCount
		{
			get
			{
				if (LineCount > 0)
				{
					return lines [0].Length;
				}
				return 0;
			}
		}

		/** 获取行数据 */
		public string[] GetLine(int lineIndex)
		{
			if (lineIndex >= LineCount)
				return null;
			
			return lines[lineIndex];
		}


		/** 获取单元格数据 */
		public string GetCell(int lineIndex, int columnIndex)
		{
			string[] csv = GetLine (lineIndex);
			if (csv == null || columnIndex >= csv.Length)
				return null;
			
			return lines[lineIndex][columnIndex];
		}



		/** 读取csv文件 */
		public CsvData ReadFile(string path)
		{
			string txt = File.ReadAllText (path);
			ParseAsset (txt);
			return this;
		}

		/** 解析 */
		public CsvData ParseAsset(string txt)
		{
			StringReader sr = new StringReader(txt);

			string      line;
			string[]    csv;
			while(true)
			{
				line = sr.ReadLine();
				if (line == null)
				{
					break;
				}

				csv = line.Split(delimiter);
				if (csv.Length != 0)
				{
					lines.Add (csv);
				}
			}

			sr.Dispose ();
			return this;
		}





	}
}

