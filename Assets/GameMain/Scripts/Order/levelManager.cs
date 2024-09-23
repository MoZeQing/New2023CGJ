using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework.DataTable;
using UnityEngine.UI;
using System.Xml;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private void Start()
        {
            
        }
    }

    [System.Serializable]
    public class NewOrderData
    { 
        /// <summary>
        /// 该份订单的标签
        /// </summary>
        public int nodeNodeTag;
        /// <summary>
        /// 订单模式
        /// </summary>
        public OrderTag orderTag;
        /// <summary>
        /// 该份订单出现的时间，只要不为零则自动视为带时限的订单
        /// </summary>
        public int orderTime;

        public OrderData GetOrderData(NewOrderData newOrderData)
        {
            return GetOrderData(newOrderData.orderTag, newOrderData.orderTime);
        }

        public OrderData GetOrderData(OrderTag orderTag,int orderTime)
        { 
            OrderData orderData=new OrderData();
            DRTag dRTag=GameEntry.DataTable.GetDataTable<DRTag>().GetDataRow(nodeNodeTag);
            string[] tagsText = dRTag.NodeTags.Split('-');
            int[] tags=new int[tagsText.Length];
            for (int i = 0; i < tagsText.Length; i++)
            {
                int tag = 0;
                if (int.TryParse(tagsText[i], out tag))
                {
                    tags[i] = tag;
                }
            }
            return new OrderData((NodeTag)tags[Random.Range(0, tags.Length)], orderTag,orderTime);
        }
    }
}