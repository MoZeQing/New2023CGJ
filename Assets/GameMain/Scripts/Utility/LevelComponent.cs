using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LevelComponent : GameFrameworkComponent
    {
        private List<LevelData> m_LevelDatas = new List<LevelData>();
        private List<LevelData> m_LoadedLevelDatas = new List<LevelData>();

        public List<string> LoadedLevels
        {
            get
            {
                List<string> newLevel = new List<string>();
                foreach (LevelData levelData in m_LoadedLevelDatas)
                {
                    newLevel.Add(levelData.levelName);
                }
                return newLevel;
            }
        }

        public int GetLoadedLevelCount
        {
            get { return m_LoadedLevelDatas.Count; }
        }
        public int GetAllLevelCount
        {
            get { return m_LevelDatas.Count; }
        }

        public bool InDialog
        {
            get;
            set;
        }

        public void LoadAllLevel()
        {
            LoadAllLevelSO();
            LoadAllLevelDataTable();
        }
        public void LoadAllLevelSO()
        {
            foreach (LevelSO levelSO in Resources.LoadAll<LevelSO>("LevelData"))
            {
                if (levelSO.unLoad)
                    continue;
                m_LevelDatas.Add(new LevelData(levelSO));
            }
        }

        public void LoadAllLevelDataTable()
        {
            DRLevel[] levels=GameEntry.DataTable.GetDataTable<DRLevel>().GetAllDataRows();
            foreach (DRLevel dRLevel in levels)
            {
                m_LevelDatas.Add(new LevelData(dRLevel));
            }
        }

        public LevelData GetLevelData()
        {
            List<LevelData> levels = new List<LevelData>();
            foreach (LevelData level in m_LoadedLevelDatas)
            {
                if (GameEntry.Utils.Check(level.trigger))
                {
                    levels.Add(level);
                }
            }
            if (levels.Count != 0)
            {
                LevelData levelData = levels[UnityEngine.Random.Range(0, levels.Count)];
                GameEntry.Utils.AddFlag(levelData.levelName);
                if (m_LoadedLevelDatas.Contains(levelData))
                    m_LoadedLevelDatas.Remove(levelData);
                return levelData;
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
            foreach (LevelData levelData in m_LoadedLevelDatas)
            {
                if (levelData.levelName == levelName)
                {
                    return levelData;
                }
            }
            return null;
        }
        public void LoadGame(List<string> levelDatas)
        {
            m_LoadedLevelDatas.Clear();
            foreach (LevelData levelData in m_LevelDatas)
            {
                if (!m_LoadedLevelDatas.Contains(levelData))
                {
                    m_LoadedLevelDatas.Add(levelData);
                }
            }
        }

        public void LoadGame()
        {
            m_LoadedLevelDatas = new List<LevelData>(m_LevelDatas);
        }
    }
}

