using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
        //�涨����0��Ĭ���Ǻ����л�����ÿһ��֮����л�������1�ſ�ʼ����ÿһ����л�
        public List<Sprite> changeSprites= new List<Sprite>();

        public bool pickUp = false;
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
                _values[TriggerTag.Day] = mPlayerData.ap.ToString();
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
                _values[TriggerTag.Money] = mCharData.family.ToString();
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
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
        public bool Check(TriggerData triggerData)
        {
            return Check(triggerData.trigger);
        }
        public bool Check(Trigger trigger)
        {
            if (trigger.key == TriggerTag.Flag)
                return _flags.Contains(trigger.value);
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
        public void RunEvent(EventData eventData)
        {
            switch (eventData.eventTag)
            {
                case EventTag.AddFavor:
                    break;
                case EventTag.AddFlag:
                    break;
                case EventTag.RemoveFlag:
                    break;
                case EventTag.AddMoney:
                    break;
            }
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
        public List<Item> items=new List<Item>();
        //public int time;
    }
}