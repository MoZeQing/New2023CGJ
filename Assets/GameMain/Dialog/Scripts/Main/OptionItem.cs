using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using GameMain;
using DG.Tweening;
using Dialog;

public class OptionItem : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Button button;

    private object data;
    private EventHandler handler;

    public void OnEnable()
    {
        button.transform.localPosition = Vector3.down * 30;
        button.GetComponent<Image>().color= Color.gray;
        button.transform.DOLocalMoveY(0f,0.3f).SetEase(Ease.InOutExpo);
        button.GetComponent<Image>().DOColor(Color.white, 0.3f);
    }
    public void OnInit(object data,EventHandler handler)
    {
        this.data = data;
        this.handler = handler;

        OptionData optionData = (OptionData)data;
        text.text = optionData.text;

        button.onClick.AddListener(Onclick);

        button.interactable= GameEntry.Utils.Check(optionData.trigger);
    }

    private void Onclick()
    {
        handler(data, EventArgs.Empty);
    }
}
