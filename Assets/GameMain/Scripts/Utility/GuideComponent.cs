using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GuideComponent : GameFrameworkComponent
    {

        private void Update()
        {
            if (GameEntry.Utils.PlayerData.guideID<=3)
            {
                //养成1
                if (GameEntry.Utils.GameState == GameState.Night&&
                    GameEntry.Utils.Day==2)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 3);
                    GameEntry.Utils.PlayerData.guideID = 4;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID == 4)
            {
                //工作
                if (GameEntry.Utils.GameState == GameState.Special &&
                    GameEntry.Utils.Day == 3)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 4);
                    GameEntry.Utils.PlayerData.guideID = 5;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID <= 5)
            {
                //外出
                if (GameEntry.Utils.GameState == GameState.Night &&
                    GameEntry.Utils.Day == 3)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 5);
                    GameEntry.Utils.PlayerData.guideID = 6;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID == 6)
            {
                //外出后
                if (GameEntry.Utils.GameState == GameState.Night &&
                    GameEntry.Utils.Location==OutingSceneState.Clothing) 
                {
                    GameEntry.Utils.PlayerData.guideID = 7;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID >= 7)
            {
                //外出的小游戏
                if (GameEntry.Utils.GameState == GameState.Night&&
                    (GameEntry.Utils.Location == OutingSceneState.Gym||
                    GameEntry.Utils.Location == OutingSceneState.Beach ||
                    GameEntry.Utils.Location == OutingSceneState.Library))
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 6);
                    GameEntry.Utils.PlayerData.guideID = 8;
                }
            }
        }
    }
}

