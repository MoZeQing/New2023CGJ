//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-22 23:34:31.599
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
    /// Order配置文件。
    /// </summary>
    public class DROrder : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取订单ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取浓缩咖啡。
        /// </summary>
        public int Espresso
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取拿铁。
        /// </summary>
        public int Latte
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取美式咖啡。
        /// </summary>
        public int CafeAmericano
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取白咖啡。
        /// </summary>
        public int WhiteCoffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取摩卡。
        /// </summary>
        public int Mocha
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取康宝蓝。
        /// </summary>
        public int ConPanna
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应剧情。
        /// </summary>
        public string Dialog
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
            Espresso = int.Parse(columnStrings[index++]);
            Latte = int.Parse(columnStrings[index++]);
            CafeAmericano = int.Parse(columnStrings[index++]);
            WhiteCoffee = int.Parse(columnStrings[index++]);
            Mocha = int.Parse(columnStrings[index++]);
            ConPanna = int.Parse(columnStrings[index++]);
            Dialog = columnStrings[index++];

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
                    Espresso = binaryReader.Read7BitEncodedInt32();
                    Latte = binaryReader.Read7BitEncodedInt32();
                    CafeAmericano = binaryReader.Read7BitEncodedInt32();
                    WhiteCoffee = binaryReader.Read7BitEncodedInt32();
                    Mocha = binaryReader.Read7BitEncodedInt32();
                    ConPanna = binaryReader.Read7BitEncodedInt32();
                    Dialog = binaryReader.ReadString();
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
