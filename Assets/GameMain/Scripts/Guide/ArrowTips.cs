using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

namespace GameMain
{
    public class ArrowTips : MonoBehaviour
    {
        [SerializeField] private Transform arrow;
        private void OnEnable()
        {
            arrow.gameObject.SetActive(false);
            GameEntry.Event.Subscribe(ArrowEventArgs.EventId, OnArrowEvent);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(ArrowEventArgs.EventId, OnArrowEvent);
        }

        private void OnArrowEvent(object sender, GameEventArgs e)
        {
            ArrowEventArgs args = e as ArrowEventArgs;
            arrow.gameObject.SetActive(args.Enable);
            arrow.localPosition = args.ArrowPos;
            arrow.localRotation = Quaternion.Euler(args.ArrowRot);
        }
    }
}

