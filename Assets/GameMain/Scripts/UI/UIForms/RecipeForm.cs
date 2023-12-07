using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;
using System;

namespace GameMain
{
    public class RecipeForm : UIFormLogic
    {
        [SerializeField] private Image mProduct;
        [SerializeField] private Image mTool;
        [SerializeField] private GameObject mRecipeItem;
        [SerializeField] private Transform mLeftCanvas;
        [SerializeField] private Transform mRightCanvas;
        [SerializeField] private Button exitBtn;

        private List<RecipeItem> nodeItems = new List<RecipeItem>();
        private List<RecipeItem> recipeItems = new List<RecipeItem>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            GameEntry.Base.GameSpeed = 0f;
            ShowNodes();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButton(1))
            {
                GameEntry.UI.CloseUIForm(this.UIForm);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            GameEntry.Base.GameSpeed = 1f;
            ClearNodes();
        }

        private void ShowNodes()
        {
            ClearNodes();
            IDataTable<DRRecipe> dtRecipe = GameEntry.DataTable.GetDataTable<DRRecipe>();
            foreach (DRRecipe recipe in dtRecipe.GetAllDataRows())
            {
                RecipeData recipeData = new RecipeData(recipe);
                foreach (NodeTag nodeTag in recipeData.products)
                {
                    GameObject go = GameObject.Instantiate(mRecipeItem, mRightCanvas);
                    RecipeItem recipeItem = go.GetComponent<RecipeItem>();
                    recipeItem.SetData(new RecipeData(recipe),nodeTag, ShowRecipe);
                    nodeItems.Add(recipeItem);
                }
            }
        }

        private void ClearNodes()
        {
            foreach (RecipeItem go in nodeItems)
            {
                Destroy(go.gameObject);
            }
            nodeItems.Clear();
        }

        private void ShowRecipe(RecipeData recipeData,NodeTag product,RecipeItem item)
        {
            ClearRecipe();
            foreach (NodeTag nodeTag in recipeData.materials)
            {
                GameObject go = GameObject.Instantiate(mRecipeItem, mLeftCanvas);
                RecipeItem recipeItem = go.GetComponent<RecipeItem>();
                recipeItem.SetData(nodeTag);
                recipeItems.Add(recipeItem);
            }
            foreach (RecipeItem recipeItem in nodeItems)
            {
                recipeItem.Choice = false;
            }
            item.Choice = true;
            mTool.gameObject.SetActive(true);
            mProduct.gameObject.SetActive(true);
            mTool.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)recipeData.tool).SpritePath);
            mProduct.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)recipeData.products[0]).SpritePath);
        }

        private void ClearRecipe()
        {
            foreach (RecipeItem go in recipeItems)
            {
                Destroy(go.gameObject);
            }
            recipeItems.Clear();
        }
    }

    public class RecipeData
    {
        public int Id { get; set; }
        public List<NodeTag> materials = new List<NodeTag>();
        public List<NodeTag> products = new List<NodeTag>();
        public NodeTag tool;
        public bool IsCoffee { get; set; }

        public RecipeData() { }
        public RecipeData(DRRecipe dRRecipe)
        {
            Id = dRRecipe.Id;
            materials = TransToEnumList(dRRecipe.Recipe);
            products = TransToEnumList(dRRecipe.Product);
            tool = TransToEnum(dRRecipe.Tool);
            IsCoffee = dRRecipe.IsCoffee;
        }

        public NodeTag TransToEnum(string value)
        {
            return (NodeTag)Enum.Parse(typeof(NodeTag), value);
        }

        public List<NodeTag> TransToEnumList(List<string> valueList)
        {
            List<NodeTag> temp = new List<NodeTag>();
            foreach (var VarIAble in valueList)
            {
                temp.Add((NodeTag)Enum.Parse(typeof(NodeTag), VarIAble));
            }
            return temp;
        }
    }
}
