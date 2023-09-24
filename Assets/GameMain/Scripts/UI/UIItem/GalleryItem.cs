using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;

    private Action<Sprite> mAction;

    public void SetAction(Action<Sprite> action)
    { 
        mAction= action;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        mAction(image.sprite);
    }
}
