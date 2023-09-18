using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ClosetItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Transform up;
    [SerializeField] private Transform down;
    [SerializeField] private Image img;

    private void Start()
    {
        img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.transform.DOPause();
        this.transform.DOLocalMove(up.localPosition,0.5f).SetEase(Ease.OutExpo);
        img.color = Color.white;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.transform.DOPause();
        this.transform.DOLocalMove(down.localPosition, 0.5f).SetEase(Ease.OutExpo);
        img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}
