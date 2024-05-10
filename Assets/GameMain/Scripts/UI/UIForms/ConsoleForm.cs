using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ConsoleForm : UIFormLogic
    {
        [SerializeField] private Text text;
        [SerializeField] private Button btn;
        [SerializeField] private Text inputText;
        // Start is called before the first frame update
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            btn.onClick.AddListener(OnClick);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            btn.onClick.RemoveAllListeners();
        }
        // Update is called once per frame
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClick();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEntry.UI.CloseUIForm(this.UIForm);
            }
        }
        public void OnClick()
        {
            ConsoleUpdate();
        }

        public void ConsoleUpdate()
        {
            if (inputText.text == string.Empty)
            {
                text.text += "«Î ‰»Î√¸¡Ó\n";
            }
            text.text += inputText.text + "\n";
            if (inputText.text == "Clear")
            {
                text.text = string.Empty;
            }
            text.text += GameEntry.Utils.RunEvent(inputText.text) ? "\ncommand is success\n" : "command is fail\n";
            inputText.text = string.Empty;
        }
    }
}
