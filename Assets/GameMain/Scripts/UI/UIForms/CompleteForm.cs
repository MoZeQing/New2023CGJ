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

        okBtn.gameObject.SetActive(false);
        for (int i = 0; i < completeItems.Count; i++)
        {
            completeItems[i].gameObject.SetActive(false);
        }


        int index = 0;
        foreach (KeyValuePair<ValueTag, int> pair in pairs)
        {
            if (index > 2)
                break;
            CompleteItem completeItem = completeItems[index];
            completeItem.SetData(pair.Key, pair.Value);
            completeItem.transform.GetChild(0).transform.localPosition = Vector3.down * 50f;
            sequence.Append(completeItem.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white,0.1f).OnComplete(() => completeItem.gameObject.SetActive(true)));
            sequence.Append(completeItem.transform.GetChild(0).DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo));
            index++;
        }
        foreach (KeyValuePair<ValueTag, int> pair in pairs)
        {
            switch (pair.Key)
            {
                case ValueTag.Ap:
                    GameEntry.Utils.Ap += pair.Value;
                    break;
                case ValueTag.Money:
                    GameEntry.Utils.Money+= pair.Value;
                    break;
                case ValueTag.Charm:
                    GameEntry.Utils.Charm += pair.Value;
                    break;
                case ValueTag.Stamina:
                    GameEntry.Utils.Stamina+= pair.Value;
                    break;
                case ValueTag.Wisdom:
                    GameEntry.Utils.Wisdom+= pair.Value;
                    break;
            }
        }
        sequence.AppendCallback(() => okBtn.gameObject.SetActive(true));
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        okBtn.onClick.RemoveAllListeners();
    }
}
