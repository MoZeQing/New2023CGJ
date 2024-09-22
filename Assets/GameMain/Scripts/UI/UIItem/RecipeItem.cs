using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RecipeItem : MonoBehaviour
{
    [SerializeField] private Image mIconImg;
    [SerializeField] private Image mBGImg;
    [SerializeField] private Image mChoiceImg;
    [SerializeField] private Image mIceImg;
    [SerializeField] private Image mBoundImg;
    [SerializeField] private Text mNameText;
    [SerializeField] private Button mBtn;
    private RecipeData mRecipe; 
    private Action<RecipeData,NodeTag,RecipeItem> mAction;
    private NodeTag mNodeTag;

    public bool Choice
    {
        set
        {
            mChoiceImg.gameObject.SetActive(value);
        }
    }

    public void SetData(RecipeData recipe,NodeTag nodeTag,Action<RecipeData,NodeTag,RecipeItem> action)
    {
        DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);

        this.gameObject.SetActive(true);
        mBtn.onClick.RemoveAllListeners();
        mRecipe = recipe;
        mAction= action;
        mNodeTag = nodeTag;
        mNameText.text = dRNode.Name;
        mBoundImg.sprite = Resources.Load<Sprite>(dRNode.BoundPath);
        mIconImg.sprite = Resources.Load<Sprite>(dRNode.IconPath);
        mBGImg.sprite= Resources.Load<Sprite>(dRNode.BackgroundPath);
        mBtn.onClick.AddListener(OnClick);
        mBtn.interactable = GameEntry.Player.HasRecipe(recipe.Id);
        mChoiceImg.gameObject.SetActive(false);
        DRNode node = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);
        mIceImg.gameObject.SetActive(node.Ice);
    }

    public void SetData(NodeTag nodeTag)
    {
        DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);

        mNodeTag = nodeTag;
        mNameText.text = dRNode.Name;
        mBoundImg.sprite = Resources.Load<Sprite>(dRNode.BoundPath);
        mIconImg.sprite = Resources.Load<Sprite>(dRNode.IconPath);
        mBGImg.sprite = Resources.Load<Sprite>(dRNode.BackgroundPath);
        DRNode node = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);
        mIceImg.gameObject.SetActive(node.Ice);
    }

    private void OnClick()
    {
        mAction(mRecipe, mNodeTag,this);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
