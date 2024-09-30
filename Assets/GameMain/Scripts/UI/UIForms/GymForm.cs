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
            Tuple<ValueTag, int> tuple2 = new Tuple<ValueTag, int>(ValueTag.Wisdom, valueData.wisdom);
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm2, OnExit, tuple2);
        }

        protected override void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.RaceGameForm, OnExit);
        }
    }
}