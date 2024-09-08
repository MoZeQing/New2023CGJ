using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialsBound : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Transform mBG;
    [SerializeField] private Transform mCanvas;
    [SerializeField, Range(0, 1f)] private float speed = 0.5f;
    [SerializeField] Ease ease= Ease.InExpo;

    private void Start()
    {
        mBG.transform.localPosition=new Vector3(0f,-200f,0f);
        mBG.transform.localScale = Vector3.one * 0.5f;
        mCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        mCanvas.GetComponent<CanvasGroup>().interactable= false;
        mCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mBG.transform.DOLocalMoveY(0f, speed).SetEase(ease);
        mBG.transform.DOScale(Vector3.one, speed).SetEase(ease);
        mCanvas.GetComponent<CanvasGroup>().DOFade(1f, speed).SetEase(ease);
        mCanvas.GetComponent<CanvasGroup>().interactable = true;
        mCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mBG.transform.DOLocalMoveY(-100, speed).SetEase(ease);
        mBG.transform.DOScale(Vector3.one * 0.5f, speed).SetEase(ease);
        mCanvas.GetComponent<CanvasGroup>().DOFade(0f, speed).SetEase(ease);
        mCanvas.GetComponent<CanvasGroup>().interactable = false;
        mCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
