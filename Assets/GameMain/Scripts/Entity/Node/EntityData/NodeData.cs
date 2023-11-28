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

        public bool RamdonJump
        {
            get;
            set;
        }
        public List<NodeTag> M_Materials
        {
            get;
            set;
        } = new List<NodeTag>();
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
        public NodeData(int entityId, int typeId, NodeTag node, int level, bool isCoffee,List<NodeTag> materials)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
            this.M_Materials = materials;
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
        //²ÄÁÏ£¨1-100£©
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
        Ice,//±ù
        Sugar,//ÌÇ
        Salt,//ÑÎ
        CondensedMilk,//Á¶Èé
        LowFoamingMilk,//µÍÅİÅ£ÄÌ
        HighFoamingMilk,//¸ßÅİÅ£ÄÌ
        //¹¤¾ß£¨101-200£©
        ManualGrinder=101,//ÊÖ¶¯ÑĞÄ¥Æ÷
        Extractor=102,//µç¶¯İÍÈ¡
        ElectricGrinder=103,//µç¶¯ÑĞÄ¥Æ÷
        Heater=104,//¼ÓÈÈÆ÷
        Syphon=105,//ºçÎüºø
        FrenchPress=106,//·¨Ñ¹ºø
        Kettle=107,//½şÅİºø
        FilterBowl=108,//ÂËÖ½Ê½ÂË±­
        Cup=109,//¿§·È±­
        Stirrer=110,//½Á°èÆ÷
        //¿§·È£¨201-300£©
        Espresso=201,//Å¨Ëõ¿§·È
        HotCafeAmericano=202,//ÈÈÃÀÊ½
        IceCafeAmericano=203,//ÀäÃÀÊ½
        Conpanna=204,//¿µ±¦À¶
        Vienna=205,//Î¬Ò²ÄÉ¿§·È
        HotLatte=206,//ÈÈÄÃÌú
        IceLatte=207,//±ùÄÃÌú
        HotMocha=208,//ÈÈÄ¦¿¨
        IceMocha=209,//ÀäÄ¦¿¨
        Kapuziner=210,//¿¨²¼ÆæÅµ
        FlatWhite=211,//°Ä°×
        Dirty=212,//Ôà¿§·È
        Ole=213,//Å·ÀÙ¿§·È
        Cat=999,//Ã¨Ã¨¿¨£¨999£©
        None=0
    }
}

