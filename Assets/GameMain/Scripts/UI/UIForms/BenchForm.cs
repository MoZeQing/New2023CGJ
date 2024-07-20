using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class BenchForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button trainBtn;
        [SerializeField] private Button matchBtn;
        [SerializeField] private Button quickBtn;
        [SerializeField] private Button gameBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private CharData charData;
        [SerializeField] private PlayerData playerData;
        //1-3成功的剧情、4-5失败的剧情
        [SerializeField] private List<DialogueGraph> matchStories = new List<DialogueGraph>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.gameObject.SetActive(false);
            trainBtn.onClick.AddListener(QuickBtn_Click);
            matchBtn.onClick.AddListener(GameBtn_Click);
            exitBtn.onClick.AddListener(OnExit);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            trainBtn.interactable = GameEntry.Utils.Money >= 200;
            matchBtn.interactable = GameEntry.Utils.Money >= 200;
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            trainBtn.onClick.RemoveAllListeners();
            matchBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void QuickBtn_Click()
        {
            Dictionary<ValueTag, int> dic = new Dictionary<ValueTag, int>();
            charData.GetValueTag(dic);
            playerData.GetValueTag(dic);
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm1, OnExit, dic);
        }

        private void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.FlopForm, OnExit);
        }

        private void TrainBtn_Click()
        {
            canvas.gameObject.SetActive(true);

            quickBtn.transform.localPosition = Vector3.down * 30;
            quickBtn.GetComponent<Image>().color = Color.gray;
            quickBtn.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.InOutExpo);
            quickBtn.GetComponent<Image>().DOColor(Color.white, 0.3f);

            gameBtn.transform.localPosition = Vector3.down * 30;
            gameBtn.GetComponent<Image>().color = Color.gray;
            gameBtn.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.InOutExpo);
            gameBtn.GetComponent<Image>().DOColor(Color.white, 0.3f);
        }

        //private void MatchBtn_Click()
        //{
        //    switch (GameEntry.Utils.CharData.CharmLevel)
        //    {
        //        case 1:
        //            if (GameEntry.Utils.CharData.charm >= 30)
        //            {
        //                GameEntry.Dialog.PlayStory(matchStories[1]);
        //                GameEntry.Dialog.SetComplete(OnExit);
        //                GameEntry.Utils.CharData.CharmLevel++;
        //            }
        //            else
        //            {
        //                GameEntry.Dialog.PlayStory(matchStories[4]);
        //                GameEntry.Dialog.SetComplete(OnExit);
        //            }
        //            break;
        //        case 2:
        //            if (GameEntry.Utils.CharData.charm >= 80)
        //            {
        //                GameEntry.Dialog.PlayStory(matchStories[2]);
        //                GameEntry.Dialog.SetComplete(OnExit);
        //                GameEntry.Utils.CharData.CharmLevel++;
        //            }
        //            else
        //            {
        //                GameEntry.Dialog.PlayStory(matchStories[5]);
        //                GameEntry.Dialog.SetComplete(OnExit);
        //            }
        //            break;
        //        case 3:
        //            GameEntry.Dialog.PlayStory(matchStories[3]);
        //            GameEntry.Dialog.SetComplete(OnExit);
        //            break;
        //    }
        //}

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
            GameEntry.Event.FireNow(this, OutEventArgs.Create(OutingSceneState.Home));
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
        }
    }
}
