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
        public List<Sprite> nodeSprites = new List<Sprite>();
        public List<Sprite> nodeImage= new List<Sprite>();
        public List<Sprite> ItemSprites = new List<Sprite>();
        //�涨����0��Ĭ���Ǻ����л�����ÿһ��֮����л�������1�ſ�ʼ����ÿһ����л�
        public List<Sprite> changeSprites= new List<Sprite>();

        public List<ShopItemData> bookstoreItemDatas= new List<ShopItemData>();
        public List<ShopItemData> greengrocerItemDatas = new List<ShopItemData>();
        public List<MusicItemData> musicHallItemDatas = new List<MusicItemData>();
        public List<ShopItemData> glassItemDatas = new List<ShopItemData>();
        public List<ShopItemData> restaurantItemDatas = new List<ShopItemData>();
        public List<ShopItemData> bakeryItemDatas = new List<ShopItemData>();
        public List<ItemData> itemDatas = new List<ItemData>();
        public Dictionary<int, int> shopItems = new Dictionary<int, int>();
        public List<Sprite> closets = new List<Sprite>();
        public int closet;
        public string actionName; 

        public bool pickUp = false;
        public int musicHallItemID;
        public int changeMusicHallItemID;
        public bool musicChangeFlag;
        private int mCarfSort = 99;

        public int CartSort
        {
            get
            {
                return mCarfSort++;
            }
        }

        //���ݹ���������
        private Dictionary<TriggerTag,string> _values= new Dictionary<TriggerTag,string>();
        private List<string> _flags= new List<string>();

        private List<WorkData> mWorkDatas= new List<WorkData>();

        private CharData mCharData=new CharData();
        private PlayerData mPlayerData = new PlayerData();
        private OutingSceneState mLocation;
        private TimeTag mTimeTag;
        private Week mWeek;
        private BehaviorTag mBehaviorTag;

        public bool outingBefore;

        public Dictionary<string ,CharSO> chars= new Dictionary<string ,CharSO>();
        public Dictionary<string, int> friends = new Dictionary<string, int>();

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
        public void AddPlayerItem(ItemData itemData, int num,bool equip)
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
                mPlayerData= value;
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
                _flags= value;
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
                mWorkDatas= value;
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
                mBehaviorTag= value;
                _values[TriggerTag.BehaviorTag]=mBehaviorTag.ToString();
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
                mWeek= value;
                _values[TriggerTag.Week] = mWeek.ToString();
            }
        }
        public TimeTag TimeTag
        {
            get
            {
                return mTimeTag;
            }
            set
            { 
                mTimeTag= value;
                _values[TriggerTag.TimeTag] = mTimeTag.ToString();
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
                mLocation= value;
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
                if (value > MaxEnergy)
                    mPlayerData.energy = MaxEnergy;
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
                Week = (Week)((Day + 20) % 7);
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
                _values[TriggerTag.Rent]=mPlayerData.rent.ToString();
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
        public void AddFlag(string flag)
        { 
            if(!_flags.Contains(flag))
                _flags.Add(flag);
        }
        public void RemoveFlag(string flag)
        { 
            if(_flags.Contains(flag))
                _flags.Remove(flag);
        }

        public void ClearFlag()
        {
            _flags.Clear();
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
                return _flags.Contains(trigger.value)!=trigger.not;
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
                    TimeTag = TimeTag.Morning;
                    GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
                    GameEntry.Utils.Day++;
                    GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, GameEntry.Utils.Day);//用这个this传参来调整黑幕
                    GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Work));
                    return true;
                case EventTag.PlayBgm:
                    return true;
                case EventTag.EndGame:
                    GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Menu));
                    return true;
                case EventTag.AddDay:
                    GameEntry.Utils.Day += int.Parse(eventData.value);
                    return true;
                case EventTag.AddAction:
                    GameEntry.Utils.actionName= eventData.value;
                    return true;
                case EventTag.Rent:
                    GameEntry.Utils.Rent=int.Parse(eventData.value);
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
        public List<PlayerItemData> items=new List<PlayerItemData>();
        //public int time;
    }
}