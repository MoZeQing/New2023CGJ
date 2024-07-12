using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ActionForm : BaseForm
{
    [SerializeField] private Image progressImg;
    [SerializeField] private Image catImg;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform completeCanvas;
    [SerializeField] private Image actionIcon;
    [SerializeField] private Text actionText;
    [SerializeField] private Button exitBtn;

    private Action mAction;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        mAction = BaseFormData.Action;
        completeCanvas.gameObject.SetActive(false);
        exitBtn.onClick.AddListener(() => 
        {
            mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        });
        progressImg.fillAmount = 0;
        canvas.transform.localPosition = Vector3.up * 1080f;
        canvas.transform.DOLocalMoveY(0, 1f).SetEase(Ease.OutExpo).OnComplete(() => 
        {
            progressImg.DOFillAmount(1f, 5f).OnComplete(() => 
            {
                completeCanvas.gameObject.SetActive(true);
                GameEntry.Sound.PlaySound(43);
                completeCanvas.transform.localScale = Vector3.one * 0.01f;
                completeCanvas.DOScale(Vector3.one, 0.5f);
            });
        });
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        exitBtn.onClick.RemoveAllListeners();
    }
}
