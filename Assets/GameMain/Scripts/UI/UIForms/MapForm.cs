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
        private int mDay = 0;

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
            mGreengrocerBtn.onClick.RemoveAllListeners();
            mGlassBtn.onClick.RemoveAllListeners();
            mRestaurantBtn.onClick.RemoveAllListeners();
            mBeachBtn.onClick.RemoveAllListeners();
            mClothingBtn.onClick.RemoveAllListeners();
            mGymBtn.onClick.RemoveAllListeners();
        }

        private void Outing(OutingSceneState outingSceneState)
        {
            if (mDay == GameEntry.Utils.Day)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "今天你已经外出过一次了");
                return;
            }
            if (outingSceneState==OutingSceneState.Beach&&GameEntry.Utils.GetPlayerItem(ItemTag.Closet4) == null)//检查泳装
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有泳装，请购买泳装才能去往海滩");
                return;
            }
            if (outingSceneState == OutingSceneState.Gym && GameEntry.Utils.GetPlayerItem(ItemTag.Closet2) == null)//检查运动装
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有运动服，请购买运动服才能去往健身房");
                return;
            }
            GameEntry.Utils.outSceneState=outingSceneState;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm((UIFormId)outingSceneState + 20, this);
            mDay = GameEntry.Utils.Day;
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
