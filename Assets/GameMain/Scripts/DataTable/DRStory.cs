//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-09-08 15:18:39.719
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
    /// Story配置表。
    /// </summary>
    public class DRStory : DataRowBase
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
        public string StoryName
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
        /// 获取外出场景。
        /// </summary>
        public int OutingSceneState
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取游戏模式。
        /// </summary>
        public int GameState
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
        /// 获取故事名称。
        /// </summary>
        public string DialogName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取事件。
        /// </summary>
        public string EventText
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
            StoryName = columnStrings[index++];
            IsRemove = bool.Parse(columnStrings[index++]);
            OutingSceneState = int.Parse(columnStrings[index++]);
            GameState = int.Parse(columnStrings[index++]);
            Trigger = columnStrings[index++];
            DialogName = columnStrings[index++];
            EventText = columnStrings[index++];

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
                    StoryName = binaryReader.ReadString();
                    IsRemove = binaryReader.ReadBoolean();
                    OutingSceneState = binaryReader.Read7BitEncodedInt32();
                    GameState = binaryReader.Read7BitEncodedInt32();
                    Trigger = binaryReader.ReadString();
                    DialogName = binaryReader.ReadString();
                    EventText = binaryReader.ReadString();
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
