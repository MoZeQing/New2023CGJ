//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-04-26 16:09:25.076
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
    /// 升级配置表。
    /// </summary>
    public class DRUpgrade : DataRowBase
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
        /// 获取优先级（默认0，128最高，-128最低）。
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取A级咖啡要求。
        /// </summary>
        public int ACoffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取B级咖啡要求。
        /// </summary>
        public int BCoffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取C级咖啡要求。
        /// </summary>
        public int CCoffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取咖啡总量。
        /// </summary>
        public int Total
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
        /// 获取解锁的咖啡。
        /// </summary>
        public string UnlockCoffee
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
            ACoffee = int.Parse(columnStrings[index++]);
            BCoffee = int.Parse(columnStrings[index++]);
            CCoffee = int.Parse(columnStrings[index++]);
            Total = int.Parse(columnStrings[index++]);
            Money = int.Parse(columnStrings[index++]);
            UnlockCoffee = columnStrings[index++];
            Increase = int.Parse(columnStrings[index++]);
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
                    ACoffee = binaryReader.Read7BitEncodedInt32();
                    BCoffee = binaryReader.Read7BitEncodedInt32();
                    CCoffee = binaryReader.Read7BitEncodedInt32();
                    Total = binaryReader.Read7BitEncodedInt32();
                    Money = binaryReader.Read7BitEncodedInt32();
                    UnlockCoffee = binaryReader.ReadString();
                    Increase = binaryReader.Read7BitEncodedInt32();
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
