//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-09-29 22:41:24.978
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
        /// 获取名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取背景路径。
        /// </summary>
        public string BackgroundPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取图标路径。
        /// </summary>
        public string IconPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描边路径。
        /// </summary>
        public string CoverPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取特效路径。
        /// </summary>
        public string EffectPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否是工具。
        /// </summary>
        public bool Tool
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否是咖啡。
        /// </summary>
        public bool Coffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否是冰咖啡。
        /// </summary>
        public bool Ice
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取制作时间。
        /// </summary>
        public int Time
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取生成时音效。
        /// </summary>
        public string ShowSound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取悬浮音效路径。
        /// </summary>
        public string HoldSound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取点击音效路径。
        /// </summary>
        public string ClickSound
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
            Name = columnStrings[index++];
            BackgroundPath = columnStrings[index++];
            IconPath = columnStrings[index++];
            CoverPath = columnStrings[index++];
            EffectPath = columnStrings[index++];
            Tool = bool.Parse(columnStrings[index++]);
            Coffee = bool.Parse(columnStrings[index++]);
            Ice = bool.Parse(columnStrings[index++]);
            Price = int.Parse(columnStrings[index++]);
            Time = int.Parse(columnStrings[index++]);
            Description = columnStrings[index++];
            ShowSound = columnStrings[index++];
            HoldSound = columnStrings[index++];
            ClickSound = columnStrings[index++];

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
                    Name = binaryReader.ReadString();
                    BackgroundPath = binaryReader.ReadString();
                    IconPath = binaryReader.ReadString();
                    CoverPath = binaryReader.ReadString();
                    EffectPath = binaryReader.ReadString();
                    Tool = binaryReader.ReadBoolean();
                    Coffee = binaryReader.ReadBoolean();
                    Ice = binaryReader.ReadBoolean();
                    Price = binaryReader.Read7BitEncodedInt32();
                    Time = binaryReader.Read7BitEncodedInt32();
                    Description = binaryReader.ReadString();
                    ShowSound = binaryReader.ReadString();
                    HoldSound = binaryReader.ReadString();
                    ClickSound = binaryReader.ReadString();
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
