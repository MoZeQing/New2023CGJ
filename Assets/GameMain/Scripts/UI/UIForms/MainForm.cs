using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace GameMain
{
    public class MainForm : UIFormLogic
    {
        [SerializeField] private Button downButton;
        [SerializeField] private Button upButton;
        [SerializeField] private Transform canvasTrans;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            upButton.onClick.AddListener(Up);
            downButton.onClick.AddListener(Down);
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

