using GameFramework.Event;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

public class TeachStage : BaseStage
{
    [SerializeField] protected BaseCharacter baseCharacter;

    private CharSO charSO=null;
    protected override void ShowCharacter(CharData left, CharData middle, CharData right)
    {

    }

    protected override void ShowCharacter(CharData charData, GameMain.DialogPos pos)
    {

    }

    public override void ShowCharacter(ChatData chatData)
    {
        if (chatData.middle.charSO != null&&charSO==null)
        {
            baseCharacter.SetData(chatData.middle.charSO);
            charSO = chatData.middle.charSO;
        }
        baseCharacter.SetAction(chatData.middle.actionData);
    }
    public override void ShowDiff(DialogPos pos, DiffTag diffTag)
    {
        if (baseCharacter != null)
        {
            baseCharacter.SetDiff(diffTag);
        }
    }
    public override void SetBackground(Sprite sprite)
    {
        
    }
}
