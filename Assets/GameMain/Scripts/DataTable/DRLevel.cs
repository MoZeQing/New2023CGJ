//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-09-11 11:49:05.584
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
    public class DRLevel : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取关卡ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取角色。
        /// </summary>
        public string Char
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应订单。
        /// </summary>
        public int Order
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应天数（由1开始）。
        /// </summary>
        public int Day
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应剧情。
        /// </summary>
        public string Foreword
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取正文。
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应单数。
        /// </summary>
        public int Index
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应交互。
        /// </summary>
        public string ActionGraph
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
            Char = columnStrings[index++];
            Order = int.Parse(columnStrings[index++]);
            Day = int.Parse(columnStrings[index++]);
            Foreword = columnStrings[index++];
            Text = columnStrings[index++];
            Index = int.Parse(columnStrings[index++]);
            ActionGraph = columnStrings[index++];

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
                    Char = binaryReader.ReadString();
                    Order = binaryReader.Read7BitEncodedInt32();
                    Day = binaryReader.Read7BitEncodedInt32();
                    Foreword = binaryReader.ReadString();
                    Text = binaryReader.ReadString();
                    Index = binaryReader.Read7BitEncodedInt32();
                    ActionGraph = binaryReader.ReadString();
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
