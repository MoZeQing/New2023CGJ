using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseCharacter : MonoBehaviour
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
        if (CharacterTag == CharacterTag.Show)
            return;
        CharacterTag = CharacterTag.Show;
        mImage.color = Color.gray;
        mImage.DOKill();
        mImage.DOColor(Color.white, 0.5f);
    }

    public void Hide()
    {
        if (CharacterTag == CharacterTag.Hide)
            return;
        CharacterTag = CharacterTag.Hide;
        mImage.DOKill();
        //淡出
        mImage.DOColor(Color.clear, 0.5f);
        mImage.gameObject.transform.DOLocalMoveX(0, 0.5f);
        mImage.gameObject.transform.localScale = Vector3.one;
        mImage.canvas.sortingOrder = 1;
    }

    public void MainShow()
    {
        mImage.DOKill();
        mImage.DOColor(Color.white, 0.5f);
        mImage.gameObject.transform.DOScale(1.05f, 0.3f);
        mImage.canvas.sortingOrder = 2;
    }

    public void SubShow()
    {
        mImage.DOKill();
        mImage.DOColor(Color.grey, 0.3f);
        mImage.gameObject.transform.DOScale(1f, 0.3f);
        mImage.canvas.sortingOrder = 1;
    }
}
public enum CharacterTag
{
    None,
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
    Idle = 0,// 默认
    Happy = 1,// 开心
    Sad = 2,// 伤心
    Upset = 3,// 难过
    Fear = 4,// 胆怯
    Steady = 5// 坚定
}
//动作Tag
public enum ActionTag
{
    None = 0,//无
    Jump = 1,//上下跳动
    Shake = 2//左右抖动
}

