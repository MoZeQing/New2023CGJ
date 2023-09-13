using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;


namespace GameMain
{
    public class NodeData : EntityData
    {
        /// <summary>
        /// ½¨ÖşµÄÖÖÀà
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
    }

    public enum NodeGroup
    { 
        Material,
        Tool,
        Coffee
    }

    public enum NodeTag
    {
        //²ÄÁÏ
        CoffeeBean,//¿§·È¶¹
        CoarseGroundCoffee,//´Ö¿§·È·Û
        MidGroundCoffee,//ÖĞ¿§·È·Û
        FineGroundCoffee,//Ï¸¿§·È·Û
        Water,//Ë®
        HotWater,//ÈÈË®
        Milk,//Å£ÄÌ
        HotMilk,//ÈÈÅ£ÄÌ
        Cream,//ÄÌÓÍ
        ChocolateSyrup,//ÇÉ¿ËÁ¦½¬
        CoffeeLiquid,//¿§·ÈÒº
        Sugar,//ÌÇ
        Ice,//±ù
        LowFoamingMilk,//µÍÅİÅ£ÄÌ
        HighFoamingMilk,//¸ßÅİÅ£ÄÌ
        //¹¤¾ß
        ManualGrinder,//ÊÖ¶¯ÑĞÄ¥Æ÷
        ElectricGrinder,//µç¶¯ÑĞÄ¥Æ÷
        Kettle,//¼ÓÈÈºø
        FilterBowl,//ÂËÖ½Ê½ÂË±­
        Cup,//¿§·È±­
        Stirrer,//½Á°èÆ÷
        //¿§·È
        Espresso,//Å¨Ëõ¿§·È
        Coffee,//¿§·È
        HotCafeAmericano,//ÈÈÃÀÊ½
        WhiteCoffee,//°×¿§·È
        HotLatte,//ÈÈÄÃÌú
        HotMocha,//ÈÈÄ¦¿¨
        ConPanna,//¿µ±¦À¶
        Kapuziner,//¿¨²¼ÆæÅµ
        FlatWhite,//°Ä°×
        //±ù¿§·È
        IceEspresso,//±ùÅ¨Ëõ¿§·È
        IceWhiteCoffee,//±ù°×¿§·È
        IceConPanna,//±ù¿µ±¦À¶
        IceLatte,//±ùÄÃÌú
        IceCafeAmericano,//±ùÃÀÊ½¿§·È
        IceMocha,//±ùÄ¦¿¨
        //¼ÓÌÇ
        SweetEspresso,//¼ÓÌÇÅ¨Ëõ¿§·È
        SweetWhiteCoffee,//¼ÓÌÇ°×¿§·È
        SweetConPanna,//¼ÓÌÇ¿µ±¦À¶
        SweetLatte,//¼ÓÌÇÄÃÌú
        SweetCafeAmericano,//¼ÓÌÇÃÀÊ½¿§·È
        SweetMocha,//¼ÓÌÇÄ¦¿¨
        None
    }
}

