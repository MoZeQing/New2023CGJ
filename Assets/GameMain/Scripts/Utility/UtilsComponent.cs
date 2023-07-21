using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class UtilsComponent : GameFrameworkComponent
    {
        public List<Sprite> nodeSprites = new List<Sprite>();

        //数据管理器部分
        private Dictionary<string,string> _values= new Dictionary<string,string>();
        private List<string> _flags= new List<string>();

        public bool Check(Trigger trigger)
        {
            if (trigger.key == "flag")
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
            if (trigger.key == "" && trigger.value == "")
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