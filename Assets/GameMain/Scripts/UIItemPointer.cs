using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIItemPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Vector3 size = Vector3.one * 1.2f;
    [SerializeField] private float speed = 0.3f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.DOScale(size, speed);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOScale(Vector3.one, speed);
    }
}
