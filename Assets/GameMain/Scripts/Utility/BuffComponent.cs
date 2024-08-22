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
        private BuffData mBuffData=new BuffData();

        public BuffData GetSaveData()
        {
            return mBuffData;
        }
        public void LoadData(BuffData buffData)
        { 
            mBuffData= buffData;
        }
        public BuffData GetBuff()
        {
            if(mBuffData==null)
                mBuffData = new BuffData();
            return mBuffData;
        }
        public void InitBuff()
        {
            mBuffData = new BuffData();
            mBuffData.buffs.Clear();
        }
        public void InitBuff(List<int> buffs)
        {
            foreach (int buff in buffs)
            {
                AddBuff(buff);
            }
            this.mBuffData.buffs = buffs;
        }
        public void AddBuff(int buffIndex)
        {
            mBuffData.AddBuff(buffIndex);
            mBuffData.buffs.Add(buffIndex);
        }

        public void AddBuff(List<int> buffs)
        {
            foreach (int buff in buffs)
            { 
                AddBuff(buff);
            }
            this.mBuffData.buffs.AddRange(buffs);
        }

        public void RemoveBuff(int buffIndex) 
        {
            mBuffData.RemoveBuff(buffIndex);
            this.mBuffData.buffs.Remove(buffIndex);
        }
    }
}