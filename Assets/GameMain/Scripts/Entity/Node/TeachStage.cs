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
    protected override void ShowCharacter(CharData left, CharData middle, CharData right)
    {

    }

    protected override void ShowCharacter(CharData charData, GameMain.DialogPos pos)
    {

    }

    public override void ShowCharacter(ChatData chatData)
    {
        if (chatData.middle.charSO != null)
        {
            baseCharacter.SetData(chatData.middle.charSO);
        }
        baseCharacter.SetAction(chatData.middle.actionData);
    }

    public override void SetBackground(Sprite sprite)
    {
        
    }

    public override void SetBackground(string path)
    {
        
    }
}
