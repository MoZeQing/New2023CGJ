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
        private bool m_ChangeDay = false;

        private MainState mMainState;
        private OrderManager mOrderManager;
        private OrderData mOrderData = null;
        //UI
        private TeachingForm mTeachingForm = null;
        private DialogForm mDialogForm = null;
        private MainForm mMainForm = null;
        private WorkForm mWorkForm = null;

        private Dictionary<string,bool> m_LoadingFlag= new Dictionary<string,bool>();

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


            //初始化信息
            InitMain();

            GameEntry.Event.Subscribe(DialogEventArgs.EventId, DialogEvent);
            
            if(m_LevelData != null)
            {
                m_LevelData = null;
            }
            if(mDay != 1)
            {
                mDay = 1;
            }
            if (mIndex != 0)
            {
                mIndex = 0;
            }
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

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, DialogEvent);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_BackGame)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }

            foreach (KeyValuePair<string, bool> loadedFlag in m_LoadingFlag)
            {
                if (!loadedFlag.Value)
                {
                    return;
                }
            }
        }
        private LevelData GetRandomLevel()
        {
            LevelData levelData = new LevelData();
            int total = Mathf.Clamp(Random.Range(0, mDay), 1, 3);
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
            return levelData;
        }
        private void DialogEvent(object sender,GameEventArgs e)
        {
            DialogEventArgs dialog = (DialogEventArgs)e;
            switch (mMainState)
            {
                case MainState.Dialog:
                    mMainState= MainState.Teach;
                    break;
                case MainState.Text:
                    mMainState = MainState.Teach;
                    break;
                case MainState.Foreword:
                    mMainState = MainState.Game;
                    break;
            }
            UpdateLevel();
        }
        private void UpdateLevel()
        {
            switch (mMainState)
            {
                //游戏阶段
                case MainState.Foreword:
                    break;
                case MainState.Game:
                    break;
                case MainState.Settle:
                    Settle();
                    break;
                case MainState.Text:
                    break;
                //养成阶段
                //当游戏环节结束时自动启动养成环节，当选择睡眠时退出养成阶段
                case MainState.Change:
                    break;
                case MainState.Teach:
                    break;
                case MainState.Dialog:
                    break;
            }
            GameEntry.Event.FireNow(this, LevelEventArgs.Create(mMainState, m_LevelData));
        }
        /// <summary>
        /// 结账(计算方法：完成的比例加上时间的奖励和小费的奖励)
        /// </summary>
        /// 完成比例*时间奖励*单价+小费
        /// 其中的时间奖励是当完成时间在限定时间的
        /*
         * 小费：
         * 小费只在非主要剧情中随机，也即除狗狗和猫猫外的剧情中给予
         * 小费的比例为咖啡费用的 0.01——0.1 倍，向下取整
         * 
         * 时间奖励和惩罚：
         * 时间奖励分为约三个阶段，现假设一杯咖啡的限时为60s
         * 时间分为如右的三个阶段 |----------|----------|----------|----...
         *                      0          30         60         90  /s
         *                        提前完成    准时完成    超时完成
         *                        1.3倍（+0.3） 1倍（0）  0.5倍（-0.5）/单价，向下取整
         */
        private void Settle()
        {
            float a = mOrderData.GetValue();
            float b = m_LevelData.OrderData.GetValue();
            float c = mOrderData.OrderTime;
            float d = m_LevelData.OrderData.OrderTime;
            float e = mOrderData.OrderTips;
            GameEntry.UI.OpenUIForm(UIFormId.SettleForm, (int)(a / b/*  c / d*/ * m_LevelData.OrderData.OrderMoney + e));
        }
        private void Change()
        {
            if (m_ChangeDay)
            {
                GameEntry.UI.OpenUIForm(UIFormId.SettleForm,mOrderData);
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChangeForm,mDay );
            }
        }
        /// <summary>
        /// 初始化游戏（测试）
        /// </summary>
        private void InitMain()
        {
            InitScene();
            InitUI();
            InitData();
        }
        private void InitScene()
        {
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //加载主界面
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            m_BackGame = false;
            //场景加载
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);

            //m_LoadingFlag.Add("LoadScene", false);
        }
        private void InitUI()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.WorkForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.TeachForm, this);

            //m_LoadingFlag.Add("OpenMainForm", false);
            //m_LoadingFlag.Add("OpenWorkForm", false);
            //m_LoadingFlag.Add("OpenTeachForm", false);
        }
        private void InitData()
        {
            GameEntry.Utils.MaxEnergy = 80;
            GameEntry.Utils.Energy = 80;
            GameEntry.Utils.MaxAp = 6;
            GameEntry.Utils.Ap = 6;
            GameEntry.Utils.Money = 10000;
            GameEntry.Utils.Mood = 20;
            GameEntry.Utils.Favor = 0;
        }
        //更新关卡
        public void GetLevel()//改为装配
        {
            mIndex++;
            if (mIndex > 3)
            {
                mDay++;
                mIndex = 1;
                m_ChangeDay = true;
            }
            else
                m_ChangeDay= false;
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

        private void GetForm(object sender,GameEventArgs args)
        { 
            OpenUIFormSuccessEventArgs openUIForm= (OpenUIFormSuccessEventArgs)args;
            if (openUIForm.UIForm.TryGetComponent<DialogForm>(out mDialogForm))
                return;
            if (openUIForm.UIForm.TryGetComponent<WorkForm>(out mWorkForm))
                return;
            if (openUIForm.UIForm.TryGetComponent<MainForm>(out mMainForm))
                return;
            if (openUIForm.UIForm.TryGetComponent<TeachingForm>(out mTeachingForm))
                return;
        }
        #region
        //private void CheckMaterials()
        //{
        //    其下的数值改为常数
        //    if (mMaterialData.Milk < 3)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Milk++;
        //    }
        //    if (mMaterialData.Milk < 3)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Milk++;
        //    }
        //    if (mMaterialData.Milk < 3)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Milk++;
        //    }
        //    if (mMaterialData.Cream < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cream)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Cream++;
        //    }
        //    if (mMaterialData.CoffeeBean < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.CoffeeBean++;
        //    }
        //    if (mMaterialData.Water < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Water++;
        //    }
        //    if (mMaterialData.ChocolateSyrup < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ChocolateSyrup)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.ChocolateSyrup++;
        //    }
        //    if (mMaterialData.Ice < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Ice)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Ice++;
        //    }
        //    if (mMaterialData.Sugar < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Sugar)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Sugar++;
        //    }
        //}
        #endregion
    }
    /// <summary>
    /// 目前所处的主游戏状态
    /// </summary>
    public enum MainState
    { 
        Foreword,//前言
        Game,//做咖啡的游戏时间
        Text,//咖啡结束后的正文时间
        Settle,//结算

        Teach,//养成阶段
        Behaviour,//选择活动阶段
        Dialog,//对话节点
        Change,//切换阶段
    }
}