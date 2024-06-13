//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-06-13 19:03:23.356
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
    /// 等级配置表。
    /// </summary>
    public class DRLevel : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取等级编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取标签代号。
        /// </summary>
        public string TagIcon
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取订单数量。
        /// </summary>
        public int OrderNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取经验值（知名度）要求。
        /// </summary>
        public int EXP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取金钱要求。
        /// </summary>
        public int Money
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取时间修正。
        /// </summary>
        public int Time
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数值增长（x/100）。
        /// </summary>
        public int Increase
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取咖啡的数量。
        /// </summary>
        public int Coffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取其它buff。
        /// </summary>
        public string Buff
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级的目标ID。
        /// </summary>
        public int UpgradeID
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
            TagIcon = columnStrings[index++];
            Text = columnStrings[index++];
            OrderNumber = int.Parse(columnStrings[index++]);
            EXP = int.Parse(columnStrings[index++]);
            Money = int.Parse(columnStrings[index++]);
            Time = int.Parse(columnStrings[index++]);
            Increase = int.Parse(columnStrings[index++]);
            Coffee = int.Parse(columnStrings[index++]);
            Buff = columnStrings[index++];
            UpgradeID = int.Parse(columnStrings[index++]);

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
                    TagIcon = binaryReader.ReadString();
                    Text = binaryReader.ReadString();
                    OrderNumber = binaryReader.Read7BitEncodedInt32();
                    EXP = binaryReader.Read7BitEncodedInt32();
                    Money = binaryReader.Read7BitEncodedInt32();
                    Time = binaryReader.Read7BitEncodedInt32();
                    Increase = binaryReader.Read7BitEncodedInt32();
                    Coffee = binaryReader.Read7BitEncodedInt32();
                    Buff = binaryReader.ReadString();
                    UpgradeID = binaryReader.Read7BitEncodedInt32();
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
