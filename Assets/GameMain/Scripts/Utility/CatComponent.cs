using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CatComponent : GameFrameworkComponent
    {
        private CatData mCatData;
        //猫猫状态数据，其中0为默认的状态，也是当所有条件都不满足时的状态，一般作为溢出状态
        private List<CatStateData> catStateDatas = new List<CatStateData>();
        private CatStateData catState;
        private Dictionary<BehaviorTag, BehaviorData> behaviors = new Dictionary<BehaviorTag, BehaviorData>();

        private void Start()
        {
            catStateDatas.Clear();
            CatStateSO[] catStateSOs = Resources.LoadAll<CatStateSO>("CatStateData");
            foreach (CatStateSO catStateSO in catStateSOs)
            {
                catStateDatas.Add(catStateSO.catStateData);
            }
        }
        public int Favor
        {
            get
            {
                return mCatData.favour;
            }
            set 
            { 
                mCatData.favour = value;
                GameEntry.Utils.UpdateData(TriggerTag.Favor, mCatData.favour.ToString());
            }
        }
        public int Family
        {
            get
            {
                return mCatData.family;
            }
            set
            {
                mCatData.family = value;
                GameEntry.Utils.UpdateData(TriggerTag.Favor, mCatData.family.ToString());
            }
        }
        public int Love
        {
            get
            {
                return mCatData.love;
            }
            set
            {
                mCatData.love = value;
                GameEntry.Utils.UpdateData(TriggerTag.Favor, mCatData.love.ToString());
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
    public class CatData
    {
        public int favour = 0;
        public int love = 0;
        public int family = 0;
    }
    [System.Serializable]
    public class BehaviorData
    {
        public BehaviorTag behaviorTag;
        public string behaviorName;
        public PlayerData playerData;
        public CatData catData;
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
        Teach
    }
}
