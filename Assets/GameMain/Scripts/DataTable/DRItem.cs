//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-18 16:49:58.679
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

        /// <summary>
        /// 获取加值描述。
        /// </summary>
        public string AMInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取商店标签。
        /// </summary>
        public int ShopIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取亲情。
        /// </summary>
        public int Family
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取爱情。
        /// </summary>
        public int Love
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取信任。
        /// </summary>
        public int Favor
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取希望。
        /// </summary>
        public int Hope
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取心情。
        /// </summary>
        public int Mood
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体力。
        /// </summary>
        public int Energy
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取AP。
        /// </summary>
        public int Ap
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取魅力。
        /// </summary>
        public int Charm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体魄。
        /// </summary>
        public int Stamina
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取智慧。
        /// </summary>
        public int Wisdom
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取前置物品ID。
        /// </summary>
        public int Preposition
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
            Info = columnStrings[index++];
            Equipable = bool.Parse(columnStrings[index++]);
            MaxNum = int.Parse(columnStrings[index++]);
            Kind = int.Parse(columnStrings[index++]);
            AMInfo = columnStrings[index++];
            ShopIndex = int.Parse(columnStrings[index++]);
            Family = int.Parse(columnStrings[index++]);
            Love = int.Parse(columnStrings[index++]);
            Favor = int.Parse(columnStrings[index++]);
            Hope = int.Parse(columnStrings[index++]);
            Mood = int.Parse(columnStrings[index++]);
            Energy = int.Parse(columnStrings[index++]);
            Ap = int.Parse(columnStrings[index++]);
            Charm = int.Parse(columnStrings[index++]);
            Stamina = int.Parse(columnStrings[index++]);
            Wisdom = int.Parse(columnStrings[index++]);
            Preposition = int.Parse(columnStrings[index++]);

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
                    Info = binaryReader.ReadString();
                    Equipable = binaryReader.ReadBoolean();
                    MaxNum = binaryReader.Read7BitEncodedInt32();
                    Kind = binaryReader.Read7BitEncodedInt32();
                    AMInfo = binaryReader.ReadString();
                    ShopIndex = binaryReader.Read7BitEncodedInt32();
                    Family = binaryReader.Read7BitEncodedInt32();
                    Love = binaryReader.Read7BitEncodedInt32();
                    Favor = binaryReader.Read7BitEncodedInt32();
                    Hope = binaryReader.Read7BitEncodedInt32();
                    Mood = binaryReader.Read7BitEncodedInt32();
                    Energy = binaryReader.Read7BitEncodedInt32();
                    Ap = binaryReader.Read7BitEncodedInt32();
                    Charm = binaryReader.Read7BitEncodedInt32();
                    Stamina = binaryReader.Read7BitEncodedInt32();
                    Wisdom = binaryReader.Read7BitEncodedInt32();
                    Preposition = binaryReader.Read7BitEncodedInt32();
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
