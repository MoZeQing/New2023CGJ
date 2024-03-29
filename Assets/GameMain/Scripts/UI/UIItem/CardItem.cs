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

    [SerializeField] private Sprite background;
    [SerializeField] private Sprite sprite;

    private bool front=true;
    private Tweener tweener;

    private void Start()
    {
        SetData(new DRBench());
    }

    public void SetData(DRBench dRBench)
    {
        btn.onClick.AddListener(OnClick);

        frontImg.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        backgroundImg.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
    }

    private void OnClick()
    {
        Debug.Log("µã»÷£¡");
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
