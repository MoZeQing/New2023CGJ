using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OptionForm : UIFormLogic
    {
        [SerializeField] private Button exit;
        [SerializeField] private Button main;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exit.onClick.AddListener(()=> GameEntry.UI.CloseUIForm(this.UIForm));
            main.onClick.AddListener(() => GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Menu)));
    }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exit.onClick.RemoveAllListeners();
            main.onClick.RemoveAllListeners();
        }
    }
}

