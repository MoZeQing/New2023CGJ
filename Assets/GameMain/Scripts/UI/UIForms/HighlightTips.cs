using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace GameMain
{
    public class HighlightTips : BaseForm,IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] private Text text;
        [SerializeField] private Image image;
        [SerializeField] private Transform canvas;

        private bool follow;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            text.text = BaseFormData.UserData.ToString();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (follow)
            {
                canvas.transform.DOMove(Input.mousePosition, 0.1f);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        public void OnPointerDown(PointerEventData data)
        {
            follow = true;
        }
        public void OnPointerUp(PointerEventData data)
        {
            follow = false;
        }
    }
}


