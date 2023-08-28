using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework.DataTable;
using UnityEngine.UI;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private void Start()
        {
            
        }
    }

    public class LevelData
    {
        public int Day
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        }
        public OrderData OrderData
        {
            get;
            set;
        }= new OrderData();
        /// <summary>
        /// Ç°ÑÔ
        /// </summary>
        public string Foreword
        {
            get;
            set;
        }
        /// <summary>
        /// ÕýÎÄ
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        public string ActionGraph
        { 
            get;
            set;
        }
        public LevelData() { }
        public LevelData(DRLevel dRLevel)
        {
            Day = dRLevel.Day;
            Index = dRLevel.Index;
            IDataTable<DROrder> dtOrder = GameEntry.DataTable.GetDataTable<DROrder>();
            //OrderData order = new OrderData(dtOrder.GetDataRow(dRLevel.Order));
            //OrderData = order;
            Foreword=dRLevel.Foreword;
            Text=dRLevel.Text;
            ActionGraph= dRLevel.ActionGraph;
        }
    }
}