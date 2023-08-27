using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CupboradForm : UIFormLogic
    {
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Button exitBtn;
        [SerializeField] private GameObject itemItem;
        [SerializeField] private Text nameText;
        [SerializeField] private Text textText;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        public void OnExit()
        {
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}