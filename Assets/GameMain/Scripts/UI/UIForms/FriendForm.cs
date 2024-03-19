using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class FriendForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;

        [SerializeField] private List<Image> images= new List<Image>();
        [SerializeField] private List<Text> texts= new List<Text>();
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text text;

        private List<KeyValuePair<string, int>> friends = new List<KeyValuePair<string, int>>();
        private int index = 0;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            index = 0;
            text.text = 1.ToString();
            leftBtn.interactable = false;
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            friends.Clear();
            foreach (KeyValuePair<string, int> pair in GameEntry.Utils.GetFriends())
            {
                friends.Add(pair);
            }
            for (int i = 0; i < images.Count; i++)
            {
                if (index >= friends.Count)
                {
                    index = 0;
                    break;
                }
                images[i].sprite = GameEntry.Utils.chars[friends[index].Key].sprite;
                texts[i].text = friends[index].Value.ToString(); ;
                index++;
            }
            if (GameEntry.Utils.GetFriends().Count <= images.Count)
            {
                rightBtn.interactable = false;
            }
            else
            {
                rightBtn.interactable = true;
            }
        }

        private void Left()
        {
            rightBtn.interactable = true;
            index -= images.Count*2;
            if (index - images.Count < 0)
                leftBtn.interactable = false;
            for (int i = 0; i < images.Count; i++)
            {
                if (index >= friends.Count)
                {
                    index = 0;
                    break;
                }
                images[i].sprite = GameEntry.Utils.chars[friends[index].Key].sprite;
                texts[i].text = friends[index].Value.ToString(); ;
                index++;
            }
            text.text = (index / images.Count).ToString();
        }

        private void Right()
        {
            leftBtn.interactable = true;
            if (index + images.Count >= GameEntry.Utils.GetFriends().Count)
                rightBtn.interactable = false;
            for (int i = 0; i < images.Count; i++)
            {
                if (index >= friends.Count)
                {
                    index = 0;
                    break;
                }
                images[i].sprite = GameEntry.Utils.chars[friends[index].Key].sprite;
                texts[i].text = friends[index].Value.ToString(); ;
                index++;
            }
            text.text = (index / images.Count).ToString();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }
    }
}
