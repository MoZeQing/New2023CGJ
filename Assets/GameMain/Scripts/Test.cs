using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Test : MonoBehaviour
    {
        private bool flag=false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                flag = !flag;
                if (flag)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.ConsoleForm, this);
                }
                else
                {
                    GameEntry.UI.CloseUIForm(UIFormId.ConsoleForm);
                }
            }
        }
    }
}
