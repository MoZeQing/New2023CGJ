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
    [SerializeField,Range(0f,2f)] private float speed=0.5f;

    private Action mAction;
    Sequence sequence = null;
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        sequence = DOTween.Sequence();
        canvas.transform.localScale= Vector3.one*0.01f;
        sequence.Append(canvas.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo));

        mAction = BaseFormData.Action;
        ValueData valueData = BaseFormData.UserData as ValueData;

        okBtn.onClick.AddListener(() =>
        {
            mAction?.Invoke();
            GameEntry.UI.CloseUIForm(this.UIForm);
        });
        BuffData buffData = GameEntry.Buff.GetBuff();
        okBtn.gameObject.SetActive(false);

        completeItems[0].SetData(ValueTag.Stamina,valueData.stamina);
        completeItems[1].SetData(ValueTag.Wisdom,valueData.wisdom);
        completeItems[2].SetData(ValueTag.Charm, valueData.charm);
        for (int i = 0; i < completeItems.Count; i++)
        {
            CompleteItem completeItem= completeItems[i];
            completeItem.transform.GetChild(0).GetComponent<CanvasGroup>().alpha= 0f;
            completeItem.transform.GetChild(0).localPosition = Vector3.down * 20f;
            sequence.Append(completeItem.transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(1f, 0.1f * speed).OnComplete(() => completeItem.gameObject.SetActive(true)));
            sequence.Append(completeItem.transform.GetChild(0).DOLocalMoveY(0, 0.5f * speed).SetEase(Ease.OutExpo));
        }
        sequence.AppendCallback(() => okBtn.gameObject.SetActive(true));
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }
    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        okBtn.onClick.RemoveAllListeners();
    }
}
