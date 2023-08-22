using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Cat :MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer mSpriteRenderer = null;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {

        }

        public void HideCat()
        {
            this.gameObject.SetActive(false);
        }
        public void ShowCat()
        {
            this.gameObject.SetActive(true);
        }
    }

}
