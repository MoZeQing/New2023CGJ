using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;

namespace GameMain
{
    public class MarketForm : ShopForm
    {
        [Header("购买区域的东西")]
        [SerializeField] private ItemKind kind;
        [Header("投资金额")]
        [SerializeField] private int invest = 500;
        [Header("投资区域")]
        [SerializeField] private Button investBtn;
        [SerializeField] private Text investText;
        [SerializeField] private Text financialText;

        protected override void OnInitValue(object userData)
        {
            base.OnInitValue(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != kind)
                    continue;
                dRItems.Add(item);
            }
            investBtn.onClick.AddListener(InvestBtn_OnClick);
        }
        private void InvestBtn_OnClick()
        {
            if (GameEntry.Player.Money < invest)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, $"你的金钱少于{invest}");
                return;
            }
            GameEntry.UI.OpenUIForm(UIFormId.OkTips, InvestBtn_OnConfirm, "你确定要投资吗？");
        }
        protected override void OnConfirm(DRItem itemData)
        {
            base.OnConfirm(itemData);
        }
        private void InvestBtn_OnConfirm()
        {
            GameEntry.Player.Investment += invest;
            GameEntry.Player.Money -= invest;
            GameEntry.Utils.AddFlag("Invest");
            UpdateItem();
        }
        protected override void UpdateItem()
        {
            financialText.text = $"投资额：{GameEntry.Player.Investment}";
            investText.text = $"投资回报（每天）：{GameEntry.Player.Investment / invest + 9}%";
            investBtn.interactable = !GameEntry.Utils.CheckFlag("Invest");

            base.UpdateItem();
        }
    }
}

