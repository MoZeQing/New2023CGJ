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
        BuffData buffData = GameEntry.Buff.GetBuff();
        okBtn.gameObject.SetActive(false);
        for (int i = 0; i < completeItems.Count; i++)
        {
            completeItems[i].gameObject.SetActive(false);
        }

        if (pairs.ContainsKey(ValueTag.Stamina))
        {
            completeItems[0].SetData(ValueTag.Stamina, pairs[ValueTag.Stamina] + (int)buffData.StaminaPlus );
        }
        else
        {
            completeItems[0].SetData(ValueTag.Stamina, 0);
        }
        sequence.Append(completeItems[0].transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 0.1f).OnComplete(() => completeItems[0].gameObject.SetActive(true)));
        sequence.Append(completeItems[0].transform.GetChild(0).DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo));
        if (pairs.ContainsKey(ValueTag.Wisdom))
        {
            completeItems[1].SetData(ValueTag.Wisdom, pairs[ValueTag.Wisdom] + (int)buffData.WisdomPlus );
        }
        else
        {
            completeItems[1].SetData(ValueTag.Wisdom, 0);
        }
        sequence.Append(completeItems[1].transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 0.1f).OnComplete(() => completeItems[1].gameObject.SetActive(true)));
        sequence.Append(completeItems[1].transform.GetChild(0).DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo));
        if (pairs.ContainsKey(ValueTag.Charm))
        {
            completeItems[2].SetData(ValueTag.Charm, pairs[ValueTag.Charm]+(int)buffData.CharmPlus );
        }
        else
        {
            completeItems[2].SetData(ValueTag.Charm, 0);
        }
        sequence.Append(completeItems[2].transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 0.1f).OnComplete(() => completeItems[2].gameObject.SetActive(true)));
        sequence.Append(completeItems[2].transform.GetChild(0).DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo));

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
                    GameEntry.Utils.Charm += (pair.Value + (int)buffData.CharmPlus );
                    break;
                case ValueTag.Stamina:
                    GameEntry.Utils.Stamina+= (pair.Value + (int)buffData.StaminaPlus );
                    break;
                case ValueTag.Wisdom:
                    GameEntry.Utils.Wisdom+= (pair.Value + (int)buffData.WisdomPlus );
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
