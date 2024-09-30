using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GymForm : OutForm
    {
        protected override void QuickBtn_Click()
        {
            GameEntry.Player.Ap -= valueData.ap;
            GameEntry.Player.Money -= valueData.money;
            GameEntry.Cat.Stamina += valueData.stamina;
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm2, OnExit, valueData);
        }

        protected override void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.RaceGameForm, OnExit);
        }
    }
}