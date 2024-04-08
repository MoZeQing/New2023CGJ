using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class BuffComponent : GameFrameworkComponent
    {
        private BuffData buffData=new BuffData();
        private List<int> buffs= new List<int>();

        public List<int> GetData()
        {
            return buffs;
        }
        public BuffData GetBuff()
        {
            if(buffData==null)
                buffData = new BuffData();
            return buffData;
        }
        public void InitBuff()
        {
            buffData = new BuffData();
            buffs.Clear();
        }
        public void InitBuff(List<int> buffs)
        {
            foreach (int buff in buffs)
            {
                AddBuff(buff);
            }
            this.buffs = buffs;
        }
        public void AddBuff(int buffIndex)
        {
            buffData.AddBuff(buffIndex);
            buffs.Add(buffIndex);
        }

        public void AddBuff(List<int> buffs)
        {
            foreach (int buff in buffs)
            { 
                AddBuff(buff);
            }
            this.buffs.AddRange(buffs);
        }

        public void RemoveBuff(int buffIndex) 
        {
            buffData.RemoveBuff(buffIndex);
            this.buffs.Remove(buffIndex);
        }
    }
}