using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIItemPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f),0.3f);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
    }
}
