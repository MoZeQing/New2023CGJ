//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-06-27 00:35:35.304
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// Coffee配置文件。
    /// </summary>
    public class DRCoffee : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取节点ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取对应咖啡Tag。
        /// </summary>
        public int NodeTag
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取咖啡名称。
        /// </summary>
        public string CoffeeName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取图片路径。
        /// </summary>
        public string ImagePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取标签。
        /// </summary>
        public List<String> Tags
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取客流。
        /// </summary>
        public int Demand
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取倍率1。
        /// </summary>
        public float Level1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取倍率2。
        /// </summary>
        public float Level2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取倍率3。
        /// </summary>
        public float Level3
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            NodeTag = int.Parse(columnStrings[index++]);
            CoffeeName = columnStrings[index++];
            ImagePath = columnStrings[index++];
            Tags = DataTableExtension.ParseListString(columnStrings[index++]);
            Demand = int.Parse(columnStrings[index++]);
            Level1 = float.Parse(columnStrings[index++]);
            Level2 = float.Parse(columnStrings[index++]);
            Level3 = float.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    NodeTag = binaryReader.Read7BitEncodedInt32();
                    CoffeeName = binaryReader.ReadString();
                    ImagePath = binaryReader.ReadString();
                    Tags = binaryReader.ReadListString();
                    Demand = binaryReader.Read7BitEncodedInt32();
                    Level1 = binaryReader.ReadSingle();
                    Level2 = binaryReader.ReadSingle();
                    Level3 = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, float>[] m_Level = null;

        public int LevelCount
        {
            get
            {
                return m_Level.Length;
            }
        }

        public float GetLevel(int id)
        {
            foreach (KeyValuePair<int, float> i in m_Level)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetLevel with invalid id '{0}'.", id));
        }

        public float GetLevelAt(int index)
        {
            if (index < 0 || index >= m_Level.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetLevelAt with invalid index '{0}'.", index));
            }

            return m_Level[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_Level = new KeyValuePair<int, float>[]
            {
                new KeyValuePair<int, float>(1, Level1),
                new KeyValuePair<int, float>(2, Level2),
                new KeyValuePair<int, float>(3, Level3),
            };
        }
    }
}
