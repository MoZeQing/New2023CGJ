using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MenuItem : MonoBehaviour
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private Image[] stars;
        [SerializeField] private Image[] tags;
        [SerializeField] private Image background;
        [SerializeField] private Sprite emptyImg;
        [SerializeField] private Sprite backgroundImg;
        [SerializeField] private Text coffeeNameText;
        [SerializeField] private Text demandText;
        [SerializeField] private Image upArrow;
        [SerializeField] private Image downArrow;

        private CoffeeData coffeeData;
        private int nowDemand = 0;

        public int CoffeeID
        {
            get
            {
                if(coffeeData != null)
                    return coffeeData.Id;
                return 0;
            }
            private set { }
        }
        public void SetData(CoffeeData coffeeData,int demand)
        {
            this.coffeeData = coffeeData;   
            canvas.gameObject.SetActive(true);
            background.sprite = backgroundImg;
            coffeeNameText.text = this.coffeeData.dRCoffee.CoffeeName;

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(i < coffeeData.Level);
            }

            for (int i = 0; i < coffeeData.Tags.Length; i++)
            {
                int result=0;
                if (!int.TryParse(coffeeData.Tags[i], out result))
                {
                    Debug.LogError($"错误，无效的数据，请检查Coffee表中的{coffeeData.Id}项的Tags");
                }
                DRTag dRTag = GameEntry.DataTable.GetDataTable<DRTag>().GetDataRow(result);
                tags[i].sprite = Resources.Load<Sprite>(dRTag.ImagePath);
            }

            if (demand != nowDemand)
            {
                if (demand > coffeeData.Demand)
                {
                    DOTween.To(value => { demandText.text = Mathf.Floor(value).ToString(); }, startValue: coffeeData.Demand, endValue: demand, duration: 1f);
                    //demandText.text = $"咖啡的需求：{demand}(+{Mathf.Floor((float)demand / (float)coffeeData.Demand * 100) - 100}%)";
                    demandText.color = Color.green;
                    //播放上涨动画
                    upArrow.gameObject.SetActive(true);
                    upArrow.transform.localPosition = Vector3.down * 10f;
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(upArrow.transform.DOLocalMoveY(10, 1f));
                    sequence.AppendCallback(() => upArrow.gameObject.SetActive(false));
                }
                else if (demand < coffeeData.Demand)
                {
                    DOTween.To(value => { demandText.text = Mathf.Floor(value).ToString(); }, startValue: coffeeData.Demand, endValue: demand, duration: 1f);
                    //demandText.text = $"咖啡的需求：{demand}({Mathf.Floor((float)demand / (float)coffeeData.Demand * 100) - 100}%)";
                    demandText.color = Color.red;
                    //播放下降动画
                    downArrow.gameObject.SetActive(true);
                    downArrow.transform.localPosition = Vector3.up * 10f;
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(downArrow.transform.DOLocalMoveY(-10, 1f));
                    sequence.AppendCallback(() => downArrow.gameObject.SetActive(false));
                }
                else
                {
                    demandText.text = $"{demand}";
                    demandText.color = Color.black;
                }
            }
            nowDemand = demand;
        }
        public void SetData(CoffeeData coffeeData)
        {
            this.coffeeData = coffeeData;
            canvas.gameObject.SetActive(true);
            background.sprite = backgroundImg;
            coffeeNameText.text = this.coffeeData.dRCoffee.CoffeeName;
            demandText.text = $"咖啡的需求：{coffeeData.Demand}";
        }

        public void Hide()
        {
            canvas.gameObject.SetActive(false);
            background.sprite = emptyImg;
        }
    }
}
