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
        public OutingSceneState outSceneState;
        public List<Sprite> closets = new List<Sprite>();
        public int closet;
        public string actionName;

        public bool pickUp = false;
        public int musicHallItemID;
        public int changeMusicHallItemID;
        public bool musicChangeFlag;
        private int mCarfSort = 99;
        public float OrderPower { get; set; } = 1f;
        public float PricePower { get; set; } = 1f;

        public List<OutingSceneState> outingSceneStates = new List<OutingSceneState>();

        public int CartSort
        {
            get
            {
                return mCarfSort++;
            }
        }
        //接下来尽可能使得utils只用于存储资源信息，而数据信息都给其它的类
        //���ݹ���������
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

        public bool outingBefore;

        public Dictionary<string, RecipeData> recipes = new Dictionary<string, RecipeData>();
        public Dictionary<string, CharSO> chars = new Dictionary<string, CharSO>();

        public void AddFriendFavor(string name, int favor)
        {
            if (!_friends.ContainsKey(name))
                _friends.Add(name, favor);
            else
                _friends[name] += favor;
            //硬编码转换
            if (name == "money") _values[TriggerTag.FMoney] = _friends[name].ToString();
            if (name == "regular") _values[TriggerTag.FRegular] = _friends[name].ToString();
            if (name == "dog") _values[TriggerTag.FDog] = _friends[name].ToString();
            if (name == "fiction") _values[TriggerTag.FFiction] = _friends[name].ToString();
            if (name == "courier") _values[TriggerTag.FCourier] = _friends[name].ToString();
            if (name == "witch") _values[TriggerTag.FWitch] = _friends[name].ToString();
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

        //����WorkData
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
                _values[TriggerTag.BehaviorTag] = mBehaviorTag.ToString();
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
                _values[TriggerTag.Week] = mWeek.ToString();
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
        //����PlayerData
        public int Money
        {
            get
            {
                return mPlayerData.money;
            }
            set
            {
                mPlayerData.money = value;
                _values[TriggerTag.Money] = mPlayerData.money.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Energy
        {
            get
            {
                return mPlayerData.energy;
            }
            set
            {
                BuffData buffData = GameEntry.Buff.GetBuff();
                if (value > MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus)
                    mPlayerData.energy = (int)(MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus);
                else
                    mPlayerData.energy = value;
                _values[TriggerTag.Energy] = mPlayerData.energy.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int MaxEnergy
        {
            get
            {
                return mPlayerData.maxEnergy;
            }
            set
            {
                mPlayerData.maxEnergy = value;
                _values[TriggerTag.MaxEnergy] = mPlayerData.maxEnergy.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int MaxAp
        {
            get
            {
                return mPlayerData.maxAp;
            }
            set
            {
                mPlayerData.maxAp = value;
                _values[TriggerTag.MaxAp] = mPlayerData.maxAp.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Ap
        {
            get
            {
                return mPlayerData.ap;
            }
            set
            {
                mPlayerData.ap = value;
                _values[TriggerTag.Ap] = mPlayerData.ap.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Day
        {
            get
            {
                return mPlayerData.day;
            }
            set
            {
                mPlayerData.day = value;
                Week = (Week)(Day % 7);
                Debug.Log(Week);
                _values[TriggerTag.Day] = mPlayerData.day.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Rent
        {
            get
            {
                return mPlayerData.rent;
            }
            set
            {
                mPlayerData.rent = value;
                _values[TriggerTag.Rent] = mPlayerData.rent.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        //����CharData
        public int Mood
        {
            get
            {
                return mCharData.mood;
            }
            set
            {
                mCharData.mood = value;
                _values[TriggerTag.Mood] = mCharData.mood.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Hope
        {
            get
            {
                return mCharData.hope;
            }
            set
            {
                mCharData.hope = value;
                _values[TriggerTag.Hope] = mCharData.hope.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Favor
        {
            get
            {
                return mCharData.favor;
            }
            set
            {
                mCharData.favor = value;
                _values[TriggerTag.Favor] = mCharData.favor.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Love
        {
            get
            {
                return mCharData.love;
            }
            set
            {
                mCharData.love = value;
                _values[TriggerTag.Love] = mCharData.love.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Family
        {
            get
            {
                return mCharData.family;
            }
            set
            {
                mCharData.family = value;
                _values[TriggerTag.Family] = mCharData.family.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Ability
        {
            get
            {
                return mCharData.ability;
            }
            set
            {
                mCharData.ability = value;
                _values[TriggerTag.Ability] = mCharData.ability.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
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
            EventData eventData = new EventData();
            EventTag eventEnum = (EventTag)Enum.Parse(typeof(EventTag), console[0]);
            eventData.eventTag = eventEnum;
            eventData.value = console[1];
            return RunEvent(eventData);
        }

        public bool RunEvent(EventData eventData)
        {
            switch (eventData.eventTag)
            {
                case EventTag.Play:
                    GameEntry.Dialog.PlayStory(eventData.value);
                    return true;
                case EventTag.AddMoney:
                    GameEntry.Utils.Money += int.Parse(eventData.value);
                    Debug.Log(int.Parse(eventData.value));
                    return true;
                case EventTag.AddFavor:
                    GameEntry.Utils.Favor += int.Parse(eventData.value);
                    return true;
                case EventTag.AddMood:
                    GameEntry.Utils.Mood+= int.Parse(eventData.value);
                    return true;
                case EventTag.AddHope:
                    GameEntry.Utils.Hope += int.Parse(eventData.value);
                    return true;
                case EventTag.AddLove:
                    GameEntry.Utils.Love+= int.Parse(eventData.value);  
                    return true;
                case EventTag.AddAbility:
                    GameEntry.Utils.Love += int.Parse(eventData.value);
                    return true;
                case EventTag.AddEnergy:
                    GameEntry.Utils.Energy += int.Parse(eventData.value);
                    return true;
                case EventTag.AddAp:
                    GameEntry.Utils.Ap+= int.Parse(eventData.value);
                    return true;
                case EventTag.AddItem:
                    GameEntry.Utils.AddPlayerItem(new ItemData((ItemTag)Enum.Parse(typeof(ItemTag), eventData.value)),1);
                    return true;
                case EventTag.AddFlag:
                    GameEntry.Utils.AddFlag(eventData.value);
                    return true;
                case EventTag.RemoveFlag:
                    GameEntry.Utils.RemoveFlag(eventData.value);
                    return true;
                case EventTag.NextDay:
                    GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
                    GameEntry.Utils.Day++;
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
                    return true;
                case EventTag.PlayBgm:
                    return true;
                case EventTag.EndGame:
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu));
                    return true;
                case EventTag.AddDay:
                    GameEntry.Utils.Day += int.Parse(eventData.value);
                    return true;
                case EventTag.AddAction:
                    GameEntry.Utils.actionName= eventData.value;
                    return true;
                case EventTag.Rent:
                    GameEntry.Utils.Rent=int.Parse(eventData.value);
                    return true; 
                case EventTag.AddFriend:
                    string[] strings= eventData.value.Split('|');
                    GameEntry.Utils.AddFriendFavor(strings[0], int.Parse(strings[1]));
                    return true; 
                case EventTag.AddRecipe:
                    GameEntry.Player.AddRecipe(int.Parse(eventData.value));
                    return true; 
                case EventTag.AddScene:
                    GameEntry.Utils.outingSceneStates.Add((OutingSceneState)int.Parse(eventData.value));
                    return true;
                case EventTag.Test:
                    GameEntry.Event.FireNow(this, ValueEventArgs.Create(PropertyTag.Energy,"成功"));
                    return true;
                case EventTag.AddBuff:
                    GameEntry.Buff.AddBuff(int.Parse(eventData.value));
                    return true;
                case EventTag.RemoveBuff:
                    GameEntry.Buff.RemoveBuff(int.Parse(eventData.value));
                    return true;
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
        public List<PlayerItemData> items=new List<PlayerItemData>();
        //public int time;
    }
}