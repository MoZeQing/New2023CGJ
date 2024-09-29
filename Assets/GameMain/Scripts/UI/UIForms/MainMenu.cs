using GameFramework.Event;
using GameFramework.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class MainMenu :BaseForm
{
        private ProcedureMenu m_ProcedureMenu;

        [SerializeField] private Button startBtn;
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button galleryForm;
        [SerializeField] private Button exitBtn;

        [SerializeField] private Button workTestBtn;
        
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMenu = (ProcedureMenu)BaseFormData.UserData;

            startBtn.onClick.AddListener(m_ProcedureMenu.StartGame);
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));
            galleryForm.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GalleryForm, this));
            exitBtn.onClick.AddListener(() => UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit));
            workTestBtn.onClick.AddListener(() => GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Test)));

            GameEntry.Event.Subscribe(DialogEventArgs.EventId, OnDialogEvent);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            startBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
            loadBtn.onClick.RemoveAllListeners();
            optionBtn.onClick.RemoveAllListeners();
            galleryForm.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, OnDialogEvent);
        }

        private void OnDialogEvent(object sender, GameEventArgs e)
        {
            DialogEventArgs args = (DialogEventArgs)e;
            if (!args.InDialog)
            {
                DRUIForms dRUIForms = GameEntry.DataTable.GetDataTable<DRUIForms>().GetDataRow((int)BaseFormData.UIFormId);

                if (dRUIForms.OpenSound != 0)
                {
                    GameEntry.Sound.PlaySound(dRUIForms.OpenSound);
                }
            }
        }
    }

}

