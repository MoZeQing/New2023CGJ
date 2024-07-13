using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GymForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button trainBtn;
        [SerializeField] private Button matchBtn;
        [SerializeField] private Button quickBtn;
        [SerializeField] private Button gameBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private CharData charData;
        [SerializeField] private PlayerData playerData;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.gameObject.SetActive(false);
            quickBtn.onClick.AddListener(QuickBtn_Click);
            gameBtn.onClick.AddListener(GameBtn_Click);
            trainBtn.onClick.AddListener(TrainBtn_Click);
            matchBtn.onClick.AddListener(MatchBtn_Click);
            exitBtn.onClick.AddListener(OnExit);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            quickBtn.onClick.RemoveAllListeners();
            gameBtn.onClick.RemoveAllListeners();
            trainBtn.onClick.RemoveAllListeners();
            matchBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void QuickBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm,OnExit);
        }

        private void GameBtn_Click()
        { 
            
        }

        private void TrainBtn_Click() 
        {
            canvas.gameObject.SetActive(true);

            quickBtn.transform.localPosition = Vector3.down * 30;
            quickBtn.GetComponent<Image>().color = Color.gray;
            quickBtn.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.InOutExpo);
            quickBtn.GetComponent<Image>().DOColor(Color.white, 0.3f);

            gameBtn.transform.localPosition = Vector3.down * 30;
            gameBtn.GetComponent<Image>().color = Color.gray;
            gameBtn.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.InOutExpo);
            gameBtn.GetComponent<Image>().DOColor(Color.white, 0.3f);
        }

        private void MatchBtn_Click() 
        {

        }

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}