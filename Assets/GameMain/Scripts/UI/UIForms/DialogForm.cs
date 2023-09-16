﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;
using UnityEngine.EventSystems;
using GameMain;
using DG.Tweening;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.Impl;

namespace GameMain
{
    public class DialogForm : UIFormLogic
    {
        [SerializeField] private DialogBox mDialogBox;
        [SerializeField] private BaseStage mStage;

        private DialogueGraph dialogueGraph;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dialogueGraph = (DialogueGraph)userData;
            mDialogBox.SetDialog(dialogueGraph);
            mDialogBox.SetComplete(OnComplete);
        }

        private void OnComplete()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            Invoke(nameof(CloseForm), 1f);
        }
        private void CloseForm()
        {
            GameEntry.Dialog.InDialog = false;
            GameEntry.Event.FireNow(this, DialogEventArgs.Create(GameEntry.Dialog.InDialog, dialogueGraph.name));
            GameEntry.UI.CloseUIForm(this.UIForm);
        } 
    }
}
