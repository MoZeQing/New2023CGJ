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
        [SerializeField] private Text pageText;
        [SerializeField] private Transform canvas;
        [SerializeField] private List<GalleryItem> mItems;

        protected List<DRGallery> dRGalleries = new List<DRGallery>();
        protected int index;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            leftBtn?.onClick.AddListener(Left);
            rightBtn?.onClick.AddListener(Right);

            index = 0;
            dRGalleries = new List<DRGallery>(GameEntry.DataTable.GetDataTable<DRGallery>().GetAllDataRows());
            ShowItems();
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
        protected virtual void ShowItems()
        {
            leftBtn.interactable = index != 0;

            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRGalleries.Count)
                {
                    mItems[i].SetData(dRGalleries[index]);
                    mItems[i].SetClick(OnClick);
                }
                else
                    mItems[i].Hide();
                index++;
            }
            rightBtn.interactable = index < dRGalleries.Count;
            if (pageText != null)
                pageText.text = (index / mItems.Count).ToString();
        }
        protected virtual void Right()
        {
            ShowItems();
        }

        protected virtual void Left()
        {
            index -= 2 * mItems.Count;
            ShowItems();
        }
        private void OnClick(DRGallery gallery)
        {
            display.gameObject.SetActive(true);
            display.sprite = Resources.Load<Sprite>(gallery.CGPath);
        }
    }
}
