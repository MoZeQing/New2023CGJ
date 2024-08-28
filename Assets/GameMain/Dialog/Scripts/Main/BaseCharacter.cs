using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XNode;
using DG.Tweening;

public class BaseCharacter :MonoBehaviour
{
    [SerializeField] private Image mImage = null;
    public CharacterTag CharacterTag { get; set; }
    public CharSO CharSO { get; set; } = null;

    public void SetAction(ActionTag actionTag)
    {
        switch (actionTag)
        {
            case ActionTag.Jump:
                JumpAction();
                break;
            case ActionTag.Shake:
                ShakeAction();
                break;
        }
    }
    //左右抖动动画
    protected virtual void ShakeAction()
    {
        mImage.gameObject.transform.DOPause();
        mImage.gameObject.transform.localPosition = Vector3.zero;
        mImage.gameObject.transform.DOPunchPosition(Vector3.right * 100, 0.4f, 10);
    }
    //上下跳动动画
    protected virtual void JumpAction()
    {
        mImage.gameObject.transform.DOPause();
        mImage.gameObject.transform.localPosition = Vector3.zero;
        mImage.gameObject.transform.DOLocalJump(Vector3.zero, 200, 1, 0.3f, false);
    }
    public void SetDiff(DiffTag diffTag)
    {
        if (CharSO == null)
            return;
        mImage.sprite = CharSO.diffs[(int)diffTag];
    }
    public void SetData(CharSO charSO)
    {
        CharSO = charSO;
    }
    //入场动画
    public void Show()
    {
        CharacterTag = CharacterTag.Show;
        mImage.color = Color.gray;
        mImage.DOKill();
        mImage.DOColor(Color.white, 0.3f);
        mImage.gameObject.transform.localPosition += Vector3.right * 100f;
        mImage.gameObject.transform.DOLocalMoveX(0f, 0.3f);
    }

    public void Hide()
    {
        CharacterTag = CharacterTag.Hide;
        mImage.DOKill();
        //淡出
        mImage.DOFade(0, 0.3f);
        mImage.gameObject.transform.DOLocalMoveX(0, 0.3f);
    }

    public void MainShow()
    {
        mImage.DOKill();
        //主要角色亮起且稍稍变大，为什么现在没有DOScale直接调整啊？？？
        mImage.DOColor(Color.white, 0.3f);
        mImage.gameObject.transform.DOScale(1.05f, 0.3f);
        mImage.canvas.sortingOrder = 2;
    }

    public void SubShow()
    {
        mImage.DOKill();
        //非主要角色变灰
        mImage.DOColor(Color.grey, 0.3f);
        mImage.gameObject.transform.DOScale(1f, 0.3f);
        mImage.canvas.sortingOrder = 1;
    }
}
public enum CharacterTag
{ 
    Show,
    Hide
}
[System.Serializable]
public class ActionData
{
    public CharSO charSO;
    public DiffTag diffTag;
    public ActionTag actionTag;

    public ActionData() { }
    public ActionData(CharSO charSO)
    { 
        this.charSO = charSO;
        diffTag = DiffTag.Idle;
        actionTag = ActionTag.None;
    }
}
//差分Tag
public enum DiffTag
{
    Idle,
    Smile
}
//动作Tag
public enum ActionTag
{
    None,//无
    Jump,//上下跳动
    Shake//左右抖动
}

