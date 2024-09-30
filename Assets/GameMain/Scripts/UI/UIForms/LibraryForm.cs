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
            Tuple<ValueTag, int> tuple3 = new Tuple<ValueTag, int>(ValueTag.Wisdom, valueData.wisdom);
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm3, OnExit, tuple3);
        }

        protected override void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.QueryForm, OnExit);
        }
    }
}
