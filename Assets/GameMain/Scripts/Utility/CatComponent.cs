using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CatComponent : GameFrameworkComponent
    {
        private int m_Favour = 0;
        private int m_Mood = 0;
        private int m_Hope = 0;
        private int m_Love = 0;
        private int m_Family = 0;

        private ActionGraph m_ActionGraph = null;
        private ActionNode m_ActionNode = null;

        public int Family
        {
            get
            {
                return m_Family;
            }
            private set { }
        }
        public int Love
        {
            get
            {
                return m_Love;
            }
            private set { }
        }
        public int Hope
        {
            get
            {
                return m_Hope;
            }
            private set { }
        }
        public int Favour
        {
            get
            {
                return m_Favour;
            }
            private set { }
        }

        public int Mood
        {
            get
            {
                return m_Mood;
            }
            private set { }
        }
        public void SetBehavior(TriggerNode triggerNode)
        { 
            
        }
        public void SetBehavior(List<TriggerData> triggers)
        {
            List<TriggerData> ans = new List<TriggerData>();
            foreach (TriggerData trigger in triggers)
            {
                if (GameEntry.Utils.Check(trigger))
                    ans.Add(trigger);
            }
            if (ans.Count == 0)
            {
                //Debug.LogError("错误，不存在合法的剧情，请检查{0}", m_ActionGraph.name.ToString());
                return;
            }
            else
            {
                //SetBehavior(ans[Random.Range(0, ans.Count - 1)]);
            }
        }
        public void SetBehavior(BehaviorTag behaviorTag)
        {

            switch (behaviorTag)
            {
                case BehaviorTag.Click:
                    //SetBehavior(m_ActionNode.click);
                    break;
                case BehaviorTag.Talk:
                    break;
                case BehaviorTag.Touch:
                    break;
                case BehaviorTag.Play:
                    break;
                case BehaviorTag.Story:
                    break;
                case BehaviorTag.Sleep:
                    break;
            }
        }
    }
    [System.Serializable]
    public class CharStateData
    {
        public int favour = 0;
        public int mood = 0;
        public int hope = 0;
        public int love = 0;
        public int family = 0;
    }
    [System.Serializable]
    public class PlayerData
    {
        public int energy;
        public int money;
        public int time;
    }
    public class Behavior
    {
        public BehaviorTag action;
        public CatData catData;
    }

    public enum BehaviorTag
    {
        Click,
        Talk,
        Touch,
        Play,
        Story,
        Sleep
    }
}
