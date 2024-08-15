using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using GameFramework.DataTable;
using GameMain;


namespace GameMain
{
    public class ClothingForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text pageText;
        [SerializeField] private List<ShopItem> mItems = new List<ShopItem>();

        private List<DRItem> dRItems = new List<DRItem>();
        private DRItem dRItem;
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Clothes)
                    continue;
                if (item.Id == 1001)
                    continue;
                if (item.Id == 1002)
                    continue;
                if (item.Id == 1006)
                    continue;
                dRItems.Add(item);
            }

            leftBtn.interactable = false;
            rightBtn.interactable = dRItems.Count > mItems.Count;

            exitBtn.onClick.AddListener(OnExit);
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            index = 0;
            ShowItems();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }
        private void ShowItems()
        {
            leftBtn.interactable = (index != 0);

            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                {
                    mItems[i].SetData(dRItems[index]);
                    mItems[i].SetClick(OnClick);
                }
                else
                    mItems[i].Hide();
                index++;
            }

            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
        }
        private void OnClick(DRItem itemData)
        {
            dRItem = itemData;
            if (itemData.Price > GameEntry.Player.Money)
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的资金不足");
            else
                GameEntry.UI.OpenUIForm(UIFormId.OkTips,UpdateItem, "你确定要购买吗？");
        }
        private void Right()
        {
            ShowItems();
        }

        private void Left()
        {
            index -= 2 * mItems.Count;
            ShowItems();
        }
        private void UpdateItem()
        {
            GameEntry.Player.Money -= dRItem.Price;
            GameEntry.Utils.AddPlayerItem(new ItemData(dRItem), 1);
            if (dRItem.EventData != null&&dRItem.EventData!=string.Empty)
            {
                GameEntry.Utils.RunEvent(dRItem.EventData);
            }

            index -= mItems.Count;
            ShowItems();
        }
        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
            GameEntry.Event.FireNow(this, OutEventArgs.Create(OutingSceneState.Home));
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
        }
    }
}
