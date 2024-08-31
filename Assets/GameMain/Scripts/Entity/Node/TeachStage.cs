using GameFramework.Event;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using Dialog;

public class TeachStage : BaseStage
{
    [SerializeField] protected BaseCharacter baseCharacter;

    private CharSO charSO=null;

    public override void ShowCharacter(ChatData chatData)
    {
        if (chatData.middleAction.charSO != null&&charSO==null)
        {
            baseCharacter.SetData(chatData.middleAction.charSO);
            charSO = chatData.middleAction.charSO;
        }
        baseCharacter.SetAction(chatData.middleAction.actionTag);
        baseCharacter.SetDiff(chatData.middleAction.diffTag);
    }
}
