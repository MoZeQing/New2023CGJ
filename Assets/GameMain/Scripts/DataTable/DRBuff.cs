//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-12 17:30:31.319
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
    /// Buff配置文件(所有的数值都以100为底，也就是实际数值会除于100)。
    /// </summary>
    public class DRBuff : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取BuffID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取buff的名称。
        /// </summary>
        public string BuffName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取buffIcon的路径。
        /// </summary>
        public string IconPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取buff描述。
        /// </summary>
        public string BuffText
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取金钱乘法（仅在结算时有效）。
        /// </summary>
        public int MoneyMulti
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取金钱加法。
        /// </summary>
        public int MoneyPlus
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体力乘法。
        /// </summary>
        public int EnergyMulti
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体力加法。
        /// </summary>
        public int EnergyPlus
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体力上限乘法。
        /// </summary>
        public int EnergyMaxMulti
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体力上限加法。
        /// </summary>
        public int EnergyMaxPlus
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取信任乘法。
        /// </summary>
        public int FavorMulti
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取信任加法。
        /// </summary>
        public int FavorPlus
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取时间乘法。
        /// </summary>
        public int TimeMulti
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取时间加法。
        /// </summary>
        public int TimePlus
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
            BuffName = columnStrings[index++];
            IconPath = columnStrings[index++];
            BuffText = columnStrings[index++];
            MoneyMulti = int.Parse(columnStrings[index++]);
            MoneyPlus = int.Parse(columnStrings[index++]);
            EnergyMulti = int.Parse(columnStrings[index++]);
            EnergyPlus = int.Parse(columnStrings[index++]);
            EnergyMaxMulti = int.Parse(columnStrings[index++]);
            EnergyMaxPlus = int.Parse(columnStrings[index++]);
            FavorMulti = int.Parse(columnStrings[index++]);
            FavorPlus = int.Parse(columnStrings[index++]);
            TimeMulti = int.Parse(columnStrings[index++]);
            TimePlus = int.Parse(columnStrings[index++]);

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
                    BuffName = binaryReader.ReadString();
                    IconPath = binaryReader.ReadString();
                    BuffText = binaryReader.ReadString();
                    MoneyMulti = binaryReader.Read7BitEncodedInt32();
                    MoneyPlus = binaryReader.Read7BitEncodedInt32();
                    EnergyMulti = binaryReader.Read7BitEncodedInt32();
                    EnergyPlus = binaryReader.Read7BitEncodedInt32();
                    EnergyMaxMulti = binaryReader.Read7BitEncodedInt32();
                    EnergyMaxPlus = binaryReader.Read7BitEncodedInt32();
                    FavorMulti = binaryReader.Read7BitEncodedInt32();
                    FavorPlus = binaryReader.Read7BitEncodedInt32();
                    TimeMulti = binaryReader.Read7BitEncodedInt32();
                    TimePlus = binaryReader.Read7BitEncodedInt32();
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
