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
            cancelBtn.onClick.AddListener(CancelData);
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
                closetImg.sprite = GameEntry.Utils.closet[(int)itemData.itemTag - 101];
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
        }

        public void CancelData()
        {
            closetImg.color = Color.clear;
            infoText.text = string.Empty;
            foreach (ClosetItem item in closetItems)
            {
                item.SetState(ClosetItemState.Idle);
            }
        }
    }
}