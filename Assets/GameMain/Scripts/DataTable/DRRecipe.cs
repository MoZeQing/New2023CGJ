//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-04-27 00:49:18.991
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
    /// Recipe配置文件。
    /// </summary>
    public class DRRecipe : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取成品ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取制作时间。
        /// </summary>
        public float ProducingTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取制作工具。
        /// </summary>
        public string Tool
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配方表。
        /// </summary>
        public List<String> Recipe
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取成品。
        /// </summary>
        public List<String> Product
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取生成咖啡等级。
        /// </summary>
        public int CoffeeLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否为除浓缩咖啡以外的其它咖啡。
        /// </summary>
        public bool IsCoffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取生产所需要的原材料。
        /// </summary>
        public List<String> Materials
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
            ProducingTime = float.Parse(columnStrings[index++]);
            Tool = columnStrings[index++];
            Recipe = DataTableExtension.ParseListString(columnStrings[index++]);
            Product = DataTableExtension.ParseListString(columnStrings[index++]);
            CoffeeLevel = int.Parse(columnStrings[index++]);
            IsCoffee = bool.Parse(columnStrings[index++]);
            Materials = DataTableExtension.ParseListString(columnStrings[index++]);

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
                    ProducingTime = binaryReader.ReadSingle();
                    Tool = binaryReader.ReadString();
                    Recipe = binaryReader.ReadListString();
                    Product = binaryReader.ReadListString();
                    CoffeeLevel = binaryReader.Read7BitEncodedInt32();
                    IsCoffee = binaryReader.ReadBoolean();
                    Materials = binaryReader.ReadListString();
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
