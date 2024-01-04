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
            mBeachBtn.gameObject.SetActive(false);
            mClothingBtn.gameObject.SetActive(false);
            mGlassBtn.gameObject.SetActive(false);
            mGreengrocerBtn.gameObject.SetActive(false);
            mGymBtn.gameObject.SetActive(false);
            mRestaurantBtn.gameObject.SetActive(false);
            foreach (OutingSceneState outingSceneState in GameEntry.Utils.outingSceneStates)
            {
                switch (outingSceneState)
                {
                    case OutingSceneState.Beach:
                        mBeachBtn.gameObject.SetActive(true);
                        break;
                    case OutingSceneState.Clothing:
                        mClothingBtn.gameObject.SetActive(true);
                        break;
                    case OutingSceneState.Glass:
                        mGlassBtn.gameObject.SetActive(true);
                        break;
                    case OutingSceneState.Greengrocer:
                        mGreengrocerBtn.gameObject.SetActive(true);
                        break;
                    case OutingSceneState.Gym:
                        mGymBtn.gameObject.SetActive(true);
                        break;
                    case OutingSceneState.Restaurant:
                        mRestaurantBtn.gameObject.SetActive(true);
                        break;
                }
            }

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
            if (GameEntry.Utils.Energy < 20)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的体力不足，还是先休息会吧");
                return;
            }
            GameEntry.Utils.Energy -= 20;
            GameEntry.Utils.outSceneState=outingSceneState;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm((UIFormId)outingSceneState + 20, this);
            mDay = GameEntry.Utils.Day;
        }
    }
}
