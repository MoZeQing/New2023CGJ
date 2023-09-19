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
        [SerializeField] private Text infoText;
        [SerializeField] private Image closetImg;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
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
    }
}