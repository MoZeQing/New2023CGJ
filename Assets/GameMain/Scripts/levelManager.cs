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
        private List<LevelData> m_LevelDatas = new List<LevelData>();
        private LevelData m_LevelData=null;

        private int mDay=1;//现在天数
        private int mIndex=0;//现在关卡数
        // Start is called before the first frame update
        void Start()
        {
            m_LevelDatas.Clear();
            IDataTable<DRLevel> dtLevel= GameEntry.DataTable.GetDataTable<DRLevel>();
            for (int i=0;i<dtLevel.Count;i++)
            {
                DRLevel dRLevel = dtLevel.GetDataRow(i);
                LevelData level = new LevelData(dRLevel);
            }

        }
        private void OnEnable()
        {
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, Level);
        }
        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, Level);
        }
        void Update()
        {

        }
        private LevelData GetRandomLevel()
        {
            LevelData levelData = new LevelData();
            int total = mDay + Random.Range(0, mDay);
            Debug.Log(total);
            for (int i = 0; i < total; i++)
            {
                int random = Random.Range(1, 6);
                switch (random)
                {
                    case 1:
                        levelData.OrderData.Latte++;
                        break;
                    case 2:
                        levelData.OrderData.WhiteCoffee++;
                        break;
                    case 3:
                        levelData.OrderData.ConPanna++;
                        break;
                    case 4:
                        levelData.OrderData.CafeAmericano++;
                        break;
                    case 5:
                        levelData.OrderData.Espresso++;
                        break;
                    case 6:
                        levelData.OrderData.Mocha++;
                        break; 
                }
            }
            int index = Random.Range(1, 3);
            //levelData.Foreword = /*string.Format("plot_f_wm_{0}", index)*/"plot_f_wm_1";
            //levelData.Text = /*string.Format("plot_wm_{0}", index)*/"plot_wm_1";
            return levelData;
        }
        private void Level(object sender, GameEventArgs e)
        {
            OrderManager orderManager = (OrderManager)sender;
            mIndex++;
            if (mIndex > 4)
            {
                mDay++;
                mIndex = 1;
            }
            m_LevelData = null;
            foreach (LevelData level in m_LevelDatas)
            { 
                if(level.Day==mDay&&level.Index==mIndex)
                    m_LevelData= level;
            }
            if (m_LevelData == null)
            {
                m_LevelData = GetRandomLevel();
            }
            orderManager.SetOrder(m_LevelData.OrderData);
            GameEntry.Event.FireNow(this, DialogEventArgs.Create(m_LevelData.Foreword));
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
        /// 前言
        /// </summary>
        public string Foreword
        {
            get;
            set;
        }
        /// <summary>
        /// 正文
        /// </summary>
        public string Text
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
            OrderData order = new OrderData(dtOrder.GetDataRow(dRLevel.Order));
            OrderData = order;
            Foreword=dRLevel.Foreword;
            Text=dRLevel.Text;
        }
    }
}