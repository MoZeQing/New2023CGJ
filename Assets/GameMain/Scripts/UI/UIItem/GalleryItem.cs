using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class GalleryItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        private DRGallery gallery;
        private Action<DRGallery> mAction;

        private void OnClick()
        {
            mAction(gallery);
        }

        public void SetData(DRGallery dRGallery)
        {
            Display();
            gallery = dRGallery;
            image.sprite = Resources.Load<Sprite>(dRGallery.CGPath);
        }

        public void SetClick(Action<DRGallery> onClick)
        {
            mAction = onClick;
            button.onClick.AddListener(OnClick);
        }
        public void Display()
        {
            this.gameObject.SetActive(true);
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }

}
