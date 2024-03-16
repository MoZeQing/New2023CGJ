using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMain;
using System;

public class CardItem : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Button btn;

    [SerializeField] private Sprite background;
    [SerializeField] private Sprite sprite;

    public void SetData(DRBench dRBench)
    {
        backgroundImg.sprite = background;
        text.gameObject.SetActive(false);
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        
    }
}
