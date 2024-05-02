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

        public BaseCompenent Adsorb
        {
            get;
            set;
        }

        public bool RamdonJump
        {
            get;
            set;
        }
        public List<NodeTag> Materials
        {
            get;
            set;
        } = new List<NodeTag>();
        public bool IsCoffee
        {
            get;
            set;
        } = false;
        public bool Grind
        {
            get;
            set;
        } = false;
        public bool Ice
        {
            get;
            set;
        } = false;
        public NodeData(int entityId, int typeId, NodeTag node)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
        }
        public NodeData(int entityId, int typeId, NodeTag node,int level=0)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
        }
        public NodeData(int entityId, int typeId, NodeTag node, bool isCoffee, int level=0)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
        }
        public NodeData(int entityId, int typeId, NodeTag node, bool isCoffee, List<NodeTag> materials, int level=0)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
            this.Materials = materials;
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
        HHeater=111,
        HStirrer=112,
        //¿§·È£¨200-300£©
        EspressoG =200,//´Ö¿§·È·Û
        Espresso =201,//Å¨Ëõ¿§·È
        CafeAmericano =202,//ÈÈÃÀÊ½
        IceCafeAmericano =203,//ÀäÃÀÊ½
        Kapuziner = 204,//¿¨²¼ÆæÅµ
        IceKapuziner = 205,//±ù¿¨²¼ÆæÅµ
        Latte = 206,//ÄÃÌú
        IceLatte = 207,//±ùÄÃÌú
        Conpanna =208,//¿µ±¦À¶
        IceConpanna =209,//±ù¿µ±¦À¶
        Ole = 210,//Å·ÀÙ
        IceOle = 211,//±ùÅ·ÀÙ
        Mocha = 212,//Ä¦¿¨
        IceMocha = 213,//ÀäÄ¦¿¨
        Vienna =214,//Î¬Ò²ÄÉ
        IceVienna = 215,//±ùÎ¬Ò²ÄÉ
        Macchiato=216,//ÂêÆæ¶ä
        IceMacchiato =217,//±ùÂêÆæ¶ä
        SaltCoffee =218,//º£ÑÎ
        Mediterranean =219,//µØÖĞº£
        Cat =999,//Ã¨Ã¨¿¨£¨999£©
        None=0
    }
}

