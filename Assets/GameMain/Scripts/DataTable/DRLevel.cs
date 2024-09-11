//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-09-11 18:05:01.223
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
    /// 教学配置表。
    /// </summary>
    public class DRLevel : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取教学内容编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取故事触发器名称。
        /// </summary>
        public string LevelName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否移除。
        /// </summary>
        public bool IsRemove
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发器条件。
        /// </summary>
        public string Trigger
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取关卡类型。
        /// </summary>
        public int LevelTag
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取前故事名称。
        /// </summary>
        public string ForeDialogName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取后故事名称。
        /// </summary>
        public string AfterDialogName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取订单信息。
        /// </summary>
        public string OrderDatas
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取关卡时间。
        /// </summary>
        public int LevelTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取关卡金钱。
        /// </summary>
        public int LevelMoney
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否全都为粗。
        /// </summary>
        public bool IsCoarse
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否全部不为粗。
        /// </summary>
        public bool NotCoarse
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
            LevelName = columnStrings[index++];
            IsRemove = bool.Parse(columnStrings[index++]);
            Trigger = columnStrings[index++];
            LevelTag = int.Parse(columnStrings[index++]);
            ForeDialogName = columnStrings[index++];
            AfterDialogName = columnStrings[index++];
            OrderDatas = columnStrings[index++];
            LevelTime = int.Parse(columnStrings[index++]);
            LevelMoney = int.Parse(columnStrings[index++]);
            IsCoarse = bool.Parse(columnStrings[index++]);
            NotCoarse = bool.Parse(columnStrings[index++]);

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
                    LevelName = binaryReader.ReadString();
                    IsRemove = binaryReader.ReadBoolean();
                    Trigger = binaryReader.ReadString();
                    LevelTag = binaryReader.Read7BitEncodedInt32();
                    ForeDialogName = binaryReader.ReadString();
                    AfterDialogName = binaryReader.ReadString();
                    OrderDatas = binaryReader.ReadString();
                    LevelTime = binaryReader.Read7BitEncodedInt32();
                    LevelMoney = binaryReader.Read7BitEncodedInt32();
                    IsCoarse = binaryReader.ReadBoolean();
                    NotCoarse = binaryReader.ReadBoolean();
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
