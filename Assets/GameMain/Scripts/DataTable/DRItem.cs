//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-09-16 21:04:45.348
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
    /// Item配置文件。
    /// </summary>
    public class DRItem : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取物品ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Info
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取筛选模式。
        /// </summary>
        public int FilterMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取可被装备。
        /// </summary>
        public bool Equipable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大数值。
        /// </summary>
        public int MaxNum
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物品种类。
        /// </summary>
        public int Kind
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加值描述。
        /// </summary>
        public string AMInfo
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
            Name = columnStrings[index++];
            Price = int.Parse(columnStrings[index++]);
            Info = columnStrings[index++];
            FilterMode = int.Parse(columnStrings[index++]);
            Equipable = bool.Parse(columnStrings[index++]);
            MaxNum = int.Parse(columnStrings[index++]);
            Kind = int.Parse(columnStrings[index++]);
            AMInfo = columnStrings[index++];

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
                    Name = binaryReader.ReadString();
                    Price = binaryReader.Read7BitEncodedInt32();
                    Info = binaryReader.ReadString();
                    FilterMode = binaryReader.Read7BitEncodedInt32();
                    Equipable = binaryReader.ReadBoolean();
                    MaxNum = binaryReader.Read7BitEncodedInt32();
                    Kind = binaryReader.Read7BitEncodedInt32();
                    AMInfo = binaryReader.ReadString();
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
