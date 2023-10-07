using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Test : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)&&!GameEntry.UI.HasUIForm(UIFormId.ConsoleForm))
            {
                GameEntry.UI.OpenUIForm(UIFormId.ConsoleForm, this);
            }
        }
    }
}
