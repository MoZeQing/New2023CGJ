using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class MainMenu :UIFormLogic
{
        private ProcedureMenu m_ProcedureMenu;

        [SerializeField] private Button button;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMenu = (ProcedureMenu)userData;

            button.onClick.AddListener(m_ProcedureMenu.StartGame);
        }
    }

}

