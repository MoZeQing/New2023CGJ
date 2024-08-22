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
        [SerializeField] private GameObject equipOn;
        [SerializeField] private Image img;
        [SerializeField] private ClosetForm closetForm;
        [SerializeField] private ItemTag closetTag;

        private ClosetItemState state;

        private void Start()
        {
            Check();
            if (GameEntry.Player.GetPlayerItem(closetTag) == null ||
                GameEntry.Player.GetPlayerItem(closetTag).itemNum == 0)
            {
                lockOn.SetActive(true);
            }
            else
            {
                //closetForm.SetData(GameEntry.Player.GetPlayerItem(closetTag));
                lockOn.SetActive(false);
            }
        }

        public void Check()
        {
            if ((ItemTag)GameEntry.Cat.Closet == closetTag)
            {
                equipOn.SetActive(true);
                img.color = Color.white;
            }
            else
            {
                equipOn.SetActive(false);
                img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        public void SetState(ClosetItemState state)
        {
            this.state = state;
            this.transform.DOLocalMove(down.localPosition, 0.5f).SetEase(Ease.OutExpo);
            Check();
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (state == ClosetItemState.Idle)
            {
                state = ClosetItemState.PickUp;
                this.transform.DOPause();
                Check();
                if (GameEntry.Player.GetPlayerItem(closetTag) == null ||
                    GameEntry.Player.GetPlayerItem(closetTag).itemNum == 0)
                {
                    img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    lockOn.SetActive(true);
                    return;
                }
                else
                {
                    //closetForm.SetData(GameEntry.Player.GetPlayerItem(closetTag));
                    lockOn.SetActive(false);
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
                //closetForm.SetData(null);
                Check();
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (lockOn.gameObject.activeSelf)
                return;
            if (state != ClosetItemState.Freeze)
            {
                state = ClosetItemState.Choice;
                //·¢ËÍÏûÏ¢
                //closetForm.SetData(GameEntry.Player.GetPlayerItem(closetTag), this);
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