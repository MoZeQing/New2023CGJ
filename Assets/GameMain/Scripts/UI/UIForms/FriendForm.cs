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

        [SerializeField] private Image friendIcon;
        [SerializeField] private Text friendFavorText;
        [SerializeField] private Text friendNameText;
        [SerializeField] private Text friendTextText;
        [SerializeField] private Image coffeeIcon;
        //[SerializeField] private Image progress;
        //[SerializeField] private List<Text> progressTexts;

        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text text;

        private List<string> friends = new List<string>();
        private int index = 0;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            index = 0;
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            friends.Clear();
            //初始化
            foreach (KeyValuePair<string, int> pair in GameEntry.Utils.GetFriends())
            {
                friends.Add(pair.Key);
            }
            rightBtn.interactable = GameEntry.Utils.GetFriends().Count > index;
            leftBtn.interactable = false;
            text.text = 1.ToString();

            ShowData(index);
        }

        private void ShowData(int index)
        {
            string charName = friends[index];
            CharSO charSO = GameEntry.Utils.chars[charName];
            //展示
            friendIcon.sprite = charSO.sprite;
            friendNameText.text = charSO.charName;
            DRNode dRNode =GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)charSO.favorCoffee);
            friendTextText.text = charSO.text;
            coffeeIcon.sprite = Resources.Load<Sprite>(dRNode.MaterialPath);
            //progress.fillAmount = friendFavor / 100f;

        }

        private void Left()
        {
            index--;
            rightBtn.interactable = true;
            if(index==0)
                leftBtn.interactable = false;
            text.text = (index + 1).ToString();

            ShowData(index);
        }

        private void Right()
        {
            index++;
            if(index==friends.Count-1)
                rightBtn.interactable = false;
            leftBtn.interactable = true;
            text.text = (index + 1).ToString();

            ShowData(index);
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
