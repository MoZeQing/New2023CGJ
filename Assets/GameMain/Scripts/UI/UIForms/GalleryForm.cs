using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GalleryForm : UIFormLogic
    {
        [SerializeField] private Image display;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button next;
        [SerializeField] private Transform canvas1;
        [SerializeField] private Transform canvas2;
        [SerializeField] private List<GalleryItem> items;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(()=>GameEntry.UI.CloseUIForm(this.UIForm));
            next.onClick.AddListener(OnNext);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetAction(OnClick);
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(1))
            {
                if (display.gameObject.activeSelf)
                {
                    display.gameObject.SetActive(false);
                }
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            next.onClick.RemoveAllListeners();
        }
        private void OnNext()
        {
            bool flag = canvas1.gameObject.activeSelf;
            canvas1.gameObject.SetActive(!flag);
            canvas2.gameObject.SetActive(flag);
        }
        private void OnClick(Sprite sprite)
        {
            display.gameObject.SetActive(true);
            display.sprite = sprite;
        }
    }
}
