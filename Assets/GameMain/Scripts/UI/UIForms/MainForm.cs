using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework.Sound;
using GameFramework.DataTable;
using GameFramework;

namespace GameMain
{
    public class MainForm : UIFormLogic
    {
        [SerializeField] private Button downButton;
        [SerializeField] private Button upButton;
        [SerializeField] private Transform canvasTrans;
        [SerializeField] private DialogForm dialogForm;
        private SoundComponent m_Sound;
        // private bool m_SoundPlayLoop;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ProcedureMain main = (ProcedureMain)userData;
            main.MainForm = this;

            upButton.onClick.AddListener(Up);
            downButton.onClick.AddListener(Down);

            // IDataTable<DRSound> dtNode = GameEntry.DataTable.GetDataTable<DRSound>();
            // DRSound drSound = dtSound.GetDataRow(19);

            m_Sound = this.GetComponent<SoundComponent>();
            // m_SoundPlayLoop = true;

            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/BGM/maou_bgm_acoustic52.mp3", "BGM");
            // GameEntry.Sound.SetVolume(0.5f);
            
        }

        public void SetDialog(string path)
        {
            dialogForm.SetDialog(path);
        }

        public void SetDialog(DialogueGraph graph)
        {
            dialogForm.SetDialog(graph);
        }

        private void Up()
        {
            Camera.main.transform.DOMove(new Vector3(0, 4.6f, -8f), 1f).SetEase(Ease.OutExpo);
            canvasTrans.transform.DOLocalMove(new Vector3(0, -800, 0), 1f).SetEase(Ease.OutExpo);
        }

        private void Down()
        {     
            Camera.main.transform.DOMove(new Vector3(0, -3.4f, -8f), 1f).SetEase(Ease.OutExpo);
            canvasTrans.transform.DOLocalMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutExpo);
        }
    }

}

