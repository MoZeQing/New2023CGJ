using DG.Tweening;
using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettleItem : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private Image[] stars;
    [SerializeField] private Image progress;
    [SerializeField] private Text expText;
    [SerializeField] private Text coffeeNameText;
    [SerializeField] private Image upArrow;

    public void SetData(CoffeeData coffeeData, int exp)
    {
        this.gameObject.SetActive(true);
        DOTween.To(value => { expText.text = Mathf.Floor(value).ToString(); }, startValue: coffeeData.Exp, endValue: coffeeData.Exp+exp, duration: 2f);
        progress.fillAmount = (float)coffeeData.Exp / (float)coffeeData.ExpLevel;
        coffeeNameText.text = coffeeData.DRCoffee.CoffeeName;
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < coffeeData.Level);
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(progress.DOFillAmount((float)(coffeeData.Exp + exp) / (float)coffeeData.ExpLevel, 2f));
        sequence.AppendCallback(() =>
        {
            if (coffeeData.Level >= stars.Length)
                return;
            if ((float)(coffeeData.Exp + exp) > coffeeData.ExpLevel)
            {
                upArrow.gameObject.SetActive(true);
                upArrow.transform.localPosition = Vector3.down * 10f;
                upArrow.transform.DOLocalMoveY(+10f, 0.5f).OnComplete(() => {
                    upArrow.gameObject.SetActive(false);
                });
                coffeeData.Exp += exp;
                progress.fillAmount = (float)(coffeeData.Exp - exp) / (float)coffeeData.ExpLevel;
                progress.DOFillAmount((float)coffeeData.Exp / (float)coffeeData.ExpLevel, 0.5f);
                stars[coffeeData.Level-1].gameObject.SetActive(true);
                stars[coffeeData.Level-1].transform.localScale = Vector3.one * 1.5f;
                stars[coffeeData.Level-1].transform.DOScale(Vector3.one, 0.5f);
            }
        });
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
