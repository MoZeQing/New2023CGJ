using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace GameMain
{
    public class ClosetForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button cancelBtn;
        [SerializeField] private Text infoText;
        [SerializeField] private Image closetImg;

        [SerializeField] private List<ClosetItem> closetItems = new List<ClosetItem>();

        private ItemData mItemData;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            okBtn.onClick.AddListener(OkBtn);
            cancelBtn.onClick.AddListener(CancelData);
            cancelBtn.interactable = false;
            okBtn.interactable = false;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
            okBtn.onClick.RemoveAllListeners();
        }

        public void SetData(ItemData itemData)
        {
            if (itemData == null)
            {
                closetImg.color = Color.clear;
                infoText.text = string.Empty;
            }
            else
            {
                closetImg.color = Color.white;
                closetImg.sprite = GameEntry.Utils.closets[(int)itemData.itemTag - 101];
                infoText.text = itemData.itemInfo;
            }
        }

        public void SetData(ItemData itemData,ClosetItem closetItem)
        {
            mItemData = itemData;
            foreach (ClosetItem item in closetItems)
            {
                if (item != closetItem)
                {
                    item.SetState(ClosetItemState.Freeze);
                }
            }
            cancelBtn.interactable = true;
            okBtn.interactable = true;
        }

        public void CancelData()
        {
            closetImg.color = Color.clear;
            infoText.text = string.Empty;
            foreach (ClosetItem item in closetItems)
            {
                item.SetState(ClosetItemState.Idle);
            }
            cancelBtn.interactable = false;
            okBtn.interactable = false;
        }

        public void OkBtn()
        {
            if (mItemData == null)
                return;
            GameEntry.Utils.closet = (int)mItemData.itemTag;
            CancelData();
            foreach (ClosetItem item in closetItems)
            {
                item.Check();
            }
        }
    }
}