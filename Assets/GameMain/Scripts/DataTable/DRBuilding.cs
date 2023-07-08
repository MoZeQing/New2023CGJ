//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-06-26 18:30:06.436
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
    /// Building配置文件。
    /// </summary>
    public class DRBuilding : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取建筑ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取电力供应。
        /// </summary>
        public int Eletricity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取宿舍供应。
        /// </summary>
        public int Dorm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取种植供应。
        /// </summary>
        public int Garden
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取标题描述。
        /// </summary>
        public string Title
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取供应描述。
        /// </summary>
        public string Produre
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取详细描述。
        /// </summary>
        public string Description
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
            AssetName = columnStrings[index++];
            Eletricity = int.Parse(columnStrings[index++]);
            Dorm = int.Parse(columnStrings[index++]);
            Garden = int.Parse(columnStrings[index++]);
            Title = columnStrings[index++];
            Produre = columnStrings[index++];
            Description = columnStrings[index++];

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
                    AssetName = binaryReader.ReadString();
                    Eletricity = binaryReader.Read7BitEncodedInt32();
                    Dorm = binaryReader.Read7BitEncodedInt32();
                    Garden = binaryReader.Read7BitEncodedInt32();
                    Title = binaryReader.ReadString();
                    Produre = binaryReader.ReadString();
                    Description = binaryReader.ReadString();
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
