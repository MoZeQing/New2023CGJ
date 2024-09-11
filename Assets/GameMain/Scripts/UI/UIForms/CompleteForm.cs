using DG.Tweening;
using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteForm : BaseForm
{
    [SerializeField] private List<CompleteItem> completeItems;
    [SerializeField] private Button okBtn;
    [SerializeField] private Transform canvas;
    [SerializeField,Range(0f,2f)] private float speed=1f;

    private Action mAction;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        Sequence sequence = DOTween.Sequence();
        canvas.transform.localScale= Vector3.one*0.01f;
        sequence.Append(canvas.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutExpo));

        mAction = BaseFormData.Action;
        Dictionary<ValueTag,int> pairs= BaseFormData.UserData as Dictionary<ValueTag,int>;

        okBtn.onClick.AddListener(() =>
        {
            mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        });
        BuffData buffData = GameEntry.Buff.GetBuff();
        okBtn.gameObject.SetActive(false);

        for (int i = 0; i < completeItems.Count; i++)
        {
            CompleteItem completeItem= completeItems[i];
            completeItem.gameObject.SetActive(false);
            if (i >= pairs.Count)
                continue;
            completeItem.transform.GetChild(0).GetComponent<Image>().color= Color.clear;
            completeItem.transform.GetChild(0).localPosition = Vector3.down * 20f;
            sequence.Append(completeItem.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 0.1f * speed).OnComplete(() => completeItem.gameObject.SetActive(true)));
            sequence.Append(completeItem.transform.GetChild(0).DOLocalMoveY(0, 0.5f * speed).SetEase(Ease.OutExpo));
        }
        sequence.AppendCallback(() => okBtn.gameObject.SetActive(true));
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        okBtn.onClick.RemoveAllListeners();
    }
}
