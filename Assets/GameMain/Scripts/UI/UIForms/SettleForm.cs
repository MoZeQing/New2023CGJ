using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System.Text;

namespace GameMain
{
    public class SettleForm : BaseForm
    {
        [SerializeField] private Transform randomCanvas;
        [SerializeField] private Transform settleCanvas;
        //[SerializeField] private Text randomText;
        [SerializeField] private Text coffeeText;
        [SerializeField] private Text catText;
        [SerializeField] private Text settleText;
        [SerializeField] private Button mOKButton;

        private bool mIsRandom = false;
        private WorkData mWorkData;

        //思考传入什么参数
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mWorkData = (WorkData)BaseFormData.UserData;
            mIsRandom = false;

            settleCanvas.gameObject.SetActive(true);

            mOKButton.onClick.AddListener(OnClick);
            //ShowRandomEvent();
            ShowSettleData();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            mOKButton.onClick.RemoveAllListeners();
        }

        private void ShowRandomEvent()
        {
            RandomEventSO[] randomEvents = Resources.LoadAll<RandomEventSO>("RandomEvent");
            List<RandomEvent> newEvents = new List<RandomEvent>();
            foreach (RandomEventSO random in randomEvents)
            {
                if (GameEntry.Utils.Check(random.randomEvent.trigger))
                {
                    newEvents.Add(random.randomEvent);
                }
            }
            RandomEvent randomEvent = newEvents[Random.Range(0, newEvents.Count - 1)];
            //randomText.text = randomEvent.text;
            mWorkData.RandomEvent = randomEvent;
        }

        private void ShowSettleData()
        {
            //咖啡列表
            coffeeText.text = string.Empty;
            catText.text = string.Empty;
            settleText.text = string.Empty;
            foreach (OrderData order in mWorkData.orderDatas)
            {
                DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)order.NodeTag);
                StringBuilder sb= new StringBuilder();
                sb.Append(dRNode.Description.ToString());
                if (order.Grind) sb.Append("(粗)");
                if (!order.Grind) sb.Append("(细)");
                if (dRNode.Ice) sb.Append("(冰)");
                if (!dRNode.Ice) sb.Append("(热)");
                coffeeText.text += (sb.ToString() + "\n");
            }
            //小猫列表

            //订单总体列表
            settleText.text += string.Format("主营业务收入：{0}\n", mWorkData.Income);
            settleText.text += string.Format("主营业务成本:{0}\n", mWorkData.Cost);
            settleText.text += string.Format("当期净利润:{0}\n", mWorkData.Income - mWorkData.Cost - mWorkData.Administration - mWorkData.Financial);
        }

        private void OnClick()
        {
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Afternoon));
            GameEntry.UI.CloseUIForm(this.UIForm);
            BuffData buffData = GameEntry.Buff.GetBuff();
            DRUpgrade dRUpgrade = GameEntry.DataTable.GetDataTable<DRUpgrade>().GetDataRow(GameEntry.Utils.PlayerData.cafeID);
            GameEntry.Player.Money += (int)(mWorkData.Income * buffData.MoneyMulti * (1 + dRUpgrade.Money / 100) + buffData.MoneyPlus);
        }
    }
}

