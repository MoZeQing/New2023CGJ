using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using static UnityEngine.Networking.UnityWebRequest;

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
        private Dictionary<MenuItem,int> mapMenuItems= new Dictionary<MenuItem, int>(); 
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Player.AddCoffee(201, 0);
            GameEntry.Player.AddCoffee(202, 0);
            GameEntry.Player.AddCoffee(203, 0);
            GameEntry.Player.AddCoffee(204, 0);
            GameEntry.Player.AddCoffee(205, 0);
            GameEntry.Player.AddCoffee(206, 0);
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
                if (managerData.MapCoffees.ContainsKey(dRCoffee.Id))//已经选择的不会出现
                    continue;
                CoffeeData coffeeData = GameEntry.Player.GetCoffee(dRCoffee.Id);
                GameObject go = GameObject.Instantiate(menuItemPre, canvas);
                MenuItem item = go.GetComponent<MenuItem>();
                item.SetData(coffeeData, managerData.GetClient(coffeeData));

                if(!managerData.banedCoffee.Contains(dRCoffee.Id))
                    item.GetComponent<Button>().onClick.AddListener(()=>ChoiceCoffeeItem(menuItem,dRCoffee.Id));
                else
                    item.GetComponent<Button>().interactable= false;//这里考虑加入一个UI提示玩家
            }
        }

        private void ChoiceCoffeeItem(MenuItem menuItem,int coffeeId)
        {
            CoffeeData coffeeData = GameEntry.Player.GetCoffee(coffeeId);
            if (mapMenuItems.ContainsKey(menuItem))
            {
                managerData.MapCoffees.Remove(mapMenuItems[menuItem]);
                mapMenuItems[menuItem] = coffeeId;
            }
            else
            {
                mapMenuItems.Add(menuItem, coffeeId);
            }
            managerData.MapCoffees.Add(coffeeId, coffeeData);

            plane.gameObject.SetActive(false);
            //重复计算组合效果
            combinationText.text=string.Empty;
            managerData.Combinations.Clear();
            managerData.eventEffects.Clear();
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
                    foreach (KeyValuePair<int, CoffeeData> pair in managerData.MapCoffees)
                    {
                        if (!pair.Value.IsTag(result))
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
                        managerData.eventEffects.Add(new EventEffectData(dREventEffect));
                        //计算组合
                        managerData.Combinations.Add(dRCombination.Id);
                    }
                }
            }
            UpdateData();
        }
      
        private void UpdateData()
        {
            foreach (KeyValuePair<MenuItem, int> pair in mapMenuItems)
            {
                CoffeeData coffeeData = managerData.MapCoffees[pair.Value];
                pair.Key.SetData(coffeeData, managerData.GetClient(coffeeData));
            }

            if (managerData.GetTotalClient() >= managerData.GetTotalDemand())
            {
                demandText.text = $"咖啡的需求：{managerData.GetTotalClient()}(+{Mathf.Floor((float)managerData.GetTotalClient() / (float)managerData.GetTotalDemand() * 100f) - 100f}%)";
                demandText.color = Color.green;
            }
            else
            {
                demandText.text = $"咖啡的需求：{managerData.GetTotalClient()}({Mathf.Floor((float)managerData.GetTotalClient() / (float)managerData.GetTotalDemand() * 100f) - 100f}%)";
                demandText.color = Color.red;
            }
        }

        private void StartManager()
        {
            UpdateData();

            BuffData buffData = GameEntry.Buff.GetBuff();
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
        public Dictionary<int,CoffeeData> MapCoffees = new Dictionary<int, CoffeeData>();
        public List<int> Combinations = new List<int>();
        public List<int> banedCoffee = new List<int>();
        public List<EventEffectData> eventEffects = new List<EventEffectData>();

        public int GetTotalDemand()
        {
            int demand = 0;
            foreach (KeyValuePair<int, CoffeeData> pair in MapCoffees)
            { 
                demand+= pair.Value.Demand;
            }
            return demand;
        }
        public int GetTotalClient()
        {
            int client = 0;
            foreach (KeyValuePair<int, CoffeeData> pair in MapCoffees)
            {
                client += GetClient(pair.Value);
            }
            return client;
        }

        public int GetTotalMoney()
        {
            int price = 0;
            foreach (KeyValuePair<int, CoffeeData> pair in MapCoffees)
            {
                price += GetPrice(pair.Value);
            }
            return (int)price / MapCoffees.Count * GetTotalClient();
        }
        public int GetClient(CoffeeData coffeeData)
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
        public int GetPrice(CoffeeData coffeeData)
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
        public int GetScience(CoffeeData coffeeData)
        {
            EventEffectData eventEffectData = new EventEffectData();
            for (int j = 0; j < eventEffects.Count; j++)
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
        public int GetExp(CoffeeData coffeeData)
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
            return (int)(GetClient(coffeeData) * (1 + eventEffectData.ParamTwo / 100f) + eventEffectData.ParamThree / 100f);
        }
    }
    public class CoffeeData
    {
        public int Id;
        public int Exp { get; set; }
        public DRCoffee DRCoffee;
        public int Level
        {
            get
            {
                string[] explevels = DRCoffee.ExpLevel.Split('-');
                for (int i = 0; i < explevels.Length; i++)
                {
                    int a= int.Parse(explevels[i]);
                    if (Exp < a) return i;
                }
                return 0;
            }
            private set { }
        }
        public int Demand
        {
            get
            {
                string[] levels = DRCoffee.DemandLevel.Split('-');
                float level = float.Parse(levels[Level]);
                return (int)(DRCoffee.Demand * level);
            }
            private set { }
        }
        public int ExpLevel
        {
            get
            {
                string[] explevels = DRCoffee.ExpLevel.Split('-');
                return int.Parse(explevels[Level]);
            }
            private set { }
        }
        public int Price
        {
            get
            {
                return DRCoffee.Price;
            }
            private set { }
        }
        public string[] Tags
        {
            get
            {
                return DRCoffee.Tags.Split('-');
            }
            private set { }
        }
        public CoffeeData(DRCoffee dRCoffee)
        {
            this.Id = dRCoffee.Id;
            this.Exp = 0;
            this.DRCoffee=dRCoffee;
        }
        public CoffeeData(DRCoffee dRCoffee, int exp)
        {
            this.Id = dRCoffee.Id;
            this.Exp= exp;
            this.DRCoffee = dRCoffee;
        }
        public NodeTag NodeTag
        {
            get
            { 
                return (NodeTag)DRCoffee.NodeTag;
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
            string[] tags = DRCoffee.Tags.Split('-');
            for (int i = 0; i < tags.Length; i++)
            {
                int result = 0;
                if (!int.TryParse(tags[i], out result))
                {
                    Debug.LogError($"错误，无效的数值，请检查Coffee表中的{DRCoffee.Id}");
                    return false;
                }
                if (tag == result)
                    return true;
            }
            return false;
        }
    }
}
