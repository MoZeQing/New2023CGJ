using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class MenuForm : BaseForm
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject menuItemPre;
        [SerializeField] private MenuItem[] menuItems;
        [SerializeField] private Text demandText;
        [SerializeField] private Text combinationText;
        [SerializeField] private Text clientText;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform plane;

        private List<MenuItem> coffeeItems;
        private int index;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => plane.gameObject.SetActive(false));
            foreach (MenuItem menuItem in menuItems)
            {
                menuItem.GetComponent<Button>().onClick.AddListener(()=>ShowCoffeePlane(menuItem));
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            foreach (MenuItem menuItem in menuItems)
            {
                menuItem.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        private void ShowCoffeePlane(MenuItem menuItem)
        { 
            plane.gameObject.SetActive(true);
            for (int i = 0; i < canvas.childCount; i++)
            {
                Destroy(canvas.GetChild(i).gameObject);
            }
            foreach (DRCoffee dRCoffee in GameEntry.DataTable.GetDataTable<DRCoffee>().GetAllDataRows())
            {
                if (!GameEntry.Player.HasCoffee(dRCoffee.Id))
                    continue;
                GameObject go = GameObject.Instantiate(menuItemPre, canvas);
                MenuItem item = go.GetComponent<MenuItem>();
                item.SetData(dRCoffee);
                item.GetComponent<Button>().onClick.AddListener(()=>OnClick(menuItem,dRCoffee));
            }
        }

        private void OnClick(MenuItem menuItem,DRCoffee dRCoffee)
        { 
            menuItem.SetData(dRCoffee);
        }
    }

    public class CoffeeData
    {
        public int ID;
        public int EXP;
        public int Level
        {
            get
            {
                return EXP / 20;
            }
            private set { }
        }
        public NodeTag NodeTag
        {
            get
            { 
                DRCoffee dRCoffee=GameEntry.DataTable.GetDataTable<DRCoffee>().GetDataRow(ID);
                return (NodeTag)dRCoffee.NodeTag;
            }
        }
    }
}
