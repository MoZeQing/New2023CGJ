//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-20 22:19:16.761
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
    /// Node配置文件。
    /// </summary>
    public class DRNode : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取节点ID。
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
        /// 获取TRUE。
        /// </summary>
        public bool Material
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取FALSE。
        /// </summary>
        public bool Tool
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取FALSE。
        /// </summary>
        public bool Coffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取制作这个成品所需时间。
        /// </summary>
        public float ProducingTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取所在层。
        /// </summary>
        public string Layer
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取所在层级。
        /// </summary>
        public int Layerint
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
            Material = bool.Parse(columnStrings[index++]);
            Tool = bool.Parse(columnStrings[index++]);
            Coffee = bool.Parse(columnStrings[index++]);
            ProducingTime = float.Parse(columnStrings[index++]);
            Layer = columnStrings[index++];
            Layerint = int.Parse(columnStrings[index++]);

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
                    Material = binaryReader.ReadBoolean();
                    Tool = binaryReader.ReadBoolean();
                    Coffee = binaryReader.ReadBoolean();
                    ProducingTime = binaryReader.ReadSingle();
                    Layer = binaryReader.ReadString();
                    Layerint = binaryReader.Read7BitEncodedInt32();
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
