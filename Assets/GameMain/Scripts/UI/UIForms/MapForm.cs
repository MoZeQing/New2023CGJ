using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class MapForm : MonoBehaviour
    {
        [SerializeField] private Button mGreengrocerBtn;
        [SerializeField] private Button mGlassBtn;
        [SerializeField] private Button mClothingBtn;
        [SerializeField] private Button mRestaurantBtn;
        [SerializeField] private Button mBeachBtn;
        [SerializeField] private Button mGymBtn;

        private MainState mMainState;

        private void OnEnable()
        {
            mGreengrocerBtn.onClick.AddListener(() => Outing(OutingSceneState.Greengrocer));
            mGlassBtn.onClick.AddListener(() => Outing(OutingSceneState.Glass));
            mRestaurantBtn.onClick.AddListener(() => Outing(OutingSceneState.Restaurant));
            mBeachBtn.onClick.AddListener(() => Outing(OutingSceneState.Beach));
            mClothingBtn.onClick.AddListener(() => Outing(OutingSceneState.Clothing));
            mGymBtn.onClick.AddListener(() => Outing(OutingSceneState.Gym));
        }

        private void OnDisable()
        {
            mGreengrocerBtn.onClick.RemoveAllListeners(); ;
            mGlassBtn.onClick.RemoveAllListeners();
            mRestaurantBtn.onClick.RemoveAllListeners();
            mBeachBtn.onClick.RemoveAllListeners();
            mClothingBtn.onClick.RemoveAllListeners();
            mGymBtn.onClick.RemoveAllListeners();
        }

        private void Outing(OutingSceneState outingSceneState)
        {
            if (outingSceneState==OutingSceneState.Beach&&GameEntry.Utils.GetPlayerItem(ItemTag.Closet3) == null)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有泳装，请购买泳装才能去往海滩");
                return;
            }
            GameEntry.Utils.outSceneState=outingSceneState;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm((UIFormId)outingSceneState + 20, this);
        }

        //protected override void OnOpen(object userData)
        //{
        //    base.OnOpen(userData);
        //    mMainState = (MainState)userData;

        //    mGreengrocerBtn.onClick.AddListener(() => Outing(OutingSceneState.Greengrocer));
        //    mGlassBtn.onClick.AddListener(() => Outing(OutingSceneState.Glass));
        //    mRestaurantBtn.onClick.AddListener(() => Outing(OutingSceneState.Restaurant));
        //    mBeachBtn.onClick.AddListener(() => Outing(OutingSceneState.Beach));
        //    mClothingBtn.onClick.AddListener(() => Outing(OutingSceneState.Park));
        //    mBookstoreBtn.onClick.AddListener(() => Outing(OutingSceneState.BookStore));
        //}

        //protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        //{
        //    base.OnUpdate(elapseSeconds, realElapseSeconds);
        //}

        //protected override void OnClose(bool isShutdown, object userData)
        //{
        //    base.OnClose(isShutdown, userData);
        //}
    }
}
