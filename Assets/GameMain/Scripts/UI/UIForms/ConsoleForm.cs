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
        void Update()
        {
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
            EventData eventData = new EventData();
            if (inputText.text == "ShowFlag")
            {
                text.text += "\n";
                foreach (KeyValuePair<string, int> pair in GameEntry.Utils._flagValues)
                {
                    text.text += string.Format("{0}:{1}\n", pair.Key, pair.Value);
                }
                return;
            }
            if (inputText.text == string.Empty)
            {
                text.text += "«Î ‰»Î√¸¡Ó\n";
            }
            else if (inputText.text == "Clear")
            {
                text.text = string.Empty;
            }
            else
            {
                text.text += inputText.text + "\n";
            }


            string[] console = inputText.text.Split(' ');
            if (console.Length != 2)
            {
                text.text += "¥ÌŒÛ£¨«Î ‰≥ˆ”––ßµƒ√¸¡Ó";
                return;
            }

            inputText.text = string.Empty;

            try
            {
                EventTag eventEnum = (EventTag)Enum.Parse(typeof(EventTag), console[0]);
                eventData.eventTag = eventEnum;
                eventData.value = console[1];
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            Debug.LogFormat("≤‚ ‘£¨√∂æŸ£∫{0}£¨÷µ£∫{1}", console[0], console[1]);
            text.text += GameEntry.Utils.RunEvent(eventData) ? "command is success\n" : "command is fail\n";
        }
    }
}
