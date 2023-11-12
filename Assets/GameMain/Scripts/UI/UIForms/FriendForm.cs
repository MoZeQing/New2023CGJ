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
        public List<Text> names= new List<Text>();
        public List<Text> texts= new List<Text>();
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            for (int i = 0; i < GameEntry.Utils.friends.Count; i++)
            {
                images[i].sprite = GameEntry.Utils.friends[i].sprite;
                names[i].text = GameEntry.Utils.friends[i].charName;
                texts[i].text = GameEntry.Utils.friends[i].favor.ToString();
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }
    }
}
