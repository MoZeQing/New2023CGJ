using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.UI;

namespace GameMain
{
    public class ClosetItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform up;
        [SerializeField] private Transform down;
        [SerializeField] private Image img;
        [SerializeField] private ClosetForm closetForm;
        [SerializeField] private ItemTag closetTag; 

        private void Start()
        {
            img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            this.transform.DOPause();
            if (GameEntry.Utils.GetPlayerItem(closetTag) == null ||
                GameEntry.Utils.GetPlayerItem(closetTag).itemNum == 0)
            {
                img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                return;
            }
            else
            {
                closetForm.SetData(GameEntry.Utils.GetPlayerItem(closetTag));
            }
            this.transform.DOLocalMove(up.localPosition, 0.5f).SetEase(Ease.OutExpo);
            img.color = Color.white;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            this.transform.DOPause();
            this.transform.DOLocalMove(down.localPosition, 0.5f).SetEase(Ease.OutExpo);
            closetForm.SetData(null);
            img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }

}