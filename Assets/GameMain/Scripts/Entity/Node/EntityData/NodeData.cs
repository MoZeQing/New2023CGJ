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
        Ice,//±ù
        Sugar,//ÌÇ
        Salt,//ÑÎ
        CondensedMilk,//Á¶Èé
        LowFoamingMilk,//µÍÅİÅ£ÄÌ
        HighFoamingMilk,//¸ßÅİÅ£ÄÌ
        //¹¤¾ß
        ManualGrinder,//ÊÖ¶¯ÑĞÄ¥Æ÷
        Extractor,//µç¶¯İÍÈ¡
        ElectricGrinder,//µç¶¯ÑĞÄ¥Æ÷
        Heater,//¼ÓÈÈÆ÷
        Syphon,//ºçÎüºø
        FrenchPress,//·¨Ñ¹ºø
        Kettle,//½şÅİºø
        FilterBowl,//ÂËÖ½Ê½ÂË±­
        Cup,//¿§·È±­
        Stirrer,//½Á°èÆ÷
        //¿§·È
        Espresso,//Å¨Ëõ¿§·È
        HotCafeAmericano,//ÈÈÃÀÊ½
        IceCafeAmericano,//ÀäÃÀÊ½
        HotLatte,//ÈÈÄÃÌú
        IceLatte,//±ùÄÃÌú
        HotMocha,//ÈÈÄ¦¿¨
        IceMocha,//ÀäÄ¦¿¨
        Kapuziner,//¿¨²¼ÆæÅµ
        FlatWhite,//°Ä°×
        None
    }
}

