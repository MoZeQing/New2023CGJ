using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using XNode;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class UtilsComponent : GameFrameworkComponent
    {
        public Sprite orderSprite;
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

        private Dictionary<TriggerTag, string> mValues = new Dictionary<TriggerTag, string>();
        private List<string> _flags = new List<string>();
        public Dictionary<string, int> _flagValues = new Dictionary<string, int>();

        private List<WorkData> mWorkDatas = new List<WorkData>();

        private CharData mCharData = new CharData();
        private PlayerData mPlayerData = new PlayerData();
        private OutingSceneState mLocation;
        private GameState mGameState;
        private Week mWeek;
        private BehaviorTag mBehaviorTag;

        public bool OutingBefore { get; set; }

        public Dictionary<string, RecipeData> recipes = new Dictionary<string, RecipeData>();
        public Dictionary<string, CharSO> chars = new Dictionary<string, CharSO>();

        public void AddFriendFavor(string name, int favor)
        {
            if (!_friends.ContainsKey(name))
                _friends.Add(name, favor);
            else
                _friends[name] += favor;
            //硬编码转换
            if (name == "Money") mValues[TriggerTag.FMoney] = _friends[name].ToString();
            if (name == "Regular") mValues[TriggerTag.FRegular] = _friends[name].ToString();
            if (name == "Dog") mValues[TriggerTag.FDog] = _friends[name].ToString();
            if (name == "Fiction") mValues[TriggerTag.FFiction] = _friends[name].ToString();
            if (name == "Courier") mValues[TriggerTag.FCourier] = _friends[name].ToString();
            if (name == "Doc") mValues[TriggerTag.FWitch] = _friends[name].ToString();
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
        public BehaviorTag BehaviorTag
        {
            get
            {
                return mBehaviorTag;
            }
            set
            {
                mBehaviorTag = value;
                mValues[TriggerTag.BehaviorTag] = mBehaviorTag.ToString();
            }
        }
        public Week Week
        {
            get
            {
                return mWeek;
            }
            set
            {
                mWeek = value;
                mValues[TriggerTag.Week] = mWeek.ToString();
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
                mValues[TriggerTag.TimeTag] = mGameState.ToString();
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
                mValues[TriggerTag.Location] = mLocation.ToString();
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
            if (!mValues.ContainsKey(trigger.key))
                return false;
            if (trigger.equals)
            {
                if (mValues[trigger.key] == trigger.value)
                    return true;
                else
                    return false;
            }
            else
            {
                if (trigger.not)//�ж�����
                {
                    if (int.Parse(mValues[trigger.key]) > int.Parse(trigger.value))
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (int.Parse(mValues[trigger.key]) < int.Parse(trigger.value))
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
            if (console.Length == 2)
            {
                eventData.value1= console[1];
            }
            if (console.Length == 3)
            {
                eventData.value1 = console[1];
                eventData.value2 = console[2];
            }
            if (console.Length == 4)
            {
                eventData.value1 = console[1];
                eventData.value2 = console[2];
                eventData.value2 = console[3];
            }
            return RunEvent(eventData);
        }

        public bool RunEvent(EventData eventData)
        {
            switch (eventData.eventTag)
            {
                case EventTag.Play:
                    GameEntry.Dialog.PlayStory(eventData.value1);
                    return true;
                case EventTag.AddMoney:
                    GameEntry.Player.Money += int.Parse(eventData.value1);
                    return true;
                case EventTag.AddFavor:
                    GameEntry.Cat.Favor += int.Parse(eventData.value1);
                    return true;
                case EventTag.AddLove:
                    GameEntry.Cat.Love+= int.Parse(eventData.value1);  
                    return true;
                case EventTag.AddEnergy:
                    GameEntry.Player.Energy += int.Parse(eventData.value1);
                    return true;
                case EventTag.AddItem:
                    GameEntry.Utils.AddPlayerItem(new ItemData((ItemTag)Enum.Parse(typeof(ItemTag), eventData.value1)), int.Parse(eventData.value2));
                    return true;
                case EventTag.AddFlag:
                    GameEntry.Utils.AddFlag(eventData.value1);
                    return true;
                case EventTag.RemoveFlag:
                    GameEntry.Utils.RemoveFlag(eventData.value1);
                    return true;
                case EventTag.NextDay://重写逻辑
                    GameEntry.Player.Day++;
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
                    return true;
                case EventTag.PlayBgm:
                    return true;
                case EventTag.EndGame:
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu));
                    return true;
                case EventTag.AddDay:
                    GameEntry.Player.Day += int.Parse(eventData.value1);
                    return true;
                case EventTag.Rent:
                    //GameEntry.Utils.Rent=int.Parse(eventData.value1);
                    return true; 
                case EventTag.AddFriend:
                    GameEntry.Utils.AddFriendFavor(eventData.value1, int.Parse(eventData.value2));
                    return true; 
                case EventTag.AddRecipe:
                    GameEntry.Player.AddRecipe(int.Parse(eventData.value1));
                    return true; 
                case EventTag.AddScene:
                    GameEntry.Utils.outingSceneStates.Add((OutingSceneState)int.Parse(eventData.value1));
                    return true;
                case EventTag.Test:
                    GameEntry.Event.FireNow(this, ValueEventArgs.Create(PropertyTag.Energy,"成功"));
                    return true;
                case EventTag.AddBuff:
                    GameEntry.Buff.AddBuff(int.Parse(eventData.value1));
                    return true;
                case EventTag.RemoveBuff:
                    GameEntry.Buff.RemoveBuff(int.Parse(eventData.value1));
                    return true;
            }
            return false;
        }
        public void UpdateData(TriggerTag key, string value)
        {
            mValues[key] = value;
            UpdateData();
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
        public List<PlayerItemData> items=new List<PlayerItemData>();
    }
}