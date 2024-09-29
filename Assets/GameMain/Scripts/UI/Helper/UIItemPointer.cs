using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using GameMain;
using System;

public class UIItemPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private UIItemAction uiItemAction;
    [SerializeField] private UIItemTag uiItemTag;
    [SerializeField] private Vector3 initSize = Vector3.one * 1.2f;
    [SerializeField] private Vector3 toSize = Vector3.one * 1.2f;
    [SerializeField] private float speed = 0.3f;

    [SerializeField] private Image image;
    [SerializeField] private GameObject cover;
    //音效编号
    [SerializeField] private int soundId;

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
        if(image!=null&&uiItemTag== UIItemTag.Holding)
            image.gameObject.SetActive(false);
        //cover?.SetActive(false);
    }

    private void Update()
    {
        switch (uiItemAction)
        {
            case UIItemAction.Overall:
                cover?.SetActive(!IsMouseInside(this.gameObject));
                break;
        }
    }
    protected Vector3 MouseToWorld(Vector3 mousePos)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.z = screenPosition.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private bool IsMouseInside(GameObject go)
    {
        Vector2 size=go.GetComponent<SpriteRenderer>().size;
        Vector3 position = this.transform.position;
        Vector3 mousePos = MouseToWorld(Input.mousePosition);
        return (position.x - size.x / 2) < mousePos.x &&
            (position.x + size.x / 2) > mousePos.x &&
            (position.y - size.y / 2) < mousePos.y &&
            (position.y + size.y / 2) > mousePos.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        try
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
        catch(Exception e)
        {
            Debug.LogError($"错误，在物体{gameObject.name}的UIItem组件上发生了{e}的错误");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        try
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
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"错误，在物体{gameObject.name}的UIItem组件上发生了{e}的错误");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        try
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
                default:
                    break;
            }
            if (soundId != 0)
            {
                GameMain.GameEntry.Sound.PlaySound(soundId);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"错误，在物体{gameObject.name}的UIItem组件上发生了{e}的错误");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        try
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
        catch (Exception e)
        {
            Debug.LogError($"错误，在物体{gameObject.name}的UIItem组件上发生了{e}的错误");
        }
    }
}
public enum UIItemAction
{ 
    Enter,
    Click,
    Overall
}
public enum UIItemTag
{
    Scaling,
    Holding,
    None
}
