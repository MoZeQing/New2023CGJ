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
        [SerializeField] private Text textText;

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
            if (GameEntry.SaveLoad.ContainsCGFlag(dRGallery.Trigger))
            {
                image.color = Color.white;
                textText.text = string.Empty;
                button.interactable = true;
            }
            else
            {
                image.color = Color.gray;
                textText.text = dRGallery.Text.Replace("\\n","\n");
                button.interactable = false;
            }
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
