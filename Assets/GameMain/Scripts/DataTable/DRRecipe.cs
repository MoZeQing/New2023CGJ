//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-23 13:52:50.388
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
    /// Recipe配置文件。
    /// </summary>
    public class DRRecipe : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取成品ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取成品名字。
        /// </summary>
        public string Product
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取咖啡豆。
        /// </summary>
        public bool CoffeeBean
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取咖啡粉。
        /// </summary>
        public bool GroundCoffee
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取水。
        /// </summary>
        public bool Water
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取热水。
        /// </summary>
        public bool HotWater
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取牛奶。
        /// </summary>
        public bool Milk
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取热牛奶。
        /// </summary>
        public bool HotMilk
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取奶油。
        /// </summary>
        public bool Cream
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取巧克力浆。
        /// </summary>
        public bool ChocolateSyrup
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取咖啡液。
        /// </summary>
        public bool CoffeeLiquid
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取糖。
        /// </summary>
        public bool Sugar
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取浓缩咖啡。
        /// </summary>
        public bool Espresso
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
            Product = columnStrings[index++];
            CoffeeBean = bool.Parse(columnStrings[index++]);
            GroundCoffee = bool.Parse(columnStrings[index++]);
            Water = bool.Parse(columnStrings[index++]);
            HotWater = bool.Parse(columnStrings[index++]);
            Milk = bool.Parse(columnStrings[index++]);
            HotMilk = bool.Parse(columnStrings[index++]);
            Cream = bool.Parse(columnStrings[index++]);
            ChocolateSyrup = bool.Parse(columnStrings[index++]);
            CoffeeLiquid = bool.Parse(columnStrings[index++]);
            Sugar = bool.Parse(columnStrings[index++]);
            Espresso = bool.Parse(columnStrings[index++]);

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
                    Product = binaryReader.ReadString();
                    CoffeeBean = binaryReader.ReadBoolean();
                    GroundCoffee = binaryReader.ReadBoolean();
                    Water = binaryReader.ReadBoolean();
                    HotWater = binaryReader.ReadBoolean();
                    Milk = binaryReader.ReadBoolean();
                    HotMilk = binaryReader.ReadBoolean();
                    Cream = binaryReader.ReadBoolean();
                    ChocolateSyrup = binaryReader.ReadBoolean();
                    CoffeeLiquid = binaryReader.ReadBoolean();
                    Sugar = binaryReader.ReadBoolean();
                    Espresso = binaryReader.ReadBoolean();
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
