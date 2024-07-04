//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-04 17:08:52.523
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
        public string Tags
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
        /// 获取初始价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取客流倍率。
        /// </summary>
        public string DemandLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取价格倍率。
        /// </summary>
        public string PriceLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取经验值。
        /// </summary>
        public string ExpLevel
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
            Tags = columnStrings[index++];
            Demand = int.Parse(columnStrings[index++]);
            Price = int.Parse(columnStrings[index++]);
            DemandLevel = columnStrings[index++];
            PriceLevel = columnStrings[index++];
            ExpLevel = columnStrings[index++];

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
                    Tags = binaryReader.ReadString();
                    Demand = binaryReader.Read7BitEncodedInt32();
                    Price = binaryReader.Read7BitEncodedInt32();
                    DemandLevel = binaryReader.ReadString();
                    PriceLevel = binaryReader.ReadString();
                    ExpLevel = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
