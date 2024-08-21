using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class ClosetForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text infoText;
        [SerializeField] private Text titleText;
        [SerializeField] private Image closetImg;
        [SerializeField] private Transform canvas;
        [Header("锚点")]
        [SerializeField] private Image[] closetItems = new Image[4];//0后1左2中3右
        [SerializeField] private Transform[] canvases = new Transform[4];//0后1左2中3右

        private List<DRItem> dRItems = new List<DRItem>();
        private int index = 0;//目前的显示序列
        private int itemIndex = 0;//目前使用的Item的序列


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            index = dRItems.Count;
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            okBtn.onClick.AddListener(OkBtn);

            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Clothes)
                    continue;
                dRItems.Add(item);
            }
            Init();
            leftBtn.onClick.AddListener(Right);
            rightBtn.onClick.AddListener(Left);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            okBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }
        private void Init()
        {
            //初始化，将编号为0，1和最大值的服装显示到左中右
            closetItems[2].sprite = Resources.Load<Sprite>(dRItems[0].IconPath);
            closetItems[2].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[0].Id) == null);
            closetItems[2].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet==dRItems[0].Id);
            closetItems[3].sprite = Resources.Load<Sprite>(dRItems[1].IconPath);
            closetItems[3].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[1].Id) == null);
            closetItems[3].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[1].Id);
            closetItems[1].sprite = Resources.Load<Sprite>(dRItems[dRItems.Count-1].IconPath);
            closetItems[1].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[dRItems.Count - 1].Id) == null);
            closetItems[1].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[dRItems.Count - 1].Id);
            index = index % dRItems.Count;
            infoText.text = dRItems[index].Info;
            infoText.text = infoText.text.Replace("\\n", "\n");
            closetImg.sprite = Resources.Load<Sprite>(dRItems[index].ImagePath);
            closetImg.color = GameEntry.Player.GetPlayerItem((ItemTag)dRItems[index].Id) != null ? Color.white : Color.gray;
            titleText.text = dRItems[index].ItemName;
            titleText.text = titleText.text.Replace("\\n", "\n");
        }
        private void Right()
        {
            //在循环中计算当前的index的数量
            index = (index + dRItems.Count - 1) % dRItems.Count;
            //将当前的框体移动到下一个对应的位置
            closetItems[0].transform.DOMove(canvases[1].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[1].transform.DOMove(canvases[2].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[2].transform.DOMove(canvases[3].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[3].transform.DOMove(canvases[0].position, 0.5f).SetEase(Ease.OutExpo);
            //缩放当中的框体，将移动到中间的框体放大
            closetItems[1].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            closetItems[2].transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(Ease.OutExpo);
            //交换所有的框体
            Image img = closetItems[0];
            closetItems[0] = closetItems[3];
            closetItems[3] = closetItems[2];
            closetItems[2] = closetItems[1];
            closetItems[1] = img;
            //将居中的框体置为顶层，并且调整颜色
            closetItems[2].color = Color.white;
            closetItems[3].color = Color.grey;
            closetItems[2].transform.SetSiblingIndex(4);

            closetItems[1].sprite = Resources.Load<Sprite>(dRItems[(index + dRItems.Count - 1) % dRItems.Count].IconPath);
            closetItems[1].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[(index + dRItems.Count - 1) % dRItems.Count].Id) == null);
            closetItems[1].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet== dRItems[(index + dRItems.Count - 1) % dRItems.Count].Id);
            closetItems[2].sprite = Resources.Load<Sprite>(dRItems[index].IconPath);
            closetItems[2].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[index].Id) == null);
            closetItems[2].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[index].Id);
            closetItems[3].sprite = Resources.Load<Sprite>(dRItems[(index + dRItems.Count + 1) % dRItems.Count].IconPath);
            closetItems[3].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[(index + dRItems.Count + 1) % dRItems.Count].Id) == null);
            closetItems[3].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[(index + dRItems.Count + 1) % dRItems.Count].Id);

            infoText.text = dRItems[index].Info;
            infoText.text = infoText.text.Replace("\\n", "\n");
            closetImg.sprite = Resources.Load<Sprite>(dRItems[index].ImagePath);
            closetImg.color = GameEntry.Player.GetPlayerItem((ItemTag)dRItems[index].Id) != null ? Color.white : Color.gray;
            titleText.text = dRItems[index].ItemName;
            titleText.text = titleText.text.Replace("\\n", "\n");
        }
        private void Left()
        {
            index = (index + dRItems.Count + 1) % dRItems.Count;

            closetItems[0].transform.DOMove(canvases[3].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[1].transform.DOMove(canvases[0].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[2].transform.DOMove(canvases[1].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[3].transform.DOMove(canvases[2].position, 0.5f).SetEase(Ease.OutExpo);

            closetItems[3].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            closetItems[2].transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(Ease.OutExpo);

            Image img = closetItems[0];
            closetItems[0] = closetItems[1];
            closetItems[1] = closetItems[2];
            closetItems[2] = closetItems[3];
            closetItems[3] = img;

            closetItems[2].color = Color.white;
            closetItems[1].color = Color.grey;
            closetItems[2].transform.SetSiblingIndex(4);

            closetItems[1].sprite = Resources.Load<Sprite>(dRItems[(index + dRItems.Count - 1) % dRItems.Count].IconPath);
            closetItems[1].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[(index + dRItems.Count - 1) % dRItems.Count].Id) == null);
            closetItems[1].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[(index + dRItems.Count - 1) % dRItems.Count].Id);
            closetItems[2].sprite = Resources.Load<Sprite>(dRItems[index].IconPath);
            closetItems[2].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[index].Id) == null);
            closetItems[2].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[index].Id);
            closetItems[3].sprite = Resources.Load<Sprite>(dRItems[(index + dRItems.Count + 1) % dRItems.Count].IconPath);
            closetItems[3].transform.GetChild(0).gameObject.SetActive(GameEntry.Player.GetPlayerItem((ItemTag)dRItems[(index + dRItems.Count + 1) % dRItems.Count].Id) == null);
            closetItems[3].transform.GetChild(1).gameObject.SetActive(GameEntry.Utils.Closet == dRItems[(index + dRItems.Count + 1) % dRItems.Count].Id);

            infoText.text = dRItems[index].Info;
            infoText.text = infoText.text.Replace("\\n", "\n");
            closetImg.sprite = Resources.Load<Sprite>(dRItems[index].ImagePath);
            closetImg.color = GameEntry.Player.GetPlayerItem((ItemTag)dRItems[index].Id) != null ? Color.white : Color.gray;
            titleText.text = dRItems[index].ItemName;
            titleText.text = titleText.text.Replace("\\n", "\n");
        }
        private void OkBtn()
        {
            int closet = index = dRItems[index].Id;
            if (GameEntry.Player.GetPlayerItem((ItemTag)closet)!=null)
            {
                for (int i = 0; i < closetItems.Length; i++)
                {
                    closetItems[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                GameEntry.Utils.Closet = closet;
                closetItems[2].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}