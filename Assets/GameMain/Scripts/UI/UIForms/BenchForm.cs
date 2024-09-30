using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;
using System;

namespace GameMain
{
    public class BenchForm : OutForm
    {
        protected override void QuickBtn_Click()
        {
            GameEntry.Player.Ap -= valueData.ap;
            GameEntry.Player.Money -= valueData.money;
            GameEntry.Cat.Charm += valueData.charm;
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm1, OnExit, valueData);
        }

        protected override void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.FlopForm, OnExit);
        }
    }
}
