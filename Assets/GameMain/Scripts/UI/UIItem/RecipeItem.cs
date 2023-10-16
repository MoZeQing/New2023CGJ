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
    private RecipeData mRecipe; 
    [SerializeField] private Button mBtn;
    private Action<RecipeData,NodeTag> mAction;
    private NodeTag mNodeTag;

    private void Start()
    {
        mBtn= GetComponent<Button>();
        mImage= GetComponent<Image>();
    }

    public void SetData(RecipeData recipe,NodeTag nodeTag,Action<RecipeData,NodeTag> action)
    {
        mRecipe = recipe;
        mAction= action;
        mNodeTag = nodeTag;
        mImage.sprite = GameEntry.Utils.nodeSprites[(int)nodeTag];
        mBtn.onClick.AddListener(OnClick);
    }

    public void SetData(NodeTag nodeTag)
    {
        mNodeTag = nodeTag;
        mImage.sprite = GameEntry.Utils.nodeSprites[(int)nodeTag];
    }

    private void OnClick()
    {
        mAction(mRecipe, mNodeTag);
    }
}
