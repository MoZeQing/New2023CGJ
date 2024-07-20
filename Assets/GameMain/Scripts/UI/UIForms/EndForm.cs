using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class EndForm : BaseForm
    {
        [SerializeField] private Button mOKButton;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mOKButton.onClick.AddListener(Back);
        }

        private void Back()
        {
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
