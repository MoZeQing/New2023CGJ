using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class PlayerComponent : GameFrameworkComponent
    {
        public PlayerData mPlayerData  = new PlayerData();

        private void Start()
        {
            mPlayerData.recipes = new List<int>();
            mPlayerData.items = new List<PlayerItemData>();
        }
        public PlayerData GetSaveData()
        {
            return mPlayerData;
        }
        public void LoadData(PlayerData playerData)
        { 
            mPlayerData= playerData;
        }
        public void ClearPlayerItem()
        {
            mPlayerData.items.Clear();
        }
        public void AddPlayerItem(ItemData itemData, int num)
        {
            if (GetPlayerItem(itemData.itemTag) == null)
            {
                mPlayerData.items.Add(new PlayerItemData(itemData, num));
            }
            else
            {
                GetPlayerItem(itemData.itemTag).itemNum += num;
            }
        }
        public void AddPlayerItem(ItemData itemData, int num, bool equip)
        {
            if (GetPlayerItem(itemData.itemTag) == null)
            {
                PlayerItemData playerItem = new PlayerItemData(itemData, num);
                playerItem.equiping = equip;
                mPlayerData.items.Add(playerItem);
            }
            else
            {
                GetPlayerItem(itemData.itemTag).itemNum += num;
            }
        }
        public PlayerItemData GetPlayerItem(ItemTag itemTag)
        {
            foreach (PlayerItemData itemData in mPlayerData.items)
            {
                if (itemData.itemTag == itemTag)
                    return itemData;
            }
            return null;
        }

        public void ClearRecipes()
        {
            mPlayerData.recipes.Clear();
        }
        public void AddRecipes(string[] indexs)
        {
            foreach (string index in indexs)
            {
                AddRecipe(int.Parse(index));
            }
        }
        public void AddRecipes(int[] indexs)
        {
            foreach (int index in indexs)
            { 
                AddRecipe(index);
            }
        }

        public void AddRecipe(int index)
        {
            if (HasRecipe(index))
                return;
            mPlayerData.recipes.Add(index);
        }
        public void RemoveRecipe(int index)
        {
            if(HasRecipe(index))
                mPlayerData.recipes.Remove(index);
        }
        public void LoadGame(SaveLoadData saveLoadData)
        {

        }
        public bool HasRecipe(int id)
        {
            return mPlayerData.recipes.Contains(id);
        }

        public bool HasCoffeeRecipe(NodeTag nodeTag)
        {
            foreach (int id in mPlayerData.recipes)
            {
                RecipeData recipe = new RecipeData(GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(id));
                if(recipe.products.Contains(nodeTag))
                    return true;
            }
            return false;
        }
        public int GuideId
        {
            get
            {
                return mPlayerData.guideID;
            }
            set
            { 
                mPlayerData.guideID = value;
            }
        }
        public int Money
        {
            get
            {
                return mPlayerData.money;
            }
            set
            {
                mPlayerData.money = value;
                GameEntry.Utils.AddValue(TriggerTag.Money, mPlayerData.money.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Energy
        {
            get
            {
                return mPlayerData.energy;
            }
            set
            {
                BuffData buffData = GameEntry.Buff.GetBuff();
                if (value > MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus)
                    mPlayerData.energy = (int)(MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus);
                else
                    mPlayerData.energy = value;
                GameEntry.Utils.AddValue(TriggerTag.Energy, mPlayerData.energy.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int MaxEnergy
        {
            get
            {
                return mPlayerData.maxEnergy;
            }
            set
            {
                mPlayerData.maxEnergy = value;
                GameEntry.Utils.AddValue(TriggerTag.MaxEnergy, mPlayerData.maxEnergy.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int MaxAp
        {
            get
            {
                return mPlayerData.maxAp;
            }
            set
            {
                mPlayerData.maxAp = value;
                GameEntry.Utils.AddValue(TriggerTag.MaxAp, mPlayerData.maxAp.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Ap
        {
            get
            {
                return mPlayerData.ap;
            }
            set
            {
                mPlayerData.ap = value;
                GameEntry.Utils.AddValue(TriggerTag.Ap, mPlayerData.ap.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public Week Week
        {
            get
            {
                return (Week)(Day % 7);
            }
        }
        public int Day
        {
            get
            {
                return mPlayerData.day;
            }
            set
            {
                mPlayerData.day = value;
                GameEntry.Utils.AddValue(TriggerTag.Day, mPlayerData.day.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
        public int Rent
        {
            get
            {
                return mPlayerData.rent;
            }
            set
            {
                mPlayerData.rent = value;
                GameEntry.Utils.AddValue(TriggerTag.Rent, mPlayerData.rent.ToString());
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public int maxEnergy;
        public int energy;
        public int money;
        public int maxAp;
        public int ap;
        public int day;
        public int rent;
        public int guideID;
        public List<PlayerItemData> items = new List<PlayerItemData>();
        public List<int> recipes = new List<int>();

        public Dictionary<ValueTag, int> GetValueTag(Dictionary<ValueTag, int> dic)
        {
            if (maxAp != 0)
                dic.Add(ValueTag.MaxAp, maxAp);
            if (ap != 0)
                dic.Add(ValueTag.Ap, ap);
            if (money != 0)
                dic.Add(ValueTag.Money, money);
            return dic;
        }
    }


    public enum ValueTag
    {
        MaxAp,
        Ap,
        Money,
        Favor,
        Wisdom,
        Stamina,
        Charm
    }
}
