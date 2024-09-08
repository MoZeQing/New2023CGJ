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
    [SerializeField] private Image mChoice;
    [SerializeField] private Image mIce;
    private RecipeData mRecipe; 
    [SerializeField] private Button mBtn;
    private Action<RecipeData,NodeTag,RecipeItem> mAction;
    private NodeTag mNodeTag;

    private void Start()
    {
        mBtn= GetComponent<Button>();
        mIconImg=this.transform.Find("Icon").GetComponent<Image>();
        mBGImg.GetComponent<Image>();
    }

    public bool Choice
    {
        set
        {
            mChoice.gameObject.SetActive(value);
        }
    }

    public void SetData(RecipeData recipe,NodeTag nodeTag,Action<RecipeData,NodeTag,RecipeItem> action)
    {
        this.gameObject.SetActive(true);
        mBtn.onClick.RemoveAllListeners();
        mRecipe = recipe;
        mAction= action;
        mNodeTag = nodeTag;
        mIconImg.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag).IconPath);
        mBGImg.sprite= Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag).BackgroundPath);
        mBtn.onClick.AddListener(OnClick);
        mBtn.interactable = GameEntry.Player.HasRecipe(recipe.Id);
        mChoice.gameObject.SetActive(false);
        DRNode node = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);
        mIce.gameObject.SetActive(node.Ice);
    }

    public void SetData(NodeTag nodeTag)
    {
        mNodeTag = nodeTag;
        mIconImg.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag).IconPath);
        mBGImg.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag).BackgroundPath);
        DRNode node = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag);
        mIce.gameObject.SetActive(node.Ice);
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
