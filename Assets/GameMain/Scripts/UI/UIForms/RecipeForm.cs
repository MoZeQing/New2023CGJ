using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;
using System;
using DG.Tweening;

namespace GameMain
{
    public class RecipeForm : UIFormLogic
    {
        [SerializeField] private Image mProduct;
        [SerializeField] private Image mTool;
        [SerializeField] private GameObject mRecipeItem;
        [SerializeField] private Transform mCanvas;
        //[SerializeField] private Transform mRightCanvas;
        [SerializeField] private Transform canvas;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text pageText;

        private List<DRRecipe> dRRecipes = new List<DRRecipe>();
        [SerializeField] private List<RecipeItem> nodeItems = new List<RecipeItem>();//ÓÒ²à½Úµã
        private List<RecipeItem> recipeItems = new List<RecipeItem>();//×ó²àÏÔÊ¾À¸
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            dRRecipes.Clear();
            foreach (DRRecipe recipe in GameEntry.DataTable.GetDataTable<DRRecipe>().GetAllDataRows())
            {
                if(recipe.IsCoffee)
                    dRRecipes.Add(recipe);
            }

            leftBtn.interactable = false;
            rightBtn.interactable = dRRecipes.Count > nodeItems.Count;

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            index = 0;
            ShowItems();
            ClearRecipe();
            mTool.gameObject.SetActive(false);
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
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
            GameEntry.Base.GameSpeed = 1f;
            //ClearNodes();
        }
        private void ShowItems()
        {
            leftBtn.interactable = index != 0;
            pageText.text = (index / nodeItems.Count + 1).ToString();

            for (int i = 0; i < nodeItems.Count; i++)
            {
                if (index < dRRecipes.Count)
                {
                    RecipeData recipeData = new RecipeData(dRRecipes[index]);
                    nodeItems[i].SetData(recipeData,recipeData.products[0],ShowRecipe);
                }
                else
                    nodeItems[i].Hide();
                index++;
            }

            rightBtn.interactable = index < dRRecipes.Count;
        }
        private void Right()
        {
            ShowItems();
        }
        private void Left()
        {
            index -= 2 * nodeItems.Count;
            ShowItems();
        }
        private void UpdateItem()
        {
            index -= nodeItems.Count;
            ShowItems();
        }

        private void ShowNodes()
        {
            //ClearNodes();
            //IDataTable<DRRecipe> dtRecipe = GameEntry.DataTable.GetDataTable<DRRecipe>();
            //foreach (DRRecipe recipe in dtRecipe.GetAllDataRows())
            //{
            //    RecipeData recipeData = new RecipeData(recipe);
            //    GameObject go = GameObject.Instantiate(mRecipeItem, mRightCanvas);
            //    RecipeItem recipeItem = go.GetComponent<RecipeItem>();
            //    recipeItem.SetData(new RecipeData(recipe), new RecipeData(recipe).products[0], ShowRecipe);
            //    nodeItems.Add(recipeItem);
            //}
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
                GameObject go = GameObject.Instantiate(mRecipeItem, mCanvas);
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
            //mProduct.gameObject.SetActive(true);
            mTool.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)recipeData.tool).SpritePath);
            //mProduct.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)recipeData.products[0]).SpritePath);
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
