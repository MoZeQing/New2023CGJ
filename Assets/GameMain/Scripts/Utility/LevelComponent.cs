using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LevelComponent : GameFrameworkComponent
    {
        private List<LevelSO> mLevelSOs = new List<LevelSO>();
        private List<LevelSO> mLoadedLevelSOs = new List<LevelSO>();

        public List<string> LoadedLevels
        {
            get
            {
                List<string> newLevel = new List<string>();
                foreach (LevelSO levelSO in mLoadedLevelSOs)
                {
                    newLevel.Add(levelSO.name);
                }
                return newLevel;
            }
        }

        public bool InDialog
        {
            get;
            set;
        }

        private void OnEnable()
        {
            foreach (LevelSO levelSO in Resources.LoadAll<LevelSO>("LevelData"))
            {
                if (levelSO.unLoad)
                    continue;
                mLevelSOs.Add(levelSO);
            }
        }
        public LevelData GetLevelData()
        {
            List<LevelSO> levels = new List<LevelSO>();
            foreach (LevelSO level in mLoadedLevelSOs)
            {
                if (GameEntry.Utils.Check(level.trigger) && !level.isRandom)
                {
                    levels.Add(level);
                }
            }
            if (levels.Count != 0)
            {
                LevelSO levelSO = levels[UnityEngine.Random.Range(0, levels.Count)];
                GameEntry.Utils.AddFlag(levelSO.name);
                if (mLoadedLevelSOs.Contains(levelSO))
                    mLoadedLevelSOs.Remove(levelSO);
                return levelSO.levelData;
            }
            else
            {
                return null;
            }
        }
        public LevelData GetRandLevelData()
        {
            //生成随机关卡数据
            return null;
        }
        public LevelData GetLevelData(string levelName)
        {
            foreach (LevelSO levelSO in mLoadedLevelSOs)
            {
                if (levelSO.name == levelName)
                {
                    return levelSO.levelData;
                }
            }
            return null;
        }
        public void LoadGame( List<string> levelData)
        {
            mLoadedLevelSOs.Clear();
            foreach (LevelSO levelSO in mLevelSOs)
            {
                if (levelData.Contains(levelSO.name))
                {
                    mLoadedLevelSOs.Add(levelSO);
                }
            }
        }

        public void LoadGame()
        {
            mLoadedLevelSOs = new List<LevelSO>(mLevelSOs);
        }
    }
}

