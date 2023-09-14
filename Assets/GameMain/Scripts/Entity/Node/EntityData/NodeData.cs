using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;


namespace GameMain
{
    public class NodeData : EntityData
    {
        /// <summary>
        /// 建筑的种类
        /// </summary>
        public NodeTag NodeTag
        {
            get;
            set;
        }

        public NodeGroup NodeGroup
        {
            get;
            set;
        }

        public Vector3 Scale
        {
            get;
            set;
        }

        public float ProducingTime
        {
            get;
            set;
        }

        public bool Follow
        {
            get;
            set;
        }
        public int MLevel
        {
            get;
            set;
        } = 0;
        public bool Jump
        {
            get;
            set;
        }

        public bool RamdonJump
        {
            get;
            set;
        }
        public bool IsCoffee
        {
            get;
            set;
        } = false;

        public NodeData(int entityId, int typeId, NodeTag node)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
        }
        public NodeData(int entityId, int typeId, NodeTag node,int level)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
        }
        public NodeData(int entityId, int typeId, NodeTag node, int level,bool isCoffee)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
        }
    }

    public enum NodeGroup
    { 
        Material,
        Tool,
        Coffee
    }

    public enum NodeTag
    {
        //材料
        CoffeeBean,//咖啡豆
        CoarseGroundCoffee,//粗咖啡粉
        MidGroundCoffee,//中咖啡粉
        FineGroundCoffee,//细咖啡粉
        Water,//水
        HotWater,//热水
        Milk,//牛奶
        HotMilk,//热牛奶
        Cream,//奶油
        ChocolateSyrup,//巧克力浆
        Ice,//冰
        Sugar,//糖
        Salt,//盐
        CondensedMilk,//炼乳
        LowFoamingMilk,//低泡牛奶
        HighFoamingMilk,//高泡牛奶
        //工具
        ManualGrinder,//手动研磨器
        Extractor,//电动萃取
        ElectricGrinder,//电动研磨器
        Heater,//加热器
        Syphon,//虹吸壶
        FrenchPress,//法压壶
        Kettle,//浸泡壶
        FilterBowl,//滤纸式滤杯
        Cup,//咖啡杯
        Stirrer,//搅拌器
        //咖啡
        Espresso,//浓缩咖啡
        HotCafeAmericano,//热美式
        IceCafeAmericano,//冷美式
        HotLatte,//热拿铁
        IceLatte,//冰拿铁
        HotMocha,//热摩卡
        IceMocha,//热摩卡
        Kapuziner,//卡布奇诺
        FlatWhite,//澳白
        None
    }
}

