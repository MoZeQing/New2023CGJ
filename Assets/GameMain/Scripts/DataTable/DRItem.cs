//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-09-11 18:05:00.916
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
        /// 获取图片路径。
        /// </summary>
        public string ImagePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取图标路径。
        /// </summary>
        public string IconPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取特别图标路径。
        /// </summary>
        public string ClothingPath
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
        /// 获取事件。
        /// </summary>
        public string EventData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取道具名称。
        /// </summary>
        public string ItemName
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
            ImagePath = columnStrings[index++];
            IconPath = columnStrings[index++];
            ClothingPath = columnStrings[index++];
            Price = int.Parse(columnStrings[index++]);
            EventData = columnStrings[index++];
            ItemName = columnStrings[index++];
            Info = columnStrings[index++];
            Equipable = bool.Parse(columnStrings[index++]);
            MaxNum = int.Parse(columnStrings[index++]);
            Kind = int.Parse(columnStrings[index++]);

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
                    ImagePath = binaryReader.ReadString();
                    IconPath = binaryReader.ReadString();
                    ClothingPath = binaryReader.ReadString();
                    Price = binaryReader.Read7BitEncodedInt32();
                    EventData = binaryReader.ReadString();
                    ItemName = binaryReader.ReadString();
                    Info = binaryReader.ReadString();
                    Equipable = binaryReader.ReadBoolean();
                    MaxNum = binaryReader.Read7BitEncodedInt32();
                    Kind = binaryReader.Read7BitEncodedInt32();
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
