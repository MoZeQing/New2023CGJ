using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GymForm : UIFormLogic
    {
        [SerializeField] private Button easyBtn;
        [SerializeField] private Button middleBtn;
        [SerializeField] private Button hardBtn;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            easyBtn.onClick.AddListener(EasyBtn_Click);
            middleBtn.onClick.AddListener(MiddleBtn_Click);
            hardBtn.onClick.AddListener(HardBtn_Click);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            easyBtn.onClick.RemoveAllListeners();
            middleBtn.onClick.RemoveAllListeners();
            hardBtn.onClick.RemoveAllListeners();
        }

        private void EasyBtn_Click() {
            GameEntry.Utils.Energy -= 20;
            GameEntry.Utils.MaxEnergy += 2;
            OnExit();
        }

        private void MiddleBtn_Click() {
            GameEntry.Utils.Energy -= 40;
            GameEntry.Utils.MaxEnergy += 5;
            OnExit();
        }

        private void HardBtn_Click()
        {
            GameEntry.Utils.Energy -= 60;
            GameEntry.Utils.MaxEnergy += 8;
            OnExit();
        }

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.outingBefore = false;
            GameEntry.Dialog.StoryUpdate();
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }

}