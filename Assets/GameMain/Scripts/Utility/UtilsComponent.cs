using GameFramework.Event;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using XNode;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class UtilsComponent : GameFrameworkComponent
    {
        public Sprite[] modeSprites;
        public Sprite orderSprite;
        public bool isClose;
        public BoundData openBoundData;
        public BoundData closeBoundData;
        public Sprite openSprite;
        public Sprite closeSprite;
        public float voice=0.5f;
        public float word=0.06f;
        public OutingSceneState outSceneState;
        public int Closet { get; set; }
        public bool PickUp { get; set; } = false;

        public float OrderPower { get; set; } = 1f;
        public float PricePower { get; set; } = 1f;

        public List<OutingSceneState> outingSceneStates = new List<OutingSceneState>();

        private int mCarfSort = 99;
        public int CartSort
        {
            get
            {
                return mCarfSort++;
            }
        }
        //接下来尽可能使得utils只用于存储资源信息，而数据信息都给其它的类

        private Dictionary<TriggerTag, string> _values = new Dictionary<TriggerTag, string>();
        private List<string> _flags = new List<string>();
        public Dictionary<string, int> _flagValues = new Dictionary<string, int>();

        private List<WorkData> mWorkDatas = new List<WorkData>();

        private CharData mCharData = new CharData();
        private PlayerData mPlayerData = new PlayerData();
        private OutingSceneState mLocation;
        private GameState mGameState;
        private Week mWeek;
        private BehaviorTag mBehaviorTag;
        private WeatherTag mWeatherTag=WeatherTag.Morning;
        private bool mIsRain;

        public Dictionary<string, RecipeData> recipes = new Dictionary<string, RecipeData>();
        public Dictionary<string, CharSO> chars = new Dictionary<string, CharSO>();

        public void AddFriendFavor(string name, int favor)
        {
            if (!_friends.ContainsKey(name))
                _friends.Add(name, favor);
            else
                _friends[name] += favor;
            //硬编码转换
            if (name == "Money") _values[TriggerTag.FMoney] = _friends[name].ToString();
            if (name == "Regular") _values[TriggerTag.FRegular] = _friends[name].ToString();
            if (name == "Dog") _values[TriggerTag.FDog] = _friends[name].ToString();
            if (name == "Fiction") _values[TriggerTag.FFiction] = _friends[name].ToString();
            if (name == "Courier") _values[TriggerTag.FCourier] = _friends[name].ToString();
            if (name == "Doc") _values[TriggerTag.FWitch] = _friends[name].ToString();
        }

        public Dictionary<string, int> GetFriends()
        {
            return _friends;
        }

        public void ClearFriendFavor()
        {
            _friends.Clear();
        }

        public Dictionary<string, int> _friends = new Dictionary<string, int>();//好友字典

        public void ClearPlayerItem()
        {
            mPlayerData.items.Clear();
        }

        public void AddPlayerItem(ItemData itemData, int num)
        {
            if (GetPlayerItem(itemData.itemTag) == null)
            {
                mPlayerData.items.Add(new PlayerItemData(itemData, num));
            }
            else
            {
                GetPlayerItem(itemData.itemTag).itemNum += num;
            }
        }
        public void AddPlayerItem(ItemData itemData, int num, bool equip)
        {
            if (GetPlayerItem(itemData.itemTag) == null)
            {
                PlayerItemData playerItem = new PlayerItemData(itemData, num);
                playerItem.equiping = equip;
                mPlayerData.items.Add(playerItem);
            }
            else
            {
                GetPlayerItem(itemData.itemTag).itemNum += num;
            }
        }
        public PlayerItemData GetPlayerItem(ItemTag itemTag)
        {
            foreach (PlayerItemData itemData in mPlayerData.items)
            {
                if (itemData.itemTag == itemTag)
                    return itemData;
            }
            return null;
        }

        public bool IsRain
        {
            get
            {
                return mIsRain;
            }
            set
            { 
                mIsRain= value;
            }
        }

        public void AddWork(WorkData workData)
        {
            mWorkDatas.Add(workData);
        }
        public PlayerData PlayerData
        {
            get
            {
                return mPlayerData;
            }
            set
            {
                mPlayerData = value;
            }
        }
        public CharData CharData
        {
            get
            {
                return mCharData;
            }
            set
            {
                mCharData = value;
            }
        }
        public List<string> Flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }
        public List<WorkData> WorkDatas
        {
            get
            {
                return mWorkDatas;
            }
            set
            {
                mWorkDatas = value;
            }
        }
        public WeatherTag WeatherTag
        {
            get
            {
                return mWeatherTag;
            }
            set
            { 
                mWeatherTag= value;
            }
        }
        public BehaviorTag BehaviorTag
        {
            get
            {
                return mBehaviorTag;
            }
            set
            {
                mBehaviorTag = value;
                _values[TriggerTag.BehaviorTag] = mBehaviorTag.ToString();
            }
        }
        public GameState GameState
        {
            get
            {
                return mGameState;
            }
            set
            {
                mGameState = value;
                GameEntry.Event.FireNow(this, GameStateEventArgs.Create(mGameState));
                _values[TriggerTag.TimeTag] = mGameState.ToString();
            }
        }
        public OutingSceneState Location
        {
            get
            {
                return mLocation;
            }
            set
            {
                mLocation = value;
                _values[TriggerTag.Location] = mLocation.ToString();
            }
        }
        public bool CheckFlag(string key)
        {
            return _flags.Contains(key);
        }
        public bool CheckFlag(string key, int value)
        {
            if (_flagValues.ContainsKey(key))
                return _flagValues[key] == value;
            return false;
        }
        public void AddFlag(string flag)
        {
            if (!_flags.Contains(flag))
                _flags.Add(flag);
            if (_flagValues.ContainsKey(flag))
            {
                _flagValues[flag]++;
            }
            else
            {
                _flagValues.Add(flag, 1);
            }
        }
        public void RemoveFlag(string flag)
        {
            if (_flags.Contains(flag))
                _flags.Remove(flag);
            if (_flagValues.ContainsKey(flag))
                _flagValues.Remove(flag);
        }

        public void ClearFlag()
        {
            _flags.Clear();
            _flagValues.Clear();
        }
        public bool Check(TriggerData triggerData)
        {
            return Check(triggerData.trigger);
        }
        public bool Check(Trigger trigger)
        {
            if (trigger == null)
                return true;
            if (trigger.GetAndTrigger().Count != 0)
            {
                foreach (Trigger tr in trigger.GetAndTrigger())
                {
                    if (!Check(tr))
                    {
                        return false;
                    }
                }
            }
            if (trigger.GetOrTrigger().Count != 0)
            {
                foreach (Trigger tr in trigger.GetOrTrigger())
                {
                    if (Check(tr))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (trigger.key == TriggerTag.None)
                return true;
            if (trigger.key == TriggerTag.Flag)
                return _flags.Contains(trigger.value) != trigger.not;
            if (trigger.key == TriggerTag.FlagCount)
            {
                string[] strings = trigger.value.Split('|');
                if (!trigger.not)
                    return CheckFlag(strings[0], int.Parse(strings[1]));
                else
                    return !CheckFlag(strings[0], int.Parse(strings[1]));
            }
            if (!_values.ContainsKey(trigger.key))
                return false;
            if (trigger.equals)
            {
                if (_values[trigger.key] == trigger.value)
                    return true;
                else
                    return false;
            }
            else
            {
                if (trigger.not)//�ж�����
                {
                    if (int.Parse(_values[trigger.key]) > int.Parse(trigger.value))
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (int.Parse(_values[trigger.key]) < int.Parse(trigger.value))
                        return false;
                    else
                        return true;
                }
            }
        }
        public bool RunEvent(string text)
        {
            string[] console = text.Split(' ');
            EventTag eventTag = (EventTag)Enum.Parse(typeof(EventTag), console[0]);
            EventData eventData=new EventData(eventTag);
            {
                eventData.values = console;
            }
            return RunEvent(eventData);
        }

        public bool RunEvent(EventData eventData)
        {
            switch (eventData.eventTag)
            {
                case EventTag.Play:
                    GameEntry.Dialog.PlayStory(eventData.values[1]);
                    return true;
                case EventTag.AddMoney:
                    GameEntry.Player.Money += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddFavor:
                    GameEntry.Cat.Favor += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddEnergy:
                    GameEntry.Player.Energy += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddAp:
                    GameEntry.Player.Ap+= int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddItem:
                    GameEntry.Utils.AddPlayerItem(new ItemData((ItemTag)Enum.Parse(typeof(ItemTag), eventData.values[1])), int.Parse(eventData.values[2]));
                    return true;
                case EventTag.AddFlag:
                    GameEntry.Utils.AddFlag(eventData.values[1]);
                    return true;
                case EventTag.RemoveFlag:
                    GameEntry.Utils.RemoveFlag(eventData.values[1]);
                    return true;
                case EventTag.NextDay://重写逻辑
                    GameEntry.Player.Day++;
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
                    return true;
                case EventTag.PlayBgm:
                    return true;
                case EventTag.EndGame:
                    GameEntry.Dialog.SetComplete(null);
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu));
                    GameEntry.UI.OpenUIForm(UIFormId.EndForm);
                    return true;
                case EventTag.AddDay:
                    GameEntry.Player.Day += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.Rent:
                    //GameEntry.Utils.Rent=int.Parse(eventData.values[1]);
                    return true; 
                case EventTag.AddFriend:
                    GameEntry.Utils.AddFriendFavor(eventData.values[1], int.Parse(eventData.values[2]));
                    return true; 
                case EventTag.AddRecipe:
                    GameEntry.Player.AddRecipe(int.Parse(eventData.values[1]));
                    return true; 
                case EventTag.AddScene:
                    GameEntry.Utils.outingSceneStates.Add((OutingSceneState)int.Parse(eventData.values[1]));
                    return true;
                case EventTag.Test:
                    GameEntry.Event.FireNow(this, ValueEventArgs.Create(TriggerTag.Energy,"成功"));
                    return true;
                case EventTag.AddBuff:
                    GameEntry.Buff.AddBuff(int.Parse(eventData.values[1]));
                    return true;
                case EventTag.RemoveBuff:
                    GameEntry.Buff.RemoveBuff(int.Parse(eventData.values[1]));
                    return true;
                case EventTag.Weather:
                    GameEntry.Utils.WeatherTag = (WeatherTag)int.Parse(eventData.values[1]);
                    return true;
                case EventTag.SetClothing:
                    GameEntry.Utils.AddPlayerItem(new ItemData((ItemTag)int.Parse(eventData.values[1])), 1);
                    GameEntry.Utils.Closet = int.Parse(eventData.values[1]);
                    break;
                case EventTag.ShowForm:
                    GameEntry.UI.OpenUIForm((UIFormId)Enum.Parse(typeof(UIFormId), eventData.values[1]));
                    break;
                case EventTag.AddCharm:
                    GameEntry.Cat.Charm+= (int.Parse(eventData.values[1]));
                    break;
                case EventTag.AddStamina:
                    GameEntry.Cat.Stamina += (int.Parse(eventData.values[1]));
                    break;
            }
            return false;
        }
        public void UpdateData()
        {
            GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
        }
    }
    [System.Serializable]
    public class PlayerData
    {
        public int maxEnergy;
        public int energy;
        public int money;
        public int maxAp;
        public int ap;
        public int day;
        public int rent;
        public int heaterID;
        public int burnisherID;
        public int stirrerID;
        public int pressID;
        public int cafeID=1;
        public int acoffee;
        public int bcoffee;
        public int ccoffee;
        public int guideID;
        public List<PlayerItemData> items=new List<PlayerItemData>();

        public Dictionary<ValueTag, int> GetValueTag(Dictionary<ValueTag, int> dic)
        {
            if (maxAp != 0)
                dic.Add(ValueTag.MaxAp, maxAp);
            if (ap != 0)
                dic.Add(ValueTag.Ap, ap);
            if (money != 0)
                dic.Add(ValueTag.Money, money);
            return dic;
        }
    }

    
    public enum ValueTag
    {
        MaxAp,
        Ap,
        Money,
        Favor,
        Wisdom,
        Stamina,
        Charm
    }
}