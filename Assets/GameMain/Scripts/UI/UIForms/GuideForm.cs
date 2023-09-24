using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace GameMain
{
    public class GuideForm : UIFormLogic
    {
        [SerializeField] private Image image;
        [SerializeField] private List<Sprite> dialogs1=new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs2=new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs3=new List<Sprite>();

        private List<Sprite> dialogs = new List<Sprite>();
        private int index;
        private ProcedureGuide mProcedureGuide;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            int i = (int)userData;
            switch (i-1)
            {
                case 1:dialogs = dialogs1;break;
                case 2:dialogs = dialogs2;break;
                case 3:dialogs = dialogs3;break;
            }
            index = 0;
            image.sprite = dialogs[index];
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(0))
            {
                if (index >= dialogs.Count)
                {
                    GameEntry.UI.CloseUIForm(this.UIForm);
                    return;
                }
                image.sprite = dialogs[index];
                index++;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
