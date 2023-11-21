using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class FriendForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;

        public List<Image> images= new List<Image>();
        public List<Text> texts= new List<Text>();
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            int index = 0;
            foreach (KeyValuePair<string, int> pair in GameEntry.Utils.friends)
            {
                images[index].sprite = GameEntry.Utils.chars[pair.Key].sprite;
                texts[index].text = string.Format("¹ØÏµ£º{0}",pair.Value.ToString());
                index++;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }
    }
}
