using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class PlayerComponent : GameFrameworkComponent
    {
        private PlayerData mPlayerData=new PlayerData();

        private void Start()
        {
            mPlayerData.recipes = new List<RecipeData>();
            mPlayerData.items = new List<PlayerItemData>();
        }

        public void ClearItem()
        {
            mPlayerData.items.Clear();
        }
        public void AddItem(ItemData itemData, int num)
        {
            if (GetItem(itemData.itemTag) == null)
            {
                mPlayerData.items.Add(new PlayerItemData(itemData, num));
            }
            else
            {
                GetItem(itemData.itemTag).itemNum += num;
            }
        }
        public void AddItem(ItemData itemData, int num, bool equip)
        {
            if (GetItem(itemData.itemTag) == null)
            {
                PlayerItemData playerItem = new PlayerItemData(itemData, num);
                playerItem.equiping = equip;
                mPlayerData.items.Add(playerItem);
            }
            else
            {
                GetItem(itemData.itemTag).itemNum += num;
            }
        }
        public PlayerItemData GetItem(ItemTag itemTag)
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
            mPlayerData.recipes.Clear();
        }

        public void AddRecipe(RecipeData recipeData)
        {
            mPlayerData.recipes.Add(recipeData);
        }

        public void AddRecipe(int index)
        {
            RecipeData recipeData = new RecipeData(GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(index));
            AddRecipe(recipeData);
        }

        public bool HasRecipe(RecipeData recipeData)
        {
            return mPlayerData.recipes.Contains(recipeData);
        }

        public bool HasRecipe(NodeTag nodeTag)
        {
            foreach (RecipeData recipe in mPlayerData.recipes)
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
                if (value > MaxEnergy)
                    mPlayerData.energy = MaxEnergy;
                else
                    mPlayerData.energy = value;
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
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
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
                GameEntry.Event.FireNow(this, PlayerDataEventArgs.Create(mPlayerData));
            }
        }
    }
}
