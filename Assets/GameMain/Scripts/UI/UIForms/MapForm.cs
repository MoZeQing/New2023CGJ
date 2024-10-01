using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class MapForm : BaseForm
    {
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Button clothingBtn;
        [SerializeField] private Button libraryBtn;
        [SerializeField] private Button gymBtn;
        [SerializeField] private Button marketBtn;
        [SerializeField] private Button benchBtn;
        [SerializeField] private Button restaurantBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button saveLoadBtn;
        [SerializeField] private Transform canvas;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(OnExit);

            libraryBtn.onClick.AddListener(() => Outing(OutingSceneState.Library));
            clothingBtn.onClick.AddListener(() => Outing(OutingSceneState.Clothing));
            gymBtn.onClick.AddListener(() => Outing(OutingSceneState.Gym));
            benchBtn.onClick.AddListener(() => Outing(OutingSceneState.Beach));
            marketBtn.onClick.AddListener(() => Outing(OutingSceneState.Market));
            //restaurantBtn.onClick.AddListener(() => Outing(OutingSceneState.Restaurant));
            saveLoadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveLoadForm, this));
            if (GameEntry.Player.GuideId == 6)
            {
                libraryBtn.gameObject.SetActive(false);
                benchBtn.gameObject.SetActive(false);
                gymBtn.gameObject.SetActive(false);
            }
            else
            {
                libraryBtn.gameObject.SetActive(true);
                benchBtn.gameObject.SetActive(true);
                gymBtn.gameObject.SetActive(true);
            }

            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);    
        }
        protected override void OnStart(object userData)
        {
            base.OnStart(userData);
            BackgroundUpdate();
        }
        private void OnPlayerDataEvent(object sender, GameEventArgs e)
        {
            BackgroundUpdate();
        }

        protected virtual bool BackgroundUpdate()
        {
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            if (GameEntry.Utils.WeatherTag == WeatherTag.Afternoon)
                backgroundImg.sprite = Resources.Load<Sprite>("Dialog/Background/MapForm_Afternoon");
            else
                backgroundImg.sprite = Resources.Load<Sprite>("Dialog/Background/MapForm_Night");
            return true;
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            libraryBtn.onClick.RemoveAllListeners();
            clothingBtn.onClick.RemoveAllListeners();
            gymBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
            benchBtn.onClick.RemoveAllListeners(); 
            marketBtn.onClick.RemoveAllListeners();
            //restaurantBtn.onClick.RemoveAllListeners();
            saveLoadBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
        }
        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
        private void Outing(OutingSceneState outingSceneState)
        {
            if (GameEntry.Player.Ap <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的体力已经耗尽，该去睡觉了！");
                return;
            }

            GameEntry.Utils.Location=outingSceneState;
            GameEntry.Dialog.StoryUpdate();
            if (GameEntry.Player.GuideId == 6)
                return;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm((UIFormId)outingSceneState + 20, this);
            GameEntry.Event.FireNow(this, OutEventArgs.Create(outingSceneState));
        }
    }
}
