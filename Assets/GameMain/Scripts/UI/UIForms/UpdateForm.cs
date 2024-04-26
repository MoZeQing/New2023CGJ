using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System.Text;

namespace GameMain
{
    public class UpdateForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button updateBtn;
        [SerializeField] private Transform canvas;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            updateBtn.onClick.AddListener(Upgrade);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            updateBtn.onClick.RemoveAllListeners();
        }

        private void Upgrade()
        { 
            
        }
    }
}


