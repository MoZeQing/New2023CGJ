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

        public NodeData(int entityId, int typeId, NodeTag node)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
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
        GroundCoffee,//¿§·È·Û
        Water,//Ë®
        HotWater,//ÈÈË®
        Milk,//Å£ÄÌ
        HotMilk,//ÈÈÅ£ÄÌ
        Cream,//ÄÌÓÍ
        ChocolateSyrup,//ÇÉ¿ËÁ¦½¬
        CoffeeLiquid,//¿§·ÈÒº
        Sugar,
        //¹¤¾ß
        Burnisher,//ÑĞÄ¥Æ÷
        Kettle,//¼ÓÈÈºø
        FilterBowl,//ÂËÖ½Ê½ÂË±­
        Cup,//¿§·È±­
        //¿§·È
        Espresso,//Å¨Ëõ¿§·È
        Coffee,//¿§·È
        CafeAmericano,//ÃÀÊ½¿§·È
        WhiteCoffee,//°×¿§·È
        Latte,//ÄÃÌú
        Mocha,//Ä¦¿¨
        ConPanna,//¿µ±¦À¶

    }
}

