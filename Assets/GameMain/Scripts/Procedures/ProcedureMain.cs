using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private List<LevelData> m_LevelDatas = new List<LevelData>();
        private LevelData m_LevelData = null;
        private MaterialData mMaterialData = new MaterialData();

        private int mDay = 1;//现在天数
        private int mIndex = 0;//现在关卡数
        private bool m_BackGame = false;

        private MainState mMainState;
        private OrderManager mOrderManager;

        public MainForm MainForm
        {
            get;
            set;
        }

        public void BackGame()
        { 
            m_BackGame=true;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //加载主界面
            Debug.Log(drScene != null);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            m_BackGame = false;
            Debug.Log("Start Load Scene");
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            //初始化信息
            m_LevelDatas.Clear();
            IDataTable<DRLevel> dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            for (int i = 0; i < dtLevel.Count; i++)
            {
                DRLevel dRLevel = dtLevel.GetDataRow(i);
                LevelData level = new LevelData(dRLevel);
                m_LevelDatas.Add(level);
            }
            GameEntry.Event.Subscribe(MaterialEventArgs.EventId, UpdateMaterial);
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, OrderEvent);
            GameEntry.Event.Subscribe(DialogEventArgs.EventId, DialogEvent);
            GameEntry.Event.Subscribe(ChangeEventArgs.EventId, ChangeEvent);
            CheckMaterials();
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //加载主界面
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            Debug.Log("Start Load Scene");
            //GameEntry.Scene.UnloadScene(AssetUtility.GetSceneAsset(drScene.AssetName), this);

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();

            GameEntry.Event.Unsubscribe(MaterialEventArgs.EventId, UpdateMaterial);//这里改成监听所有的实体生产的事件
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, OrderEvent);
            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, DialogEvent);
            GameEntry.Event.Unsubscribe(ChangeEventArgs.EventId, ChangeEvent);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_BackGame)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
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
            levelData.Foreword = string.Format("plot_f_wm_{0}", index);
            levelData.Text = string.Format("plot_wm_{0}", index);
            levelData.Day= mDay;
            levelData.Index= mIndex;
            return levelData;
        }
        //因为监听导致了线程问题，因此弃用监听的模式
        private void OrderEvent(object sender, GameEventArgs e)
        {
            mOrderManager = (OrderManager)sender;
            OrderEventArgs order = (OrderEventArgs)e;
            if (order.OrderData.Check())
                UpdateLevel();
        }
        private void DialogEvent(object sender,GameEventArgs e)
        {
            DialogEventArgs dialog = (DialogEventArgs)e;
            //if (dialog.DialogTag == m_LevelData.Foreword || dialog.DialogTag == m_LevelData.Text)
                UpdateLevel();
        }
        private void ChangeEvent(object sender, GameEventArgs e)
        { 
            if(mMainState==MainState.Change)
                UpdateLevel();
        }
        private void UpdateLevel()
        {
            if(m_LevelData==null)
                mMainState= MainState.Change;
            switch (mMainState)
            {
                case MainState.Foreword:
                    mMainState = MainState.Game;
                    break;
                case MainState.Game:
                    mMainState = MainState.Text;
                    break;
                case MainState.Text:
                    mMainState = MainState.Change;
                    ChangeScene();
                    break;
                case MainState.Change:
                    mMainState = MainState.Foreword;
                    GetLevel();
                    break;
            }
            GameEntry.Event.FireNow(this, LevelEventArgs.Create(mMainState, m_LevelData));
        }
        private void ChangeScene()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, 6f);
        }
        //更新关卡
        public void GetLevel()//改为装配
        {
            Debug.Log("正在初始化关卡");
            mIndex++;
            if (mIndex > 4)
            {
                mDay++;
                mIndex = 1;
            }
            m_LevelData = null;
            foreach (LevelData level in m_LevelDatas)
            {
                if (level.Day == mDay && level.Index == mIndex)
                    m_LevelData = level;
            }
            if (m_LevelData == null)
            {
                m_LevelData = GetRandomLevel();
            }
            mOrderManager.SetOrder(m_LevelData.OrderData);
        }
        private void UpdateMaterial(object sender, GameEventArgs e)
        {
            MaterialEventArgs args = (MaterialEventArgs)e;

            switch (args.NodeTag)
            {
                case NodeTag.Milk:
                    mMaterialData.Milk += args.Value;
                    break;
                case NodeTag.Water:
                    mMaterialData.Water += args.Value;
                    break;
                case NodeTag.Cream:
                    mMaterialData.Cream += args.Value;
                    break;
                case NodeTag.CoffeeBean:
                    mMaterialData.CoffeeBean += args.Value;
                    break;
                case NodeTag.ChocolateSyrup:
                    mMaterialData.ChocolateSyrup += args.Value;
                    break;
            }
            CheckMaterials();
        }
        private void CheckMaterials()
        {
            //其下的数值改为常数
            if (mMaterialData.Milk < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.Milk++;
            }
            if (mMaterialData.Cream < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cream)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.Cream++;
            }
            if (mMaterialData.CoffeeBean < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.CoffeeBean++;
            }
            if (mMaterialData.Water < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.Water++;
            }
            if (mMaterialData.ChocolateSyrup < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ChocolateSyrup)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.ChocolateSyrup++;
            }
        }
    }
    /// <summary>
    /// 目前所处的主游戏状态
    /// </summary>
    public enum MainState
    { 
        Foreword,//前言
        Game,//做咖啡的游戏时间
        Text,//咖啡结束后的正文时间
        Change//转场
    }
}