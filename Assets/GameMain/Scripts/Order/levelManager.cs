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
    //关卡类型
    public enum LevelTag
    {
        Normal,//普通
        Rain,//雨天
        Urgent,//加急
        Bad,//恶劣的客人
        Damage//损坏的工作台
    }

    [System.Serializable]
    public class NewOrderData
    { 
        /// <summary>
        /// 该份订单的标签
        /// </summary>
        public int nodeNodeTag;
        /// <summary>
        /// 该份订单出现的时间，只要不为零则自动视为带时限的订单
        /// </summary>
        public int orderTime;
        /// <summary>
        /// 是否为VIP客人
        /// </summary>
        public bool vip;

        public OrderData GetOrderData(LevelTag levelTag,int orderTime,bool isCoarse,bool notCoarse)
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
            return new OrderData((NodeTag)tags[Random.Range(0, tags.Length)], levelTag,orderTime,isCoarse,notCoarse);
        }
    }
}