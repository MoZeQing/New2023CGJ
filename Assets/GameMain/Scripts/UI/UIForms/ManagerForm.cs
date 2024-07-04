using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ManagerForm : BaseForm
    {
        [Header("结算面板")]
        [SerializeField] private Transform canvas;
        [SerializeField] private Text eventEffect1;
        [SerializeField] private Text eventEffect2;
        [SerializeField] private Text settleMoneyText;
        [SerializeField] private Text settleClientText;
        [SerializeField] private List<SettleItem> settleItems;
        [Header("主界面")]
        [SerializeField] private Text moneyText;
        [SerializeField] private Text clientText;
        [SerializeField] private BubbleItem[] bubbles;
        [Header("控制数值")]
        //[SerializeField,Range(0f,2f)] private float speed=1;
        [SerializeField] private float rate=5;
        [SerializeField] private bool isHideItem;
        [SerializeField,Range(0,5)] private float itemLifeTime;
        private float time;
        private float totalTime;
        private int money;
        private int client;
        private ManagerData managerData;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            for (int i = 0; i < settleItems.Count; i++)
            {
                settleItems[i].Hide();
            }

            managerData = (ManagerData)BaseFormData.UserData;
            canvas.localPosition = Vector3.up * 1080f;
            canvas.gameObject.SetActive(false);
            time = 1/rate;
            for (int i = 0; i < bubbles.Length; i++)
            {
                bubbles[i].gameObject.SetActive(false);
            }
            time = rate;
            totalTime = 10;
            DOTween.To(value => { moneyText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: managerData.GetTotalClient(), duration: 10);
            DOTween.To(value => { clientText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: managerData.GetTotalMoney(), duration: 10);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            totalTime -= Time.deltaTime;
            if (totalTime < 0)
                return;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = rate;
                //随意写的，蠢得一比
                int block = 50;
                while (true)
                {
                    block--;
                    int index = Random.Range(0, bubbles.Length);
                    if (!bubbles[index].gameObject.activeSelf)
                    {
                        bubbles[index].SetData(Random.Range(1, 6), "测试", itemLifeTime, isHideItem);
                        break;
                    }
                    if (block < 0)
                        break;
                }
            }
        }

        public void CloseForm()
        {
            canvas.gameObject.SetActive(true);

            eventEffect1.text = string.Empty;
            DRDaily drDaily = GameEntry.DataTable.GetDataTable<DRDaily>().GetDataRow(managerData.Daily);
            string[] dailyEventEffectTags = drDaily.EventEffect.Split('-');
            eventEffect1.text += $"【{drDaily.Name}】：{drDaily.Text}\n";
            for (int i = 0; i < dailyEventEffectTags.Length; i++)
            {
                int result = 0;
                if (!int.TryParse(dailyEventEffectTags[i], out result))
                {
                    Debug.LogError($"错误，随机的日常的事件数据错误，请检查Daily表中的{dailyEventEffectTags[i]}");
                    return;
                }
                DREventEffect dREventEffect = GameEntry.DataTable.GetDataTable<DREventEffect>().GetDataRow(result);
                eventEffect1.text += $"{dREventEffect.Text}\n";
            }

            eventEffect2.text = string.Empty;
            for (int i = 0; i < managerData.Combinations.Count; i++)
            {
                DRCombination dRCombination = GameEntry.DataTable.GetDataTable<DRCombination>().GetDataRow(managerData.Combinations[i]);
                string[] combinationsEventEffectTags = dRCombination.EventEffect.Split('-');
                eventEffect2.text += $"【{dRCombination.Name}】：{dRCombination.Text}\n";
                for (int j = 0; j < combinationsEventEffectTags.Length; j++)
                {
                    int result = 0;
                    if (!int.TryParse(combinationsEventEffectTags[i], out result))
                    {
                        Debug.LogError($"错误，随机的日常的事件数据错误，请检查Combination表中的{combinationsEventEffectTags[i]}");
                        return;
                    }
                    DREventEffect dREventEffect = GameEntry.DataTable.GetDataTable<DREventEffect>().GetDataRow(result);
                    eventEffect2.text += $"{dREventEffect.Text}\n";
                }
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo));
            sequence.Append(DOTween.To(value => { settleClientText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: managerData.GetTotalClient(), duration: 1f));
            sequence.Append(DOTween.To(value => { settleMoneyText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: managerData.GetTotalMoney(), duration: 1f));
            sequence.AppendCallback(() =>
            {
                List<CoffeeData> coffeeDatas = new List<CoffeeData>(managerData.MapCoffees.Values);
                for (int i = 0; i < settleItems.Count; i++)
                {
                    if (i < coffeeDatas.Count)
                        settleItems[i].SetData(coffeeDatas[i], managerData.GetClient(coffeeDatas[i]));
                    else
                        settleItems[i].Hide();
                }
            });
        }
    }
}
