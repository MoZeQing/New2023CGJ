using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;


namespace GameMain
{
    public class LibraryForm : OutForm
    {       
        protected override void QuickBtn_Click()
        {
            GameEntry.Player.Ap -= valueData.ap;
            GameEntry.Player.Money -= valueData.money;
            GameEntry.Cat.Wisdom += valueData.wisdom;
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm3, OnExit, valueData);
        }

        protected override void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.QueryForm, OnExit);
        }
    }
}
