using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private ManagerData managerData;
        private List<int> banedCoffee = new List<int>();
        private List<EventEffectData> eventEffects = new List<EventEffectData>();
        private Dictionary<int,MenuItem> mapMenuItems= new Dictionary<int,MenuItem>(); 
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Player.AddCoffee(201, 40);
            GameEntry.Player.AddCoffee(202, 40);
            GameEntry.Player.AddCoffee(203, 20);
            GameEntry.Player.AddCoffee(204, 20);
            GameEntry.Player.AddCoffee(205, 0);
            GameEntry.Player.AddCoffee(206, 40);
            GameEntry.Player.AddCoffee(207, 0);

            managerData = new ManagerData();
            exitBtn.onClick.AddListener(() => plane.gameObject.SetActive(false));
            okBtn.onClick.AddListener(StartManager);

            for (int i=0;i<menuItems.Length;i++)
            {
                MenuItem menuItem = menuItems[i];
                menuItem.GetComponent<Button>().onClick.AddListener(() => ShowCoffeePlane(menuItem));
                menuItem.Hide();
                if (GameEntry.Player.CoffeeLevel <= i)
                    menuItem.GetComponent<Button>().interactable = false;
            }
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
                if (!GameEntry.Player.HasCoffee(dRCoffee.Id))//ban掉得不会出现
                    continue;
                if (managerData.CoffeeTags.Contains(dRCoffee.Id))//已经选择的不会出现
                    continue;
                CoffeeData coffeeData = GameEntry.Player.GetCoffee(dRCoffee.Id);
                GameObject go = GameObject.Instantiate(menuItemPre, canvas);
                MenuItem item = go.GetComponent<MenuItem>();
                item.SetData(coffeeData, GetDemand(coffeeData));

                if(!banedCoffee.Contains(dRCoffee.Id))
                    item.GetComponent<Button>().onClick.AddListener(()=>ChoiceCoffeeItem(menuItem,dRCoffee.Id));
                else
                    item.GetComponent<Button>().interactable= false;//这里考虑加入一个UI提示玩家
            }
        }

        private void ChoiceCoffeeItem(MenuItem menuItem,int coffeeId)
        {
            if(menuItem.CoffeeID != 0&& managerData.CoffeeTags.Contains(menuItem.CoffeeID))
                managerData.CoffeeTags[managerData.CoffeeTags.IndexOf(menuItem.CoffeeID)] = coffeeId;
            else
                managerData.CoffeeTags.Add(coffeeId);

            if (mapMenuItems.ContainsKey(coffeeId))
                mapMenuItems[coffeeId] = menuItem;
            else
                mapMenuItems.Add(coffeeId, menuItem);

            CoffeeData coffeeData = GameEntry.Player.GetCoffee(coffeeId);
            plane.gameObject.SetActive(false);
            //重复计算组合效果
            combinationText.text=string.Empty;
            managerData.Combinations.Clear();
            eventEffects.Clear();
            foreach (DRCombination dRCombination in GameEntry.DataTable.GetDataTable<DRCombination>().GetAllDataRows())
            {
                int index = 0;
                string[] tags=dRCombination.Tags.Split('-');

                for (int i = 0; i < tags.Length; i++)
                {
                    int result = 0;
                    if (!int.TryParse(tags[i], out result))
                    {
                        Debug.LogError($"错误，无效的数值，请检查Combinatio表中的{dRCombination.Id}的Tag");
                        return;
                    }
                    for (int j = 0; j < managerData.CoffeeTags.Count; j++)
                    {
                        if (!GameEntry.Player.GetCoffee(managerData.CoffeeTags[j]).IsTag(result))
                            continue;
                        else
                            index++;
                    }
                }

                if (tags.Length == index)
                {
                    combinationText.text += $"【{dRCombination.Name}】：{dRCombination.Text}\n";
                    //计算组合
                    string[] eventeffects = dRCombination.EventEffect.Split('-');
                    for (int j = 0; j < eventeffects.Length; j++)
                    {
                        int result = 0;
                        if (!int.TryParse(eventeffects[j], out result))
                        {
                            Debug.LogError($"错误，无效的数值，请检查Combinatio表中的{dRCombination.Id}的EventEffect{dRCombination.EventEffect}");
                            return;
                        }
                        if (result == 0)
                        {
                            Debug.LogError($"错误，无效的数值，请检查Combinatio表中的{dRCombination.Id}的EventEffect{dRCombination.EventEffect}");
                            return;
                        }

                        DREventEffect dREventEffect = GameEntry.DataTable.GetDataTable<DREventEffect>().GetDataRow(result);
                        EventEffectTag eventEffectTag;
                        if (!Enum.TryParse<EventEffectTag>(dREventEffect.EventEffectTag, out eventEffectTag))
                        {
                            Debug.LogError($"错误的EventEffect标签，请检查{dREventEffect.Id}的标签");
                            return;
                        }
                        eventEffects.Add(new EventEffectData(dREventEffect));
                        //计算组合
                        managerData.Combinations.Add(dRCombination.Id);
                    }
                }
            }
            UpdateData();
        }
        private int GetDemand(CoffeeData coffeeData)
        {
            EventEffectData eventEffectData = new EventEffectData();
            for (int j = 0; j < eventEffects.Count; j++)
            {
                if (eventEffects[j].EventEffectTag == EventEffectTag.Client)
                {
                    if (coffeeData.IsTag(eventEffects[j].Trigger))
                    {
                        eventEffectData.Add(eventEffects[j]);
                    }
                }
            }
            return (int)(coffeeData.Demand * (1 + eventEffectData.ParamTwo / 100f) + eventEffectData.ParamThree / 100f);
        }
        private int GetPrice(CoffeeData coffeeData)
        {
            EventEffectData eventEffectData = new EventEffectData();
            for (int j = 0; j < eventEffects.Count; j++)
            {
                if (eventEffects[j].EventEffectTag == EventEffectTag.Price)
                {
                    if (coffeeData.IsTag(eventEffects[j].Trigger))
                    {
                        eventEffectData.Add(eventEffects[j]);
                    }
                }
            }
            return (int)(coffeeData.Price * (1 + eventEffectData.ParamTwo / 100f) + eventEffectData.ParamThree / 100f);
        }
        private int GetScience(CoffeeData coffeeData)
        {
            EventEffectData eventEffectData = new EventEffectData();
            for (int j = 0; j<eventEffects.Count; j++)
            {
                if (eventEffects[j].EventEffectTag == EventEffectTag.Science)
                {
                    if (coffeeData.IsTag(eventEffects[j].Trigger))
                    {
                        eventEffectData.Add(eventEffects[j]);
                    }
                }
            }
            return (int)(coffeeData.Demand * (1 + eventEffectData.ParamTwo / 100f) + eventEffectData.ParamThree / 100f);
        }
        private int GetExp(CoffeeData coffeeData)
        {
            EventEffectData eventEffectData = new EventEffectData();
            for (int j = 0; j < eventEffects.Count; j++)
            {
                if (eventEffects[j].EventEffectTag == EventEffectTag.Exp)
                {
                    if (coffeeData.IsTag(eventEffects[j].Trigger))
                    {
                        eventEffectData.Add(eventEffects[j]);
                    }
                }
            }
            return (int)(coffeeData.Demand * (1 + eventEffectData.ParamTwo / 100f) + eventEffectData.ParamThree / 100f);
        }
        private void UpdateData()
        {
            managerData.Demand = 0;
            managerData.Client = 0;
            managerData.Price= 0;

            for (int i = 0; i < managerData.CoffeeTags.Count; i++)
            {
                CoffeeData coffeeData = GameEntry.Player.GetCoffee(managerData.CoffeeTags[i]);
                mapMenuItems[managerData.CoffeeTags[i]].SetData(coffeeData, GetDemand(coffeeData));
                managerData.Demand += coffeeData.Demand;
                managerData.Client += GetDemand(coffeeData);
                managerData.Price += coffeeData.Price;
            }
            managerData.Price /= managerData.CoffeeTags.Count;

            if (managerData.Client >= managerData.Demand)
            {
                demandText.text = $"咖啡的需求：{managerData.Client}(+{Mathf.Floor((float)managerData.Client / (float)managerData.Demand * 100f) - 100f}%)";
                demandText.color = Color.green;
            }
            else
            {
                demandText.text = $"咖啡的需求：{managerData.Client}({Mathf.Floor((float)managerData.Client / (float)managerData.Demand * 100f) - 100f}%)";
                demandText.color = Color.red;
            }
        }

        private void StartManager()
        {
            UpdateData();

            BuffData buffData = GameEntry.Buff.GetBuff();
            //managerData.Demand
            //managerData.Client
            managerData.Money = (int)(managerData.Client * managerData.Price);
            managerData.Money = (int)(managerData.Money * buffData.MoneyMulti + buffData.MoneyPlus);

            GameEntry.UI.OpenUIForm(UIFormId.ManagerForm, managerData);
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }

    public class EventEffectData
    {
        /// <summary>
        /// 事件标签
        /// </summary>
        public EventEffectTag EventEffectTag;
        /// <summary>
        /// 所有的事件效果
        /// </summary>
        public List<int> eventeffects= new List<int>();
        /// <summary>
        /// 事件的条件
        /// </summary>
        public string Trigger;
        /// <summary>
        /// 参数1
        /// </summary>
        public string ParamOne;
        /// <summary>
        /// 参数2
        /// </summary>
        public int ParamTwo;
        /// <summary>
        /// 参数3
        /// </summary>
        public int ParamThree;

        public EventEffectData() { }
        public EventEffectData(EventEffectTag eventEffectTag) 
        { 
            this.EventEffectTag= eventEffectTag;
        }
        public EventEffectData(DREventEffect dREventEffect)
        { 
            EventEffectTag= (EventEffectTag)Enum.Parse(typeof(EventEffectTag), dREventEffect.EventEffectTag);
            eventeffects.Add(dREventEffect.Id);
            Trigger=dREventEffect.Trigger;
            ParamOne = dREventEffect.ParamOne;
            ParamTwo = dREventEffect.ParamTwo;
            ParamThree = dREventEffect.ParamThree;
        }
        public void Add(EventEffectData eventEffectData)
        {
            eventeffects.AddRange(eventEffectData.eventeffects);
            ParamTwo += eventEffectData.ParamTwo;
            ParamThree += eventEffectData.ParamThree;
        }
        public void Add(DREventEffect dREventEffect)
        {
            eventeffects.Add(dREventEffect.Id);
            ParamTwo += dREventEffect.ParamTwo;
            ParamThree += dREventEffect.ParamThree;
        }
        public void Remove(EventEffectData eventEffectData)
        {
            foreach (int eventEffect in eventeffects)
            {
                eventeffects.Remove(eventEffect);
            }
            ParamTwo -= eventEffectData.ParamTwo;
            ParamThree -= eventEffectData.ParamThree;
        }
        public void Remove(DREventEffect dREventEffect)
        {
            if (!eventeffects.Contains(dREventEffect.Id))
            {
                Debug.LogError($"错误，你正在试图删除一个不存在的EventEffect，其ID为{dREventEffect.Id}");
                return;
            }

            eventeffects.Remove(dREventEffect.Id);
            ParamTwo -= dREventEffect.ParamTwo;
            ParamThree -= dREventEffect.ParamThree;
        }
    }

    public enum EventEffectTag
    { 
        Client=100,
        BanedCoffee=101,
        Money=102,
        Price=103,
        Science=104,
        Exp=105
    }
    public class ManagerData
    {
        public int Demand;
        public int Client;
        public int Science;
        public int Money;
        public int Price;
        public List<int> CoffeeTags = new List<int>();
        public List<int> Combinations = new List<int>();
    }
    public class CoffeeData
    {
        public int Id;
        public int Exp;
        public DRCoffee dRCoffee;
        public int Level
        {
            get
            {
                return Exp / 20;
            }
            private set { }
        }
        public int Demand
        {
            get
            {
                string[] levels = dRCoffee.DemandLevel.Split('-');
                float level = float.Parse(levels[Level]);
                return (int)(dRCoffee.Demand * level);
            }
            private set { }
        }
        public int Price
        {
            get
            {
                return dRCoffee.Price;
            }
            private set { }
        }
        public string[] Tags
        {
            get
            {
                return dRCoffee.Tags.Split('-');
            }
            private set { }
        }
        public CoffeeData(DRCoffee dRCoffee)
        {
            this.Id = dRCoffee.Id;
            this.Exp = 0;
            this.dRCoffee=dRCoffee;
        }
        public CoffeeData(DRCoffee dRCoffee, int exp)
        {
            this.Id = dRCoffee.Id;
            this.Exp= exp;
            this.dRCoffee = dRCoffee;
        }
        public NodeTag NodeTag
        {
            get
            { 
                return (NodeTag)dRCoffee.NodeTag;
            }
        }
        public bool IsTag(string tagsText)
        {
            if (tagsText == string.Empty) return true;
            string[] tags = tagsText.Split('-');
            for (int i = 0; i < tags.Length; i++)
            {
                int result = 0;
                if (!int.TryParse(tags[i], out result))
                {
                    Debug.LogError($"错误，错误的参数，来源于{tagsText}");
                    return false;
                }
                if (IsTag(result))
                    return true;
            }            
            return false;
        }
        public bool IsTag(int tag)
        {
            if (tag == 0) return true;
            string[] tags = dRCoffee.Tags.Split('-');
            for (int i = 0; i < tags.Length; i++)
            {
                int result = 0;
                if (!int.TryParse(tags[i], out result))
                {
                    Debug.LogError($"错误，无效的数值，请检查Coffee表中的{dRCoffee.Id}");
                    return false;
                }
                if (tag == result)
                    return true;
            }
            return false;
        }
    }
}
