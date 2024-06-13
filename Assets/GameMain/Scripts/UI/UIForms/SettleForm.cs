using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System.Text;
using DG.Tweening;

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
        [SerializeField] private Text mLevelText;
        [SerializeField] private Image progressImg;

        [SerializeField] private List<Image> starImages=new List<Image>();

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
            //CheckLevel();
            ShowSettleData();
        }

        //private void CheckLevel()
        //{

        //}

        private void OnEnable()
        {

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

            float totalTime = 0f;
            float playerTotalTime = 0f;
            int orderValue = 0;
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
                totalTime += order.OrderTime;
                playerTotalTime += order.PlayerTime;
                orderValue += order.orderValue;
            }
            //结算列表
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < starImages.Count; i++)
            {
                starImages[i].gameObject.SetActive(playerTotalTime  > totalTime * i);
                if (playerTotalTime <= totalTime * i) continue;
                starImages[i].transform.localScale = Vector3.one * 2f;
                sequence.Append(starImages[i].transform.DOScale(Vector3.one, 1f));
            }
            DRLevel level = GameEntry.DataTable.GetDataTable<DRLevel>().GetDataRow(GameEntry.Utils.Level);
            progressImg.fillAmount = (float)(GameEntry.Utils.OrderValue- orderValue) / (float)level.EXP;
            sequence.Append(progressImg.DOFillAmount((float)GameEntry.Utils.OrderValue / (float)level.EXP, 1f).SetEase(Ease.InOutExpo));
            if (GameEntry.Utils.OrderValue >= level.EXP)
            {
                GameEntry.Utils.Level = level.UpgradeID;
                DRLevel newLevel = GameEntry.DataTable.GetDataTable<DRLevel>().GetDataRow(GameEntry.Utils.Level);
                sequence.AppendCallback(() =>
                {
                    progressImg.fillAmount = 0f;
                    progressImg.DOFillAmount((float)GameEntry.Utils.OrderValue / (float)newLevel.EXP, 1f).SetEase(Ease.InOutExpo);
                    mLevelText.text= newLevel.TagIcon.ToString();
                });
            }
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
            GameEntry.Utils.Money += (int)(mWorkData.Income * buffData.MoneyMulti * (1 + dRUpgrade.Money / 100) + buffData.MoneyPlus);
        }
    }
}

