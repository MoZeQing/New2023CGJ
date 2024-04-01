using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMain;
using System;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CardItem : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image frontImg;
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Button btn;

    private Action<DRBench> mAction;
    private bool front=true;
    private DRBench dRBench;
    public void SetData(DRBench dRBench,Action<DRBench> action)
    {
        btn.onClick.AddListener(OnClick);
        text.text = dRBench.Text;
        mAction = action;
        this.dRBench = dRBench;
        front = false;

        frontImg.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        backgroundImg.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    private void OnClick()
    {
        mAction(dRBench);
        Turn();
    }

    public void Turn()
    {
        if (front)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(frontImg.transform.DORotate(new Vector3(0f, 90f, 0f), 0.6f));
            sequence.Append(backgroundImg.transform.DORotate(new Vector3(0f, 0f, 0f), 0.6f));
            front = false;
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(backgroundImg.transform.DORotate(new Vector3(0f, 90f, 0f), 0.6f));
            sequence.Append(frontImg.transform.DORotate(new Vector3(0f, 0f, 0f), 0.6f));
            front = true;
        }
    }
}
