//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-09-08 18:31:33.616
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
    /// Query配置表。
    /// </summary>
    public class DRQuery : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取问题编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取（可能的）图片路径。
        /// </summary>
        public string ImagePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取标签内容。
        /// </summary>
        public string Query
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取回答1。
        /// </summary>
        public string Answer1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取回答2。
        /// </summary>
        public string Answer2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取回答3。
        /// </summary>
        public string Answer3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取回答4。
        /// </summary>
        public string Answer4
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取正确答案。
        /// </summary>
        public int TrueAnswer
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取正确答案提示。
        /// </summary>
        public string AnswerTitle
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
            ImagePath = columnStrings[index++];
            Query = columnStrings[index++];
            Answer1 = columnStrings[index++];
            Answer2 = columnStrings[index++];
            Answer3 = columnStrings[index++];
            Answer4 = columnStrings[index++];
            TrueAnswer = int.Parse(columnStrings[index++]);
            AnswerTitle = columnStrings[index++];

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
                    ImagePath = binaryReader.ReadString();
                    Query = binaryReader.ReadString();
                    Answer1 = binaryReader.ReadString();
                    Answer2 = binaryReader.ReadString();
                    Answer3 = binaryReader.ReadString();
                    Answer4 = binaryReader.ReadString();
                    TrueAnswer = binaryReader.Read7BitEncodedInt32();
                    AnswerTitle = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, string>[] m_Answer = null;

        public int AnswerCount
        {
            get
            {
                return m_Answer.Length;
            }
        }

        public string GetAnswer(int id)
        {
            foreach (KeyValuePair<int, string> i in m_Answer)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetAnswer with invalid id '{0}'.", id));
        }

        public string GetAnswerAt(int index)
        {
            if (index < 0 || index >= m_Answer.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetAnswerAt with invalid index '{0}'.", index));
            }

            return m_Answer[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_Answer = new KeyValuePair<int, string>[]
            {
                new KeyValuePair<int, string>(1, Answer1),
                new KeyValuePair<int, string>(2, Answer2),
                new KeyValuePair<int, string>(3, Answer3),
                new KeyValuePair<int, string>(4, Answer4),
            };
        }
    }
}
