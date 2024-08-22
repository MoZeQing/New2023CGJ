using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GuideComponent : GameFrameworkComponent
    {
        private bool flag;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            { 
                flag=!flag;
                if(!flag)
                    GameEntry.UI.OpenUIForm(UIFormId.ConsoleForm);
                else
                    GameEntry.UI.CloseUIForm(UIFormId.ConsoleForm);
            }


            if (GameEntry.Player.GuideId<=3)
            {
                //养成1
                if (GameEntry.Utils.GameState == GameState.Night&&
                    GameEntry.Player.Day==2)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 3);
                    GameEntry.Player.GuideId = 4;
                }
            }
            if (GameEntry.Player.GuideId == 4)
            {
                //工作
                if (GameEntry.Utils.GameState == GameState.Special &&
                    GameEntry.Player.Day == 3)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 4);
                    GameEntry.Player.GuideId = 5;
                }
            }
            if (GameEntry.Player.GuideId <= 5)
            {
                //外出
                if (GameEntry.Utils.GameState == GameState.Night &&
                    GameEntry.Player.Day == 4)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 5);
                    GameEntry.Player.GuideId = 6;
                }
            }
            if (GameEntry.Player.GuideId == 6)
            {
                //外出后
                if (GameEntry.Utils.GameState == GameState.Night &&
                    GameEntry.Utils.Location==OutingSceneState.Clothing) 
                {
                    GameEntry.Player.GuideId = 7;
                }
            }
            if (GameEntry.Player.GuideId == 7)
            {
                //外出的小游戏
                if (GameEntry.Utils.GameState == GameState.Night&&
                    (GameEntry.Utils.Location == OutingSceneState.Gym||
                    GameEntry.Utils.Location == OutingSceneState.Beach ||
                    GameEntry.Utils.Location == OutingSceneState.Library))
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 6);
                    GameEntry.Player.GuideId = 8;
                }
            }
        }
    }
}

