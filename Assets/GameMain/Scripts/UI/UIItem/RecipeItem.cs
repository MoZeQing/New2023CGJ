using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RecipeItem : MonoBehaviour
{
    [SerializeField] private Image mImage;
    [SerializeField] private Image mChoice;
    private RecipeData mRecipe; 
    [SerializeField] private Button mBtn;
    private Action<RecipeData,NodeTag,RecipeItem> mAction;
    private NodeTag mNodeTag;

    private void Start()
    {
        mBtn= GetComponent<Button>();
        mImage= GetComponent<Image>();
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
        mRecipe = recipe;
        mAction= action;
        mNodeTag = nodeTag;
        mImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag).SpritePath);
        mBtn.onClick.AddListener(OnClick);
        if (!GameEntry.Player.HasRecipe(recipe.Id))
            mBtn.interactable = false;
    }

    public void SetData(NodeTag nodeTag)
    {
        mNodeTag = nodeTag;
        mImage.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)nodeTag).SpritePath);
    }

    private void OnClick()
    {
        mAction(mRecipe, mNodeTag,this);
    }
}
