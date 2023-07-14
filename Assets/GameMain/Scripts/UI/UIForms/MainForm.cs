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
        [SerializeField] private Button catButton;
        [SerializeField] private Transform canvasTrans;
        [SerializeField] private DialogForm dialogForm;
        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private int r;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ProcedureMain main = (ProcedureMain)userData;
            main.MainForm = this;

            upButton.onClick.AddListener(Up);
            downButton.onClick.AddListener(Down);
            catButton.onClick.AddListener(Cat);

            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 0.3f;
            playSoundParams.Priority = 64;
            playSoundParams.SpatialBlend = 0f;
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/BGM/maou_bgm_acoustic52.mp3", "BGM", playSoundParams);

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
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/page_turn.mp3", "Sound");

            Camera.main.transform.DOMove(new Vector3(0, 4.6f, -8f), 1f).SetEase(Ease.OutExpo);
            canvasTrans.transform.DOLocalMove(new Vector3(0, -800, 0), 1f).SetEase(Ease.OutExpo);
        }

        private void Down()
        {
            GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/page_turn.mp3", "Sound");

            Camera.main.transform.DOMove(new Vector3(0, -3.4f, -8f), 1f).SetEase(Ease.OutExpo);
            canvasTrans.transform.DOLocalMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutExpo);
        }

        private void Cat()
        {
            r = Random.Range(0, 30);

            if(r == 0)
            {
                GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/Yudachi.mp3", "Sound");
            }
            else
            {
                GameEntry.Sound.PlaySound($"Assets/GameMain/Audio/Sounds/cat.mp3", "Sound");
            }
        }
    }

}

