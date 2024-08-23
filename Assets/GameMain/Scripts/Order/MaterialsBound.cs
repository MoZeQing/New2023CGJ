using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialsBound : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Transform canvas;

    //private void Start()
    //{
    //    canvas.transform.DOLocalMoveY(-14.8f, 0.5f);
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //canvas.transform.DOLocalMoveY(-12.8f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //canvas.transform.DOLocalMoveY(-14.8f, 0.5f);
    }
}
