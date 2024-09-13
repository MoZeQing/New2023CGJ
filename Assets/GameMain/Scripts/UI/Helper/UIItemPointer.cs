using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using GameMain;

public class UIItemPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private UIItemAction uiItemAction;
    [SerializeField] private UIItemTag uiItemTag;
    [SerializeField] private Vector3 initSize = Vector3.one * 1.2f;
    [SerializeField] private Vector3 toSize = Vector3.one * 1.2f;
    [SerializeField] private float speed = 0.3f;

    [SerializeField] private Image image;
    //“Ù–ß±‡∫≈
    [SerializeField] private int soundId;

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
        if(image!=null&&uiItemTag== UIItemTag.Holding)
            image.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiItemAction != UIItemAction.Enter)
            return;
        switch (uiItemTag)
        {
            case UIItemTag.Scaling:
                image.transform.DOScale(toSize, speed);
                break;
            case UIItemTag.Holding:
                if (image != null)
                    image.gameObject.SetActive(true);
                break;
        }
        if (soundId != 0)
        {
            GameMain.GameEntry.Sound.PlaySound(soundId);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiItemAction != UIItemAction.Enter)
            return;
        switch (uiItemTag)
        {
            case UIItemTag.Scaling:
                image.transform.DOScale(initSize, speed);
                break;
            case UIItemTag.Holding:
                if (image != null)
                    image.gameObject.SetActive(false);
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (uiItemAction != UIItemAction.Click)
            return;
        switch (uiItemTag)
        {
            case UIItemTag.Scaling:
                image.transform.DOScale(toSize, speed);
                break;
            case UIItemTag.Holding:
                if (image != null)
                    image.gameObject.SetActive(true);
                break;
        }
        if (soundId != 0)
        {
            GameMain.GameEntry.Sound.PlaySound(soundId);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (uiItemAction != UIItemAction.Click)
            return;
        switch (uiItemTag)
        {
            case UIItemTag.Scaling:
                image.transform.DOScale(initSize, speed);
                break;
            case UIItemTag.Holding:
                if (image != null)
                    image.gameObject.SetActive(false);
                break;
        }
    }
}
public enum UIItemAction
{ 
    Enter,
    Click
}
public enum UIItemTag
{
    Scaling,
    Holding
}
