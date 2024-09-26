using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [System.Serializable]
    public class OrderData
    {
        //咖啡种类
        public NodeTag NodeTag;
        public OrderTag OrderTag;
        public string NodeName;
        //订单生成的时间
        public float OrderTime;
        public bool Grind;

        public OrderData() { }

        public OrderData(NodeTag nodeTag, OrderTag orderTag, int orderTime)
        {
            this.OrderTag = orderTag;
            DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);
            this.NodeTag = nodeTag;
            this.NodeName = dRNode.Name;
            Grind = Random.Range(0, 2) == 1;
            switch (orderTag)
            {
                case OrderTag.None:
                    break;
                case OrderTag.Urgent:
                    OrderTime = orderTime;
                    break;
                case OrderTag.Coarse:
                    Grind = true;
                    break;
                case OrderTag.Fine:
                    Grind = false;
                    break;
            }
        }
    }

    public enum OrderTag
    {
        None,
        Coarse,
        Fine,
        Urgent,
        Vip,
    }

}