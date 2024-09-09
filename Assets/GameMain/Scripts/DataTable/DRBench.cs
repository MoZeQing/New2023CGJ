//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-09-08 18:31:33.497
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
    /// level配置文件。
    /// </summary>
    public class DRBench : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取效果ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取好感度要求。
        /// </summary>
        public int Favor
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡片描述。
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取获得的金钱。
        /// </summary>
        public int Money
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取获得的体力。
        /// </summary>
        public int Energy
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取获得的最大体力值。
        /// </summary>
        public int EnergyMax
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取获得的buff。
        /// </summary>
        public List<String> Buff
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
            Favor = int.Parse(columnStrings[index++]);
            Text = columnStrings[index++];
            Money = int.Parse(columnStrings[index++]);
            Energy = int.Parse(columnStrings[index++]);
            EnergyMax = int.Parse(columnStrings[index++]);
            Buff = DataTableExtension.ParseListString(columnStrings[index++]);

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
                    Favor = binaryReader.Read7BitEncodedInt32();
                    Text = binaryReader.ReadString();
                    Money = binaryReader.Read7BitEncodedInt32();
                    Energy = binaryReader.Read7BitEncodedInt32();
                    EnergyMax = binaryReader.Read7BitEncodedInt32();
                    Buff = binaryReader.ReadListString();
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
