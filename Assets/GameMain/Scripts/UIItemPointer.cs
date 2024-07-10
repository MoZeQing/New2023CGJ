using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using GameMain;

public class UIItemPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private UIItemTag uiItemTag;
    [SerializeField] private Vector3 size = Vector3.one * 1.2f;
    [SerializeField] private float speed = 0.3f;

    [SerializeField] private Image image;
    [SerializeField] private Animator animator;
    //“Ù–ß±‡∫≈
    [SerializeField] private int soundId;

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
        if(image!=null)
            image.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (uiItemTag)
        {
            case UIItemTag.Scaling:
                this.transform.DOScale(size, speed);
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
        switch (uiItemTag)
        {
            case UIItemTag.Scaling:
                this.transform.DOScale(Vector3.one, speed);
                break;
            case UIItemTag.Holding:
                if (image != null)
                    image.gameObject.SetActive(false);
                break;
        }
    }
}

public enum UIItemTag
{
    Scaling,
    Holding,
    Animation,
}
