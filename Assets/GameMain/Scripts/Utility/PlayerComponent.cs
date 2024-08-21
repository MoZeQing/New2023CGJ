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
        public List<RecipeData> recipes = new List<RecipeData>();//已解锁的配方

        private void Start()
        {
            recipes = new List<RecipeData>();
            mPlayerData.items = new List<PlayerItemData>();
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

        public void ClearRecipe()
        {
            recipes.Clear();
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

        public void AddRecipe(RecipeData recipeData)
        {
            if (!HasRecipe(recipeData.Id))
            {
                if (recipeData.IsCoffee)
                    GameEntry.UI.OpenUIForm(UIFormId.UnlockForm, recipeData);
                recipes.Add(recipeData);
            }
        }

        public void AddRecipe(int index)
        {
            if (HasRecipe(index))
                return;
            RecipeData recipeData = new RecipeData(GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(index));
            recipes.Add(recipeData);
        }
        public void RemoveRecipe(int index)
        {
            RecipeData recipeData = null;
            foreach (RecipeData recipe in recipes)
            {
                if (recipe.Id == index)
                    recipeData = recipe;
            }
            recipes.Remove(recipeData);
        }
        public void LoadGame(SaveLoadData saveLoadData)
        {
            recipes.Clear();
            foreach (int index in saveLoadData.recipes)
            {
                GameEntry.Player.recipes.Add(new RecipeData(GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(index)));
            }
        }

        public bool HasRecipe(int id)
        {
            foreach (RecipeData recipe in recipes)
            {
                if (recipe.Id==id)
                    return true;
            }
            return false;
        }

        public bool HasCoffeeRecipe(NodeTag nodeTag)
        {
            foreach (RecipeData recipe in recipes)
            {
                if (recipe.products.Contains(nodeTag))
                    return true;
            }
            return false;
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
                //_values[TriggerTag.Money] = mPlayerData.money.ToString();
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
                //_values[TriggerTag.Energy] = mPlayerData.energy.ToString();
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
                //_values[TriggerTag.MaxEnergy] = mPlayerData.maxEnergy.ToString();
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
                //_values[TriggerTag.MaxAp] = mPlayerData.maxAp.ToString();
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
                //_values[TriggerTag.Ap] = mPlayerData.ap.ToString();
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
                //_values[TriggerTag.Day] = mPlayerData.day.ToString();
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
                //_values[TriggerTag.Rent] = mPlayerData.rent.ToString();
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
    }
}
