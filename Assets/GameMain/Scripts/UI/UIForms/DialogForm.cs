using System.Collections;
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
    public class DialogForm : BaseForm
    {
        [SerializeField] private DialogBox mDialogBox;
        [SerializeField] private BaseStage mStage;

        private DialogueGraph dialogueGraph;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            SetData((DialogueGraph)BaseFormData.UserData);
        }

        public void SetData(DialogueGraph dialogue)
        {
            dialogueGraph= dialogue;
            mDialogBox.SetDialog(dialogueGraph);
            mDialogBox.SetComplete(CloseForm);
        }
        private void OnComplete()
        {
            GameEntry.Dialog.OnComplete();
        }
        private void CloseForm()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.Dialog.InDialog = false;
            Invoke(nameof(OnComplete), 1f);
            GameEntry.Event.FireNow(this, DialogEventArgs.Create(GameEntry.Dialog.InDialog, dialogueGraph.name));
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
