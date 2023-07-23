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
        public List<Sprite> nodeSprites = new List<Sprite>();
        //规定，第0张默认是黑屏切换（即每一段之间的切换），第1张开始则是每一天的奇幻
        public List<Sprite> changeSprites= new List<Sprite>();

        //数据管理器部分
        private Dictionary<TriggerTag,string> _values= new Dictionary<TriggerTag,string>();
        private Dictionary<string,int> mCharFavor= new Dictionary<string,int>();
        private List<string> _flags= new List<string>();

        private int mFavor = 0;
        private CharData mCharData=null;
        private OrderData mOrderData = null;
        private OrderData mSupplyData=null;
        private LevelData mLevelData= null;
        private int mMoney = 0;
        public int Money
        {
            get
            {
                return mMoney;
            }
            set
            { 
                mMoney= value;
                _values[TriggerTag.Money] = mMoney.ToString();
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
                mCharData= value;
                mFavor = mCharFavor[mCharData.charName];
                _values[TriggerTag.Davor] = mFavor.ToString();
            }
        }
        public int Favor
        {
            get
            {
                return mFavor;
            }
            set
            { 
                mFavor= value;
                mCharFavor[mCharData.charName] = mFavor;
            }
        }
        public OrderData OrderData
        {
            get 
            { 
                return mOrderData; 
            }
            set
            { 
                mOrderData= value;
                _values[TriggerTag.Coffee]=mOrderData.NodeTag.ToString();
            }
        }
        public LevelData LevelData
        { 
            get
            { 
                return mLevelData;
            }
            set
            { 
                mLevelData= value;
                ActionGraph actionGraph = Resources.Load<ActionGraph>(string.Format("ActionData/{0}", mLevelData.ActionGraph));
                mCharData = actionGraph.charSO.charData;

                _values[TriggerTag.Day] = mLevelData.Day.ToString();
                _values[TriggerTag.Index] = mLevelData.Index.ToString();
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
            if (trigger.And.Count != 0)
            {
                foreach (Trigger tr in trigger.And)
                {
                    if (!Check(tr))
                    {
                        return false;
                    }
                }
            }
            if (trigger.OR.Count != 0)
            {
                foreach (Trigger tr in trigger.OR)
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
                if (trigger.not)//判断至少
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
    }
}