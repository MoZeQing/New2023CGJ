using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CupboradForm : BaseForm
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text pageText;
        [SerializeField] private Image itemImage;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;
        [SerializeField] private List<Item> mItems = new List<Item>();

        private List<DRItem> dRItems = new List<DRItem>();
        private DRItem mItemData = new DRItem();
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind == ItemKind.Clothes)
                    continue;
                if ((ItemKind)item.Kind == ItemKind.Instrument)
                    continue;
                if (GameEntry.Player.GetPlayerItem((ItemTag)item.Id)==null)
                    continue;
                dRItems.Add(item);
            }

            exitBtn.onClick.AddListener(OnExit);
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            index = 0;
            ShowItems();

            leftBtn.interactable = false;
            rightBtn.interactable = dRItems.Count > mItems.Count;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }

        private void ShowItems()
        {
            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                {
                    mItems[i].SetData(dRItems[index]);
                    mItems[i].SetTouch(OnTouch);
                }
                else
                    mItems[i].Hide();
                index++;
            }
            leftBtn.interactable = (index != 0);
            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
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
        private void OnTouch(bool flag, DRItem itemData)
        {
            if (flag)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.sprite = Resources.Load<Sprite>(itemData.IconPath);
                headerField.text = itemData.Name;
                contentField.text = itemData.Info;
            }
            else
            {
                itemImage.gameObject.SetActive(false);
                headerField.text = string.Empty;
                contentField.text = string.Empty;
            }
        }

        private void OnExit()
        {
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}