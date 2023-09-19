using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.UI;

namespace GameMain
{
    public class ClosetItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Transform up;
        [SerializeField] private Transform down;
        [SerializeField] private GameObject lockOn;
        [SerializeField] private Image img;
        [SerializeField] private ClosetForm closetForm;
        [SerializeField] private ItemTag closetTag;


        private ClosetItemState state;

        private void Start()
        {
            img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            if (GameEntry.Utils.GetPlayerItem(closetTag) == null ||
                GameEntry.Utils.GetPlayerItem(closetTag).itemNum == 0)
            {
                lockOn.SetActive(true);
            }
            else
            {
                closetForm.SetData(GameEntry.Utils.GetPlayerItem(closetTag));
                lockOn.SetActive(false);
            }
        }

        public void SetState(ClosetItemState state)
        {
            this.state = state;
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (state == ClosetItemState.Idle)
            {
                state = ClosetItemState.PickUp;
                this.transform.DOPause();
                if (GameEntry.Utils.GetPlayerItem(closetTag) == null ||
                    GameEntry.Utils.GetPlayerItem(closetTag).itemNum == 0)
                {
                    img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    lockOn.SetActive(true);
                    return;
                }
                else
                {
                    closetForm.SetData(GameEntry.Utils.GetPlayerItem(closetTag));
                    lockOn.SetActive(false);
                    img.color = Color.white;
                }
                this.transform.DOLocalMove(up.localPosition, 0.5f).SetEase(Ease.OutExpo);
            }
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            if (state==ClosetItemState.PickUp)
            {
                state = ClosetItemState.Idle;
                this.transform.DOPause();
                this.transform.DOLocalMove(down.localPosition, 0.5f).SetEase(Ease.OutExpo);
                closetForm.SetData(null);
                img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (state != ClosetItemState.Freeze)
            {
                state = ClosetItemState.Choice;
                //·¢ËÍÏûÏ¢
                closetForm.SetData(GameEntry.Utils.GetPlayerItem(closetTag), this);
            }
        }
    }

    public enum ClosetItemState
    { 
        Idle,
        PickUp,
        Choice,
        Freeze,
    }
}