using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CatComponent : GameFrameworkComponent
    {
        //猫猫状态数据，其中0为默认的状态，也是当所有条件都不满足时的状态，一般作为溢出状态
        private List<CatStateData> catStateDatas = new List<CatStateData>();
        private CatStateData catState;
        private Dictionary<BehaviorTag, BehaviorData> behaviors = new Dictionary<BehaviorTag, BehaviorData>();
        private CatData mCatData=new CatData();

        private void Start()
        {
            catStateDatas.Clear();
            CatStateSO[] catStateSOs = Resources.LoadAll<CatStateSO>("CatStateData");
            foreach (CatStateSO catStateSO in catStateSOs)
            {
                catStateDatas.Add(catStateSO.catStateData);
            }
        }
        public CatData GetSaveData()
        { 
            return mCatData;
        }
        public void LoadData(CatData charData)
        {
            mCatData = charData;
        }
        public int Closet
        {
            get
            {
                return mCatData.closet;
            }
            set
            {
                mCatData.closet = value;
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Wisdom
        {
            get
            {
                return mCatData.wisdom;
            }
            set
            {
                mCatData.wisdom = value;
                GameEntry.Utils.AddValue(TriggerTag.Wisdom, mCatData.wisdom.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Charm
        {
            get
            {
                return mCatData.charm;
            }
            set
            {
                mCatData.charm = value;
                GameEntry.Utils.AddValue(TriggerTag.Charm, mCatData.charm.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Stamina
        {
            get
            {
                return mCatData.stamina;
            }
            set
            {
                mCatData.stamina = value;
                GameEntry.Utils.AddValue(TriggerTag.Stamina, mCatData.stamina.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Favor
        {
            get
            {
                return mCatData.favor;
            }
            set
            {
                mCatData.favor = value;
                GameEntry.Utils.AddValue(TriggerTag.Favor, mCatData.favor.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int CharmLevel
        {
            get
            {
                return mCatData.charm / 40;
            }
        }
        public int WisdomLevel
        {
            get
            {
                return mCatData.wisdom / 40;
            }
        }
        public int StaminaLevel
        {
            get
            {
                return mCatData.stamina / 40;
            }
        }
        public CatStateData GetCatState()
        {
            UpdateState();
            return catState;
        }
        public BehaviorData GetBehavior(BehaviorTag behaviorTag)
        {
            UpdateState();
            return behaviors[behaviorTag];
        }
        public void UpdateState()
        {
            //如果当前有效则直接启动
            if (catState!=null&&GameEntry.Utils.Check(catState.trigger))
                return;
            for (int i=0;i<catStateDatas.Count;i++)
            {
                CatStateData stateData = catStateDatas[i];
                if (GameEntry.Utils.Check(stateData.trigger))
                {
                    catState = stateData;
                    behaviors.Clear();
                    foreach (BehaviorData behavior in catState.behaviors)
                    {
                        behaviors.Add(behavior.behaviorTag, behavior);
                    }
                    continue;
                }
            }
        }
    }
    [System.Serializable]
    public class CatStateData
    {
        public ParentTrigger trigger;
        public List<BehaviorData> behaviors;
    }
    [System.Serializable]
    public class BehaviorData
    {
        public BehaviorTag behaviorTag;
        public string behaviorName;
        public PlayerData playerData;
        public CatData charData;
        public List<DialogueGraph> dialogues;
    }
    [System.Serializable]
    public class CatData
    {
        public int closet;
        public int favor;//好感度
        public int stamina;//体能
        public int wisdom;//智慧
        public int charm;//魅力
        public int StaminaLevel
        {
            get
            {
                return Mathf.Min(stamina / 40 + 1, 4);
            }
        }//体能
        public int WisdomLevel
        {
            get
            {
                return Mathf.Min(wisdom / 40 + 1, 4);
            }
        }//智慧
        public int CharmLevel
        {
            get
            {
                return Mathf.Min(charm / 40 + 1, 4);
            }
        }//魅力

        public CatData() { }

        public Dictionary<ValueTag, int> GetValueTag(Dictionary<ValueTag, int> dic)
        {
            if (favor != 0)
                dic.Add(ValueTag.Favor, favor);
            if (stamina != 0)
                dic.Add(ValueTag.Stamina, stamina);
            if (wisdom != 0)
                dic.Add(ValueTag.Wisdom, wisdom);
            if (charm != 0)
                dic.Add(ValueTag.Charm, charm);
            return dic;
        }
    }
    public enum BehaviorTag
    {
        Click,
        Clean,
        Play,
        Talk,
        Bath,
        TV,
        Story,
        Touch,
        Rest,
        Sleep,
        Morning,
        Hug,
        Teach,
        Sport,
        Read,
        Augur,
        Out
    }
}
