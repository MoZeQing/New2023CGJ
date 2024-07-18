//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-18 17:41:50.543
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
    public class DRShop : DataRowBase
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
        /// 获取商店界面。
        /// </summary>
        public string Shop
        {
            get;
            private set;
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
        /// 获取好感度要求。
        /// </summary>
        public int Favor
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取差分。
        /// </summary>
        public int Diff
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对话内容。
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取获得的好感度。
        /// </summary>
        public int AddFavor
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
            Shop = columnStrings[index++];
            Char = columnStrings[index++];
            Favor = int.Parse(columnStrings[index++]);
            Diff = int.Parse(columnStrings[index++]);
            Text = columnStrings[index++];
            AddFavor = int.Parse(columnStrings[index++]);

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
                    Shop = binaryReader.ReadString();
                    Char = binaryReader.ReadString();
                    Favor = binaryReader.Read7BitEncodedInt32();
                    Diff = binaryReader.Read7BitEncodedInt32();
                    Text = binaryReader.ReadString();
                    AddFavor = binaryReader.Read7BitEncodedInt32();
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
