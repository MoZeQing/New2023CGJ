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
        [SerializeField] private Image[] stars;

        private bool mIsRandom = false;
        private WorkData mWorkData;

        //思考传入什么参数
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mWorkData = (WorkData)BaseFormData.UserData;
            mIsRandom = false;
            mOKButton.onClick.AddListener(OnClick);
            float level = GetPower(mWorkData);
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(i < level);
                if (i < level)
                {
                    stars[i].transform.localScale = Vector3.zero;
                    sequence.Append(stars[i].transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutExpo).OnComplete(()=>
                    {
                        GameEntry.Sound.PlaySound(300+i);
                    }));
                }
            }
            sequence.AppendCallback(ShowSettleData);
            settleCanvas.gameObject.SetActive(true);
        }

        private int GetPower(WorkData workData)
        {
            //剩余时间大于1/3得到3星
            //剩余时间大于1/6得到2星
            //剩余时间大于0或至少完成1单得到1星
            //剩余时间等于且完成没有完成订单得到0星
            int a = (int)(workData.Power * 6);
            if (a > 2)
                return 3;
            if (a > 1)
                return 2;
            if (a > 0 || workData.Income > 0)
                return 1;
            return 0;
        }

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
            Sequence sequence = DOTween.Sequence(); 
            foreach (OrderData order in mWorkData.orderDatas)
            {
                DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)order.NodeTag);
                StringBuilder sb= new StringBuilder();
                sb.Append(dRNode.Description.ToString());
                if (order.Grind) sb.Append("(粗)");
                if (!order.Grind) sb.Append("(细)");
                if (dRNode.Ice) sb.Append("(冰)");
                if (!dRNode.Ice) sb.Append("(热)");
                sequence.Append(coffeeText.DOText(sb.ToString() + "\n", 1f));
            }
            //小猫列表

            //订单总体列表
            int money = (int)(mWorkData.Income*mWorkData.Power+0.33f);
            sequence.Append(DOTween.To(value => { settleText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: money, duration: 0.5f));
            GameEntry.Utils.Money += money;
        }

        private void OnClick()
        {
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Afternoon));
            GameEntry.UI.CloseUIForm(this.UIForm);
            BuffData buffData = GameEntry.Buff.GetBuff();
            GameEntry.Utils.Money += (int)(mWorkData.Income * buffData.MoneyMulti  + buffData.MoneyPlus);
        }
    }
}

