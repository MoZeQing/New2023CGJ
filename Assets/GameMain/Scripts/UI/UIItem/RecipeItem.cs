using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItem : MonoBehaviour
{
    private Image mImage;
    private NodeTag mNodeTag; 
    private Button mBtn;
    private Action<NodeTag> mAction;

    private void Start()
    {
        mBtn= GetComponent<Button>();
        mImage= GetComponent<Image>();
    }

    public void SetData(NodeTag nodeTag,Action<NodeTag> action)
    { 
        mNodeTag= nodeTag;
        mAction= action;
        mImage.sprite = GameEntry.Utils.nodeSprites[(int)nodeTag];
        mBtn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        mAction(mNodeTag);
    }
}
