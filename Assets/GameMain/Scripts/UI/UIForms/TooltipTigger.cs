using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMain
{
    public class TooltipTigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField]private Tooltip tooltip;
        [SerializeField] private string content;
        [SerializeField] private string header;
        public void OnPointerEnter(PointerEventData eventData)
        {
            Show(content,header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Show(string content,string header = "")
        {
            tooltip.SetText(content, header);
            tooltip.gameObject.SetActive(true);
        }

        private void Hide()
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}
