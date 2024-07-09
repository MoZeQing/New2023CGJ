using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GalleryForm : BaseForm
    {
        [SerializeField] private Image display;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text text;
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform canvas1;
        [SerializeField] private Transform canvas2;
        [SerializeField] private List<GalleryItem> items;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(()=>GameEntry.UI.CloseUIForm(this.UIForm));
            leftBtn.onClick.AddListener(OnNext);
            rightBtn.onClick.AddListener(OnNext);
            leftBtn.interactable = canvas2.gameObject.activeSelf;
            rightBtn.interactable = canvas1.gameObject.activeSelf;

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
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }
        private void OnNext()
        {
            bool flag = canvas1.gameObject.activeSelf;
            canvas1.gameObject.SetActive(!flag);
            canvas2.gameObject.SetActive(flag);
            text.text = (flag ? 2 : 1).ToString();
            leftBtn.interactable = flag;
            rightBtn.interactable=!flag;
        }
        private void OnClick(Sprite sprite)
        {
            display.gameObject.SetActive(true);
            display.sprite = sprite;
        }
    }
}
