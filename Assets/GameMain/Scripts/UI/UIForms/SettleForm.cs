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
        [SerializeField] private Text incomeText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text settleText;
        [SerializeField] private Text catLevelText;
        [SerializeField] private Text catText;
        [SerializeField] private Text buffText;
        [SerializeField] private Button mOKButton;
        [SerializeField] private Image[] stars;

        private bool mIsRandom = false;
        private WorkData mWorkData;
        private int income;
        private int levelMoney;
        private int catValue;
        private int buffValue;

        //思考传入什么参数
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            catLevelText.text = $"猫猫Lv.{GameEntry.Cat.CharmLevel}";
            mWorkData = (WorkData)BaseFormData.UserData;
            mIsRandom = false;
            mOKButton.onClick.AddListener(OnClick);
            float level = GetPower(mWorkData);
            coffeeText.text = string.Empty;
            foreach (OrderData order in mWorkData.orderDatas)
            {
                DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)order.NodeTag);
                StringBuilder sb = new StringBuilder();
                sb.Append(dRNode.Description.ToString());
                if (order.Grind) sb.Append("(粗)");
                if (!order.Grind) sb.Append("(细)");
                if (dRNode.Ice) sb.Append("(冰)");
                if (!dRNode.Ice) sb.Append("(热)");
                coffeeText.text += sb.ToString() + "\n";
            }
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
            float a = workData.Power;
            if (a > 0.333f && workData.orderDatas.Count == workData.OrderCount)
                return 3;
            if (a > 0.166f&&workData.orderDatas.Count==workData.OrderCount)
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

        private void ShowSettleData()
        {
            //咖啡列表

            levelText.text = string.Empty;
            Sequence sequence = DOTween.Sequence(); 

            //小猫列表

            //订单总体列表
            income = (int)mWorkData.Income;
            levelMoney = (int)(mWorkData.Money * (float)GetPower(mWorkData) / 3f);
            sequence.Append(DOTween.To(value => { incomeText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: income, duration: 0.5f));
            sequence.Append(DOTween.To(value => { levelText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: levelMoney, duration: 0.5f));
            catValue = (int)((levelMoney + income) * (((float)GameEntry.Cat.CharmLevel - 1f) / 3f));
            sequence.Append(DOTween.To(value => { catText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: catValue, duration: 0.5f));
            BuffData buffData = GameEntry.Buff.GetBuff();
            sequence.Append(DOTween.To(value => { settleText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: (int)((levelMoney + income + catValue) * buffData.MoneyMulti + buffData.MoneyPlus), duration: 0.5f));
        }

        private void OnClick()
        {
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Afternoon));
            GameEntry.UI.CloseUIForm(this.UIForm);
            BuffData buffData = GameEntry.Buff.GetBuff();
            GameEntry.Player.Money += (int)((levelMoney + income+catValue) * buffData.MoneyMulti  + buffData.MoneyPlus);
        }
    }
}

