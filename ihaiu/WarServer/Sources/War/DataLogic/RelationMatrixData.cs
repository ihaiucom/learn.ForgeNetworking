using System;
using System.Collections.Generic;
using Games.Wars;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 1:33:44 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
#if UNITY_EDITOR
    [Serializable]
#endif
    /** 关卡--势力关系矩阵配置 */
    public class RelationMatrixData
    {
        public int                      count = 0;
        public List<int>                legionIdList = new List<int>();
#if UNITY_EDITOR
        [UnityEngine.SerializeField]
#endif
        public int[][]                  datas = new int[][] { };

        /** 势力数量 */
        public int Count
        {
            get
            {
                return legionIdList.Count;
            }
        }

        /** 势力列表 */
        internal List<int> LegionIdList
        {
            get
            {
                return legionIdList;
            }
        }

        private void AddLegion(int legionId)
        {
            if(!legionIdList.Contains(legionId))
                legionIdList.Add(legionId);
        }

        private int GetIndex(int legionId)
        {
            return legionIdList.IndexOf(legionId);
        }

        public int GetLegionId(int index)
        {
            return legionIdList[index];
        }

        /** 关系数据转换成列表 */
        public List<int> DataToList()
        {
            List<int> list = new List<int>();
            for(int y = 0; y < datas.Length; y ++)
            {
                for(int x = 0; x < datas[y].Length; x ++)
                {
                    list.Add(datas[y][x]);
                }
            }
            return list;
        }


        /** 将已有的关系复制到现在的矩阵 */
        public void CopyDataFrom(RelationMatrixData matrix)
        {
            for (int y = 0; y < matrix.LegionIdList.Count; y++)
            {
                int legionA = matrix.LegionIdList[y];
                if (GetIndex(legionA) == -1)
                    continue;

                for (int x = 0; x < matrix.LegionIdList.Count; x++)
                {
                    int legionB = matrix.LegionIdList[x];

                    if (GetIndex(legionB) == -1)
                        continue;

                    SetValue(legionA, legionB, matrix.GetValue(legionA, legionB));
                }
            }
        }

        /** 生成, 根据势力列表和关系矩阵数据 */
        public void Generate(List<int> legionList, List<int> dataList)
        {
            this.legionIdList = new List<int>(legionList.ToArray());
            this.count = legionList.Count;

            int i = 0;
            datas = new int[count][];
            for (int y = 0; y < count; y++)
            {
                datas[y] = new int[count];
                for (int x = 0; x < datas[y].Length; x++)
                {
                    datas[y][x] = dataList[i];
                    i++;
                }
            }
        }


        /** 生成初始化, 根据势力列表 */
        public void Generate(List<int> legionList)
        {
            this.legionIdList = new List<int>(legionList.ToArray());
            this.count = legionList.Count;
            datas = new int[count][];
            for(int y = 0; y < count; y ++)
            {
                datas[y] = new int[count];
            }
        }

        public void Generate(List<StageLegionConfig> legionList)
        {
            List<int> list = new List<int>();
            for(int i = 0; i < legionList.Count; i ++)
            {
                list.Add(legionList[i].legionId);
            }
            Generate(list);
        }

        /** 设置值 */
        public void SetValue(int legionA, int legionB, int val)
        {
            int a = GetIndex(legionA);
            int b = GetIndex(legionB);
            bool isNeedResetData = false;
            if(a == -1)
            {
                AddLegion(legionA);
                a = GetIndex(legionA);
                isNeedResetData = true;
            }

            if (b == -1)
            {
                AddLegion(legionB);
                b = GetIndex(legionB);
                isNeedResetData = true;
            }

            if(isNeedResetData)
            {
                int[][] tmp = datas;
                datas = new int[count][];
                for (int y = 0; y < count; y++)
                {
                    datas[y] = new int[count];
                    for (int x = 0; x < datas[y].Length; x++)
                    {
                        if(y < tmp.Length)
                        {
                            datas[y][x] = tmp[y][x];
                        }
                    }
                }

            }



            if (a < b)
            {
                datas[a][count - b -1] = val;
            }
            else if(a > b)
            {
                datas[b][count - a - 1] = val;
            }
        }

        /** 获取值 */
        public int GetValue(int legionA, int legionB)
        {
            int a = GetIndex(legionA);
            int b = GetIndex(legionB);
            if (a < b)
            {
                return datas[a][count - b - 1];
            }
            else if(a > b)
            {
                return datas[b][count - a - 1];
            }
            else
            {
                return 1;
            }
        }

        /** 获取势力关系 */
        public RelationType GetRelationType(int legionA, int legionB)
        {
            if(legionA == legionB)
            {
                return RelationType.Own;
            }
            return GetValue(legionA, legionB) == 0 ? RelationType.Enemy : RelationType.Friendly;
        }

    }
}
