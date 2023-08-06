using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Cat :Entity, IPointerClickHandler
    {
        private SpriteRenderer mSpriteRenderer = null;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            GameEntry.Cat.SetBehavior(BehaviorTag.Click);
        }
    }

}
