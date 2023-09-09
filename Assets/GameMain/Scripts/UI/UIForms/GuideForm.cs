using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GuideForm : UIFormLogic
    {
        [SerializeField] private List<GameObject> dialogs;

        private int index;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButton(0))
            {
                foreach (GameObject go in dialogs)
                { 
                    go.SetActive(false);
                }
                dialogs[index].SetActive(true);
                index++;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
