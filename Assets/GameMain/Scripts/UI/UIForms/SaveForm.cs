using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SaveForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }
    }
}

