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
        private CharData mCharData=new CharData();

        private void Start()
        {
            catStateDatas.Clear();
            CatStateSO[] catStateSOs = Resources.LoadAll<CatStateSO>("CatStateData");
            foreach (CatStateSO catStateSO in catStateSOs)
            {
                catStateDatas.Add(catStateSO.catStateData);
            }
        }
        public CharData GetSaveData()
        { 
            return mCharData;
        }
        public void LoadData(CharData charData)
        {
            mCharData = charData;
        }
        public int Closet
        {
            get
            {
                return mCharData.closet;
            }
            set
            {
                mCharData.closet = value;
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Wisdom
        {
            get
            {
                return mCharData.wisdom;
            }
            set
            {
                mCharData.wisdom = value;
                GameEntry.Utils.AddValue(TriggerTag.Wisdom, mCharData.wisdom.ToString());
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Charm
        {
            get
            {
                return mCharData.charm;
            }
            set
            {
                mCharData.charm = value;
                GameEntry.Utils.AddValue(TriggerTag.Charm, mCharData.charm.ToString());
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int Stamina
        {
            get
            {
                return mCharData.stamina;
            }
            set
            {
                mCharData.stamina = value;
                GameEntry.Utils.AddValue(TriggerTag.Stamina, mCharData.stamina.ToString());
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
                GameEntry.Utils.AddValue(TriggerTag.Favor, mCharData.favor.ToString());
                GameEntry.Event.FireNow(this, CharDataEventArgs.Create(mCharData));
            }
        }
        public int CharmLevel
        {
            get
            {
                return mCharData.charm / 40;
            }
        }
        public int WisdomLevel
        {
            get
            {
                return mCharData.wisdom / 40;
            }
        }
        public int StaminaLevel
        {
            get
            {
                return mCharData.stamina / 40;
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
        public CharData charData;
        public List<DialogueGraph> dialogues;
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
        Augur
    }
}
