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

        private void ShowRecipe(RecipeData recipeData,NodeTag product)
        {
            ClearRecipe();
            foreach (NodeTag nodeTag in recipeData.materials)
            {
                GameObject go = GameObject.Instantiate(mRecipeItem, mLeftCanvas);
                RecipeItem recipeItem = go.GetComponent<RecipeItem>();
                recipeItem.SetData(nodeTag);
                recipeItems.Add(recipeItem);
            }
            mTool.sprite = GameEntry.Utils.nodeSprites[(int)recipeData.tool];
            mProduct.sprite = GameEntry.Utils.nodeSprites[(int)product];
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
        public List<NodeTag> materials = new List<NodeTag>();
        public List<NodeTag> products = new List<NodeTag>();
        public NodeTag tool;

        public RecipeData() { }
        public RecipeData(DRRecipe dRRecipe)
        {
            materials = TransToEnumList(dRRecipe.Recipe);
            products = TransToEnumList(dRRecipe.Product);
            tool = TransToEnum(dRRecipe.Tool);
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
